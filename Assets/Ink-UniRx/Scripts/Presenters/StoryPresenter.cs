using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Ink.Runtime;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using Utility.UniRx;

namespace InkUniRx
{
    public class StoryPresenter : MonoBehaviour
    {
        #region Internals

        #endregion
        
        #region Inspector

        [SerializeField] private StoryPresenterSettings settings;
        [SerializeField] private TextAsset storyTextAsset;
        
        #endregion

        #region Properties

        #endregion

        #region Member Variables

        private Story _story;

        private bool _continueMaximally;

        private IStoryBeginningPresenter[] _storyBeginningPresenters;
        private IStoryEndingPresenter[] _storyEndingPresenters;
        private IStoryPathBeginningPresenter[] _storyPathBeginningPresenters;
        private IStoryPathEndingPresenter[] _storyPathEndingPresenters;
        private IStoryTextPresenter[] _storyTextPresenters;
        private IStoryChoicePresenter[] _storyChoicePresenters;
        
        private readonly CompositeDisposable _storyDisposables = new CompositeDisposable();
        private IObservable<Unit> _whenChoiceSelected;
        private CancellationTokenSource _curTransitionCts;

        #endregion
        
        #region Unity Callbacks

        private void Awake()
        {
            _storyBeginningPresenters = GetComponentsInChildren<IStoryBeginningPresenter>();
            _storyEndingPresenters = GetComponentsInChildren<IStoryEndingPresenter>();
            _storyPathBeginningPresenters = GetComponentsInChildren<IStoryPathBeginningPresenter>();
            _storyPathEndingPresenters = GetComponentsInChildren<IStoryPathEndingPresenter>();
            _storyTextPresenters = GetComponentsInChildren<IStoryTextPresenter>();
            _storyChoicePresenters = GetComponentsInChildren<IStoryChoicePresenter>();
            
            InitSettings();
            InitPlayer();
            _storyDisposables.AddTo(this);
        }

        private void OnDestroy()
        {
            _storyDisposables.Clear();
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
            
            await OnShowStoryBeginningAsync();
            
            do
            {
                await OnShowStoryPathBeginningAsync();
                
                do
                {
                    ContinueNextLine();
                    await OnShowStoryTextAsync();

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
            
            _storyDisposables.Clear();
            _whenChoiceSelected = _story.OnMakeChoiceAsObservable().AsUnitObservable();
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
                    t.OnShowPathBeginningAsync(_story, ct)));
        
        private async UniTask OnShowStoryPathEndingAsync() => 
            await ShowStoryPresentersAsync(ct => 
                _storyPathEndingPresenters.Select(t => 
                    t.OnShowPathEndingAsync(_story, ct)));
        
        private async UniTask OnShowStoryTextAsync() =>
            await ShowStoryPresentersAsync(ct => 
                _storyTextPresenters.Select(t => 
                    t.OnShowStoryTextAsync(_story, ct)));
        
        private async UniTask OnShowStoryChoiceAsync() =>
            await ShowStoryPresentersAsync(ct => 
                _storyChoicePresenters.Select(t => 
                    t.OnShowStoryChoiceAsync(_story, ct)));
        
        private void ContinueNextLine()
        {
            if (_continueMaximally)
                _story.ContinueMaximally();
            else
                _story.Continue();
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