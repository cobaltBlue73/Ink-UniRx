using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Ink.Runtime;
using InkUniRx.Presenters.Interfaces;
using InkUniRx.Settings;
using InkUniRx.ViewModels;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Utility.UniRx;

namespace InkUniRx.Presenters
{
    public class StoryPresenter : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private StoryPresenterSettings settings;
        [SerializeField] private TextAsset storyTextAsset;
        
        #endregion

        #region Properties
        public IReactiveCollection<StoryPathElement> StoryLog => _storyLog;
        
        #endregion

        #region Member Variables

        private Story _story;

        private bool _continueMaximally;
        private readonly ReactiveCollection<StoryPathElement> _storyLog = new ReactiveCollection<StoryPathElement>();

        private IStoryBeginningPresenter[] _storyBeginningPresenters;
        private IStoryEndingPresenter[] _storyEndingPresenters;
        private IStoryPathBeginningPresenter[] _storyPathBeginningPresenters;
        private IStoryPathEndingPresenter[] _storyPathEndingPresenters;
        private IStoryPathContentPresenter[] _storyPathContentPresenters;
        private IStoryPathChoicesPresenter[] _storyPathChoicesPresenters;

        private CancellationTokenSource _curTransitionCts;
        
        #endregion
        
        #region Unity Callbacks

        private void Awake()
        {
            _storyLog.AddTo(this);
            InitSubPresenters();
            InitSettings();
            InitPlayer();
        }

        private void OnDestroy()
        {
            _curTransitionCts?.Dispose();
        }

        #endregion

        #region Public Methods

        public void SetStoryTexAsset(TextAsset storyAsset)
        {
            storyTextAsset = storyAsset;
            InitPlayer();
        }
        
        [Button(nameof(BeginStory))]
        public void BeginStory(TextAsset storyAsset = null)
        {
            if (storyAsset) SetStoryTexAsset(storyAsset);
            RunStoryAsync().Forget();
        }
        
        public void SkipTransitions() => _curTransitionCts?.Cancel();

        public void ClearLogs() => _storyLog.Clear();

        #endregion
        
        #region Private Methods

        private async UniTaskVoid RunStoryAsync()
        {
            if (_story == null || !_story.canContinue) return;
            
            InitStory();
            
            await OnShowStoryBeginningAsync();
            
            do
            {
                await OnShowStoryPathBeginningAsync();
                
                var pathLineCount = 0;
                do
                {
                    ContinueNextLine();
                    await OnShowStoryTextAsync(++pathLineCount);

                } while (_story.canContinue);
                
                await OnShowStoryPathEndingAsync();

                if (!_story.HasChoices()) continue;

                await OnShowStoryChoiceAsync();

            } while (_story.canContinue);

            await OnShowStoryEndingAsync();

            CleanUpStory();
        }

        #region Helpers
        
        private void InitPlayer()
        {
            if(!storyTextAsset) return;

            _story = new Story(storyTextAsset.text);
        }

        private void InitSettings()
        {
            if(!settings) return;

            settings.ContinueMaximally.SetAndSubscribe(ref _continueMaximally, 
                val => _continueMaximally = val);
        }
     
        private async UniTask ShowStoryPresentersAsync(Func<CancellationToken, IEnumerable<UniTask>> onShow)
        {
            using (_curTransitionCts = new CancellationTokenSource())
            {
                await UniTask.WhenAll(onShow(_curTransitionCts.Token));
            }
            _curTransitionCts = null;
        }
        
        private async UniTask OnShowStoryBeginningAsync() =>
            await ShowStoryPresentersAsync(ct => 
                _storyBeginningPresenters.Select(t => 
                    t.OnShowStoryBeginningAsync(_story, ct)));
        
        private async UniTask OnShowStoryEndingAsync() =>
            await ShowStoryPresentersAsync(ct => 
                _storyEndingPresenters.Select(t => 
                    t.OnShowStoryEndingAsync(_story, ct)));
        
        private async UniTask OnShowStoryPathBeginningAsync() =>
            await ShowStoryPresentersAsync(ct => 
                _storyPathBeginningPresenters.Select(t => 
                    t.OnShowStoryPathBeginningAsync(_story, ct)));
        
        private async UniTask OnShowStoryPathEndingAsync() => 
            await ShowStoryPresentersAsync(ct => 
                _storyPathEndingPresenters.Select(t => 
                    t.OnShowStoryPathEndingAsync(_story, ct)));
        
        private async UniTask OnShowStoryTextAsync(int lineNumber)
        {
            var content = new StoryPathContent(_story, lineNumber, !_continueMaximally);
            _storyLog.Add(content);
            await ShowStoryPresentersAsync(ct =>
                _storyPathContentPresenters.Select(t =>
                    t.OnShowStoryPathContentAsync(content, ct)));
        }

        private async UniTask OnShowStoryChoiceAsync()
        {
            var choices = new StoryPathChoices(_story);
            _storyLog.Add(choices);
            await ShowStoryPresentersAsync(ct =>
                _storyPathChoicesPresenters.Select(t =>
                    t.OnShowStoryPathChoicesAsync(choices, ct)));

            await UniTask.WaitUntil(() => _story.canContinue);
        }

        private void ContinueNextLine()
        {
            if (_continueMaximally)
                _story.ContinueMaximally();
            else
                _story.Continue();
        }

        private void InitSubPresenters()
        {
            _storyBeginningPresenters = GetComponentsInChildren<IStoryBeginningPresenter>();
            _storyEndingPresenters = GetComponentsInChildren<IStoryEndingPresenter>();
            _storyPathBeginningPresenters = GetComponentsInChildren<IStoryPathBeginningPresenter>();
            _storyPathEndingPresenters = GetComponentsInChildren<IStoryPathEndingPresenter>();
            _storyPathContentPresenters = GetComponentsInChildren<IStoryPathContentPresenter>();
            _storyPathChoicesPresenters = GetComponentsInChildren<IStoryPathChoicesPresenter>();
        }
        
        private void InitStory()
        {
            ClearLogs();
        }

        private void CleanUpStory()
        {
            ClearLogs();
        }

        #endregion
        
        #endregion
        
    }
}