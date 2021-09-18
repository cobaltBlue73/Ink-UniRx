using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Ink.Runtime;
using InkUniRx.Presenters.Events;
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

        private CancellationTokenSource _animationSkipCTS;
        private CancellationTokenSource _storyCancelCTS;
        
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
            _animationSkipCTS?.Dispose();
            _storyCancelCTS?.Dispose();
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

        public void CancelStory() => _storyCancelCTS?.Cancel();
        
        public void SkipTransitions() => _animationSkipCTS?.Cancel();
        
        #endregion
        
        #region Private Methods

        private async UniTaskVoid RunStoryAsync()
        {
            if (_story == null || !_story.canContinue) return;
            
            InitStory();

            using (_storyCancelCTS = new CancellationTokenSource())
            {
                var cancelStoryToken = _storyCancelCTS.Token;
                
                await WaitForStoryStartAsync(cancelStoryToken);
            
                do
                {
                    await WaitForStoryPathStartAsync(cancelStoryToken);
                    
                    do
                    {
                        ContinueNextLine();
                       
                        await WaitForStoryPathContinueAsync(cancelStoryToken);

                    } while (_story.canContinue && !cancelStoryToken.IsCancellationRequested);
                
                    await WaitForStoryPathEndAsync(cancelStoryToken);

                    if (!_story.HasChoices()) continue;

                    await WaitForStoryPathChoiceSelectionAsync(cancelStoryToken);

                } while (_story.canContinue && !cancelStoryToken.IsCancellationRequested);

                await WaitForStoryEndAsync(cancelStoryToken);
    
            }

            _storyCancelCTS = null;
            
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
        
        private async UniTask WaitForStoryStartAsync(CancellationToken cancelStoryToken)
        {
            using (_animationSkipCTS = new CancellationTokenSource())
            {
                await AsyncMessageBroker.Default
                    .PublishAsync(new StoryStart(_story, 
                        cancelStoryToken, _animationSkipCTS.Token));
            }
            _animationSkipCTS = null;
        }

        private async UniTask WaitForStoryEndAsync(CancellationToken cancelStoryToken)
        {
            using (_animationSkipCTS = new CancellationTokenSource())
            {
                await AsyncMessageBroker.Default
                    .PublishAsync(new StoryEnd(_story, 
                        cancelStoryToken, _animationSkipCTS.Token));
            }
            _animationSkipCTS = null;
        }

        private async UniTask WaitForStoryPathStartAsync(CancellationToken cancelStoryToken)
        {
            using (_animationSkipCTS = new CancellationTokenSource())
            {
                await AsyncMessageBroker.Default
                    .PublishAsync(new StoryPathStart(_story, 
                        cancelStoryToken, _animationSkipCTS.Token));
            }
            _animationSkipCTS = null;
        }

        private async UniTask WaitForStoryPathEndAsync(CancellationToken cancelStoryToken)
        {
            using (_animationSkipCTS = new CancellationTokenSource())
            {
                await AsyncMessageBroker.Default
                    .PublishAsync(new StoryPathEnd(_story, 
                        cancelStoryToken, _animationSkipCTS.Token));
            }
            _animationSkipCTS = null;
        }

        private async UniTask WaitForStoryPathContinueAsync(CancellationToken cancelStoryToken)
        {
            using (_animationSkipCTS = new CancellationTokenSource())
            {
                await AsyncMessageBroker.Default
                    .PublishAsync(new StoryPathContinue(_story, 
                        cancelStoryToken, _animationSkipCTS.Token));
            }
            _animationSkipCTS = null;
        }

        private async UniTask WaitForStoryPathChoiceSelectionAsync(CancellationToken cancelStoryToken)
        {
            using (_animationSkipCTS = new CancellationTokenSource())
            {
                await AsyncMessageBroker.Default
                    .PublishAsync(new StoryPathChoiceSelection(_story, 
                        cancelStoryToken, _animationSkipCTS.Token));
            }
            _animationSkipCTS = null;

            await UniTask.WhenAll(_story.OnMakeChoiceAsObservable().ToUniTask(true, cancelStoryToken),
                UniTask.WaitUntil(() => _story.canContinue, cancellationToken: cancelStoryToken))
                .SuppressCancellationThrow();
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