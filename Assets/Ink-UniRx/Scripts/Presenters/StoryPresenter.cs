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
        private IStoryTextPresenter[] _storyLinePresenters;
        
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
            _storyLinePresenters = GetComponentsInChildren<IStoryTextPresenter>();
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

        /*public void SubmitBeginStoryTransition(IStoryBeginningPresenter beginningPresenter) => 
            _storyBeginningPresenters.Add(beginningPresenter);
        
        public void SubmitBeginPathTransition(IStoryPathBeginningPresenter beginningPresenter) => 
            _storyPathBeginningPresenters.Add(beginningPresenter);
        
        public void SubmitEndPathTransition(IStoryPathEndingPresenter endingPresenter) => 
            _storyPathEndingPresenters.Add(endingPresenter);
        
        public void SubmitNewLineTransition(IStoryTextPresenter presenter) => 
            _storyLinePresenters.Add(presenter);*/
        
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
                    await OnShowStoryLineAsync();

                } while (_story.canContinue);
                
                await OnShowStoryPathEndingAsync();

                if (!_story.HasChoices()) continue;

                await WaitForChoiceSelection();

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
        
        private async UniTask OnShowStoryLineAsync() =>
            await ShowStoryPresentersAsync(ct => 
                _storyLinePresenters.Select(t => 
                    t.OnShowStoryTextAsync(_story, ct)));
        
        private void ContinueNextLine()
        {
            if (_continueMaximally)
                _story.ContinueMaximally();
            else
                _story.Continue();
        }
        
        private async UniTask WaitForChoiceSelection()
        {
            await _whenChoiceSelected.ToUniTask(true);
            // await UniTask.WhenAll(_whenChoiceSelected.ToUniTask(true), 
            //     UniTask.WaitUntil(()=> _story.canContinue));
        }

        private void InitStory()
        {
           // SortTransitions();
        }

        private void CleanUpStory()
        {
          
        }
        
        /*void SortTransitions()
        {
            _storyBeginningPresenters.Sort();
            _storyPathBeginningPresenters.Sort();
            _storyPathEndingPresenters.Sort();
            _storyLinePresenters.Sort();
        }
        
        void ClearTransitions()
        {
            _storyBeginningPresenters.Clear();
            _storyPathBeginningPresenters.Clear();
            _storyPathEndingPresenters.Clear();
            _storyLinePresenters.Clear();
        }*/

        #endregion
        
        #endregion
        
    }
}