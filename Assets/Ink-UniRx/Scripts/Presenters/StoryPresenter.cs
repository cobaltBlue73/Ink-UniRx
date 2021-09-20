using System.Threading;
using Cysharp.Threading.Tasks;
using Ink.Runtime;
using InkUniRx.Presenters.Events;
using InkUniRx.Settings;
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
        private StoryStart _storyStart;
        private StoryEnd _storyEnd;
        private StoryPathStart _storyPathStart;
        private StoryPathEnd _storyPathEnd;
        private StoryPathNewText _storyPathNewText;
        private StoryPathSelectChoice _storyPathSelectChoice;
        private bool _continueMaximally;
        private CancellationTokenSource _animationSkipCts;
        private CancellationTokenSource _storyCancelCts;
        
        #endregion
        
        #region Unity Callbacks

        private void Awake()
        {
            InitSettings();
            InitPlayer();
        }

        private void OnDestroy()
        {
            _animationSkipCts?.Dispose();
            _storyCancelCts?.Dispose();
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

        public void CancelStory()
        {
            _animationSkipCts?.Cancel();
            _storyCancelCts?.Cancel();
        }

        public void SkipAnimation() => _animationSkipCts?.Cancel();
        
        #endregion
        
        #region Private Methods

        private async UniTaskVoid RunStoryAsync()
        {
            if (_story == null || !_story.canContinue) return;
            
            using (_storyCancelCts = new CancellationTokenSource())
            {
                var cancelStoryToken = _storyCancelCts.Token;
                
                InitStory(cancelStoryToken);
                
                await WaitForStoryStartAsync();
            
                do
                {
                    await WaitForStoryPathStartAsync();
                    
                    do
                    {
                        ContinueNextLine();
                       
                        await WaitForStoryPathContinueAsync();
                            
                    } while (_story.canContinue && !cancelStoryToken.IsCancellationRequested);
                
                    await WaitForStoryPathEndAsync();

                    if (!_story.HasChoices()) continue;

                    await WaitForStoryPathSelectChoiceAsync(cancelStoryToken);

                } while (_story.canContinue && !cancelStoryToken.IsCancellationRequested);

                await WaitForStoryEndAsync();
            }
            
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
        
        private async UniTask WaitForStoryStartAsync()
        {
            using (_animationSkipCts = new CancellationTokenSource())
            {
                _storyStart.SetCancelAnimationToken(_animationSkipCts.Token);
                await AsyncMessageBroker.Default.PublishAsync(_storyStart);
            }
            _animationSkipCts = null;
        }

        private async UniTask WaitForStoryEndAsync()
        {
            using (_animationSkipCts = new CancellationTokenSource())
            {
                _storyEnd.SetCancelAnimationToken(_animationSkipCts.Token);
                await AsyncMessageBroker.Default.PublishAsync(_storyEnd);
            }
            _animationSkipCts = null;
        }

        private async UniTask WaitForStoryPathStartAsync()
        {
            using (_animationSkipCts = new CancellationTokenSource())
            {
                _storyPathStart.SetCancelAnimationToken(_animationSkipCts.Token);
                await AsyncMessageBroker.Default
                    .PublishAsync(_storyPathStart);
            }
            _animationSkipCts = null;
        }

        private async UniTask WaitForStoryPathEndAsync()
        {
            using (_animationSkipCts = new CancellationTokenSource())
            {
                _storyPathEnd.SetCancelAnimationToken(_animationSkipCts.Token);
                await AsyncMessageBroker.Default.PublishAsync(_storyPathEnd);
            }
            _animationSkipCts = null;
        }

        private async UniTask WaitForStoryPathContinueAsync()
        {
            using (_animationSkipCts = new CancellationTokenSource())
            {
                _storyPathNewText.SetCancelAnimationToken(_animationSkipCts.Token);
                await AsyncMessageBroker.Default.PublishAsync(_storyPathNewText);
            }
            _animationSkipCts = null;
        }

        private async UniTask WaitForStoryPathSelectChoiceAsync(CancellationToken cancelStoryToken)
        {
            using (_animationSkipCts = new CancellationTokenSource())
            {
                _storyPathSelectChoice.SetCancelAnimationToken(_animationSkipCts.Token);
                await AsyncMessageBroker.Default.PublishAsync(_storyPathSelectChoice);
            }
            _animationSkipCts = null;

            await _story.WaitUntilCanContinue(cancelStoryToken)
                .SuppressCancellationThrow();
        }

        private void ContinueNextLine()
        {
            if (_continueMaximally)
                _story.ContinueMaximally();
            else
                _story.Continue();
        }
        
        private void InitStory(CancellationToken cancelStoryToken)
        {
            _storyStart = new StoryStart(_story, cancelStoryToken);
            _storyEnd = new StoryEnd(_story, cancelStoryToken);
            _storyPathStart = new StoryPathStart(_story, cancelStoryToken);
            _storyPathEnd = new StoryPathEnd(_story, cancelStoryToken);
            _storyPathNewText = new StoryPathNewText(_story, cancelStoryToken);
            _storyPathSelectChoice = new StoryPathSelectChoice(_story, cancelStoryToken);
        }

        private void CleanUpStory()
        {
            _storyCancelCts = null;
            _animationSkipCts = null;
            _storyStart = null;
            _storyEnd = null;
            _storyPathStart = null;
            _storyPathEnd = null;
            _storyPathNewText = null;
            _storyPathSelectChoice = null;
        }

        #endregion
        
        #endregion
        
    }
}