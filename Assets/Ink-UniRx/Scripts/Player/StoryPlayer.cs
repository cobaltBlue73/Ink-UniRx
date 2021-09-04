using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Ink.Runtime;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using Utility.UniRx;

namespace InkUniRx
{
    public class StoryPlayer : MonoBehaviour
    {
        #region Internals

        #endregion
        
        #region Inspector
        
        [SerializeField] private StoryPlayerSettings settings;
        [SerializeField] private TextAsset storyTextAsset;
        
        #endregion

        #region Properties
        
        public IObservable<Unit> WhenStoryBegins => _whenStoryBegins.AsObservable();
        public IObservable<Unit> WhenStoryEnds => _whenStoryEnds.AsObservable();
        public IObservable<Unit> WhenPathBegins => _whenPathBegins.AsObservable();
        public IObservable<Unit> WhenPathEnds => _whenPathEnds.AsObservable();
        public IObservable<Unit> WhenNewLine => _whenNewLine.AsObservable();
        public IObservable<Choice> WhenChoiceSelected => _whenChoiceSelected.AsObservable();
        
        #endregion

        #region Member Variables

        private Story _story;
        private bool _continueMaximally;
        private bool _autoContinue;
        private float _autoContinueDelay;
        
        private readonly ReactiveCommand _continueCmd = new ReactiveCommand();
        
        private readonly Subject<Unit> _whenStoryBegins = new  Subject<Unit>();
        private readonly Subject<Unit> _whenStoryEnds = new  Subject<Unit>();
        private readonly Subject<Unit> _whenPathBegins = new  Subject<Unit>();
        private readonly Subject<Unit> _whenPathEnds = new  Subject<Unit>();
        private readonly Subject<Unit> _whenNewLine = new Subject<Unit>();
        private readonly Subject<Choice> _whenChoiceSelected = new Subject<Choice>();
        
        private readonly List<IBeginStoryTransition> _beginStoryTransitions = new List<IBeginStoryTransition>();
        private readonly List<IBeginPathTransition> _beginPathTransitions = new List<IBeginPathTransition>();
        private readonly List<IEndPathTransition> _endPathTransitions = new List<IEndPathTransition>();
        private readonly List<INewLineTransition> _newLineTransitions = new List<INewLineTransition>();
        private readonly CompositeDisposable _storyDisposables = new CompositeDisposable();

        private CancellationTokenSource _curTransitionCts;

        private Func<string> _continue;

        #endregion
        
        #region Unity Callbacks

        private void Awake()
        {
            InitSettings();
            InitPlayer();
            _storyDisposables.AddTo(this);
            _continueCmd.AddTo(this);
        }

        private void OnDestroy()
        {
            void CleanUpSubject<T>(Subject<T> subject)
            {
                subject.OnCompleted();
                subject.Dispose();
            }
            
            CleanUpSubject(_whenStoryBegins);
            CleanUpSubject(_whenStoryEnds);
            CleanUpSubject(_whenPathBegins);
            CleanUpSubject(_whenPathEnds);
            CleanUpSubject(_whenNewLine);
            CleanUpSubject(_whenChoiceSelected);
            ClearTransitions();
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

        [ContextMenu(nameof(BeginStory))]
        public void BeginStory() => RunStoryAsync().Forget();

        [ContextMenu(nameof(ContinueNext))]
        public void ContinueNext()
        {
            SkipTransitions();
            _continueCmd.Execute();
        }

        public void SelectChoice(int choiceIndex)
        {
            SkipTransitions();
            _story?.ChooseChoiceIndex(choiceIndex);
        }

        [ContextMenu(nameof(SkipTransitions))]
        public void SkipTransitions() => _curTransitionCts?.Cancel();

        public void SubmitBeginStoryTransition(IBeginStoryTransition transition) => 
            _beginStoryTransitions.Add(transition);
        
        public void SubmitBeginPathTransition(IBeginPathTransition transition) => 
            _beginPathTransitions.Add(transition);
        
        public void SubmitEndPathTransition(IEndPathTransition transition) => 
            _endPathTransitions.Add(transition);
        
        public void SubmitNewLineTransition(INewLineTransition transition) => 
            _newLineTransitions.Add(transition);
        
        #endregion
        
        #region Private Methods
        
        private async UniTaskVoid RunStoryAsync()
        {
            if (_story == null || !_story.canContinue) return;
            
            InitStory();
            
            _whenStoryBegins.OnNext(Unit.Default);
            await PlayBeginStoryTransitionsAsync();
            
            do
            {
                _whenPathBegins.OnNext(Unit.Default);
                await PlayBeginPathTransitionsAsync();
                
                do
                {
                    _continue();
                    await PlayNewLineTransitionsAsync();
                     
                    if(!_story.canContinue) continue;
                    
                    await WaitForContinueAsync();
                    
                } while (_story.canContinue);
                
                _whenPathEnds.OnNext(Unit.Default);
                await PlayEndPathTransitionsAsync();

                if (!_story.HasChoices()) continue;

                await WaitForChoiceSelection();

            } while (_story.canContinue);
            
            _whenStoryEnds.OnNext(Unit.Default);
            
            CleanUpStory();
        }

        #region Helpers
        
        private void InitPlayer()
        {
            if(!storyTextAsset) return;

            _story = new Story(storyTextAsset.text);
            if (_continueMaximally)
                _continue = _story.ContinueMaximally;
            else
                _continue = _story.Continue;
            
            _storyDisposables.Clear();
            _story.OnContinueAsObservable().Subscribe(_whenNewLine).AddTo(_storyDisposables);
            _story.OnMakeChoiceAsObservable().Subscribe(_whenChoiceSelected).AddTo(_storyDisposables);
        }

        private void InitSettings()
        {
            if(!settings) return;

            _continueMaximally = !settings.ContinueEachLine;
            settings.AutoContinue.SetAndSubscribe(ref _autoContinue, 
                val => _autoContinue = val).AddTo(this);
            settings.AutoContinueDelay.SetAndSubscribe(ref _autoContinueDelay, 
                val => _autoContinueDelay = val).AddTo(this);
        }
     
        private async UniTask PlayStoryTransitionsAsync(Func<CancellationToken, IEnumerable<UniTask>> onTransition)
        {
            _curTransitionCts = new CancellationTokenSource();
            await UniTask.WhenAll(onTransition(_curTransitionCts.Token));
            _curTransitionCts.Dispose();
            _curTransitionCts = null;
        }
        
        private async UniTask PlayBeginStoryTransitionsAsync() =>
            await PlayStoryTransitionsAsync(ct => 
                _beginStoryTransitions.Select(t => 
                    t.PlayBeginStoryTransitionAsync(ct)));
        
        private async UniTask PlayBeginPathTransitionsAsync() =>
            await PlayStoryTransitionsAsync(ct => 
                _beginPathTransitions.Select(t => 
                    t.PlayBeginPathTransitionAsync(ct)));
        
        private async UniTask PlayNewLineTransitionsAsync() =>
            await PlayStoryTransitionsAsync(ct => 
                _newLineTransitions.Select(t => 
                    t.PlayNewLineTransitionAsync(ct)));
        
        private async UniTask PlayEndPathTransitionsAsync() => 
            await PlayStoryTransitionsAsync(ct => 
                _endPathTransitions.Select(t => 
                    t.PlayEndPathTransitionAsync(ct)));

        private async UniTask WaitForContinueAsync()
        {
            if (!_autoContinue)
            {
                await _continueCmd.ToUniTask(true);
                return;
            }

            await UniTask.WhenAny(
                UniTask.Delay(TimeSpan.FromSeconds(_autoContinueDelay)),
                _continueCmd.ToUniTask(true));
            
        }

        private async UniTask WaitForChoiceSelection()
        {
            await _whenChoiceSelected.ToUniTask(true);
        }

        private void InitStory()
        {
           SortTransitions();
        }

        private void CleanUpStory()
        {
          
        }
        
        void SortTransitions()
        {
            _beginStoryTransitions.Sort();
            _beginPathTransitions.Sort();
            _endPathTransitions.Sort();
            _newLineTransitions.Sort();
        }
        
        void ClearTransitions()
        {
            _beginStoryTransitions.Clear();
            _beginPathTransitions.Clear();
            _endPathTransitions.Clear();
            _newLineTransitions.Clear();
        }

        #endregion
        
        #endregion
        
    }
}