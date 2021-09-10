using System;
using System.Collections.Generic;
using System.Linq;
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
        
        
        
        #endregion

        #region Member Variables

        private Story _story;

        private bool _continueMaximally;

        private IStoryStartPresenter[] _storyStartPresenters;
        private IStoryEndPresenter[] _storyEndPresenters;
        private IStoryPathPreStartPresenter[] _storyPathPreStartPresenters;
        private IStoryPathPostEndPresenter[] _storyPathPostEndPresenters;
        private IStoryContentPresenter[] _storyContentPresenters;
        private IStoryChoicesPresenter[] _storyChoicesPresenters;

        private CancellationTokenSource _curTransitionCts;
        
        #endregion
        
        #region Unity Callbacks

        private void Awake()
        {
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
        
        #endregion
        
        #region Private Methods

        private async UniTaskVoid RunStoryAsync()
        {
            if (_story == null || !_story.canContinue) return;
            
            InitStory();
            
            await OnShowStoryStartAsync();
            
            do
            {
                await OnShowStoryPathPreStartAsync();
                
                var pathLineCount = 0;
                do
                {
                    ContinueNextLine();
                    await OnShowStoryContentAsync(++pathLineCount);

                } while (_story.canContinue);
                
                await OnShowStoryPathPostEndAsync();

                if (!_story.HasChoices()) continue;

                await OnShowStoryChoicesAsync();

            } while (_story.canContinue);

            await OnShowStoryEndAsync();

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
        
        private async UniTask OnShowStoryStartAsync() =>
            await ShowStoryPresentersAsync(ct => 
                _storyStartPresenters.Select(t => 
                    t.OnShowStoryStartAsync(_story, ct)));
        
        private async UniTask OnShowStoryEndAsync() =>
            await ShowStoryPresentersAsync(ct => 
                _storyEndPresenters.Select(t => 
                    t.OnShowStoryEndAsync(_story, ct)));
        
        private async UniTask OnShowStoryPathPreStartAsync() =>
            await ShowStoryPresentersAsync(ct => 
                _storyPathPreStartPresenters.Select(t => 
                    t.OnShowStoryPathPreStartAsync(_story, ct)));
        
        private async UniTask OnShowStoryPathPostEndAsync() => 
            await ShowStoryPresentersAsync(ct => 
                _storyPathPostEndPresenters.Select(t => 
                    t.OnShowStoryPathPostEndAsync(_story, ct)));
        
        private async UniTask OnShowStoryContentAsync(int lineNumber)
        {
            var content = new StoryContent(_story, lineNumber, !_continueMaximally);
           
            await ShowStoryPresentersAsync(ct =>
                _storyContentPresenters.Select(t =>
                    t.OnShowStoryContentAsync(content, ct)));
        }

        private async UniTask OnShowStoryChoicesAsync()
        {
            var choices = _story.currentChoices
                .Select(c => new StoryChoice(_story, c))
                .ToArray();
            
            await ShowStoryPresentersAsync(ct =>
                _storyChoicesPresenters.Select(t =>
                    t.OnShowStoryChoicesAsync(choices, ct)));

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
            _storyStartPresenters = GetComponentsInChildren<IStoryStartPresenter>();
            _storyEndPresenters = GetComponentsInChildren<IStoryEndPresenter>();
            _storyPathPreStartPresenters = GetComponentsInChildren<IStoryPathPreStartPresenter>();
            _storyPathPostEndPresenters = GetComponentsInChildren<IStoryPathPostEndPresenter>();
            _storyContentPresenters = GetComponentsInChildren<IStoryContentPresenter>();
            _storyChoicesPresenters = GetComponentsInChildren<IStoryChoicesPresenter>();
        }
        
        private void InitStory()
        {
            
        }

        private void CleanUpStory()
        {
            
        }

        #endregion
        
        #endregion
        
    }
}