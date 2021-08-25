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
        
        [SerializeField] private StoryVM storyVM;

        #endregion

        #region Properties
        
        public StoryVM StoryVM => storyVM;
        public StoryPlayerSettings Settings => _settings ??= StoryPlayerSettings.Instance;
        
        public IObservable<Unit> WhenStoryBegins => _whenStoryBegins.AsObservable();
        public IObservable<Unit> WhenStoryEnds => _whenStoryEnds.AsObservable();
        public IObservable<Unit> WhenPathBegins => _whenPathBegins.AsObservable();
        public IObservable<Unit> WhenPathEnds => _whenPathEnds.AsObservable();
        public IObservable<Unit> WhenNewLine => _whenNewLine.AsObservable();
        public IObservable<Choice> WhenChoiceSelected => _whenChoiceSelected.AsObservable();
        
        #endregion

        #region Member Variables

        private Story _story;
        private StoryPlayerSettings _settings;
        private bool _continueMaximally;
        private bool _autoContinue;
        
        private readonly ReactiveCommand _continueCommand = new ReactiveCommand();
        
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
        
        #endregion
        
        #region Unity Callbacks

        private void Awake()
        {
            if(storyVM) InitTeller();
            Settings.AutoContinue.SetAndSubscribe(ref _autoContinue, 
                val => _autoContinue = val)
                .AddTo(this);
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
            _continueCommand.Dispose();
            _storyDisposables.Clear();
            ClearTransitions();
            _curTransitionCts?.Dispose();
        }

        #endregion

        #region Public Methods

        public void SetStoryVM(StoryVM storyVM)
        {
            this.storyVM = storyVM;
            InitTeller();
        }

        [ContextMenu(nameof(BeginStory))]
        public void BeginStory() => RunStoryAsync().Forget();

        [ContextMenu(nameof(ContinueStory))]
        public void ContinueStory()
        {
            CancelTransitions();
            _continueCommand.Execute();
        }
        
        public void SelectChoice(int choiceIndex)
        {
            CancelTransitions();
            _story?.ChooseChoiceIndex(choiceIndex);
        }
        
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
            await RunStoryTransitionsAsync(ct => 
                _beginStoryTransitions.Select(t => 
                    t.RunBeginStoryTransitionAsync(ct)));
            
            do
            {
                _whenPathBegins.OnNext(Unit.Default);
                await RunStoryTransitionsAsync(ct => 
                    _beginPathTransitions.Select(t => 
                        t.RunBeginPathTransitionAsync(ct)));
                
                do
                {
                    var _ = _continueMaximally ? 
                        _story.ContinueMaximally() : 
                        _story.Continue();

                    await RunStoryTransitionsAsync(ct => 
                        _newLineTransitions.Select(t => 
                            t.RunNewLineTransitionAsync(ct)));
                    
                    if(!_story.canContinue) continue;
                    
                    await WaitForContinueAsync();
                    
                } while (_story.canContinue);
                
                _whenPathEnds.OnNext(Unit.Default);
                await RunStoryTransitionsAsync(ct => 
                    _endPathTransitions.Select(t => 
                        t.RunEndPathTransitionAsync(ct)));

                if (!_story.HasChoices()) continue;

                await _whenChoiceSelected.ToUniTask(true);

            } while (_story.canContinue);
            
            _whenStoryEnds.OnNext(Unit.Default);
            
            CleanUpStory();
        }

        #region Helpers
        
        private void InitTeller()
        {
            _story = storyVM.Story;
            _continueMaximally = Settings.ContinueMaximally;
            _storyDisposables.Clear();
            _story.OnContinueAsObservable().AsUnitObservable().Subscribe(_whenNewLine).AddTo(_storyDisposables);
            _story.OnMakeChoiceAsObservable().Subscribe(_whenChoiceSelected).AddTo(_storyDisposables);
        }
     
        private async UniTask RunStoryTransitionsAsync(Func<CancellationToken, IEnumerable<UniTask>> onTransition)
        {
            using (_curTransitionCts = new CancellationTokenSource())
            {
                await UniTask.WhenAll(onTransition(_curTransitionCts.Token));
            }
            _curTransitionCts = null;
        }
        
        private IObservable<Unit> GetWhenNewLine()
        {
            return _story.OnContinueAsObservable();
        }

        private IObservable<Choice> GetWhenChoiceSelected()
        {
            return _story.OnMakeChoiceAsObservable();
        }
        
        private async UniTask WaitForContinueAsync()
        {
            if (!_autoContinue) await _continueCommand;
        }

        private void InitStory()
        {
           SortTransitions();
        }

        private void CleanUpStory()
        {
          
        }
        
        private void CancelTransitions()
        {
            _curTransitionCts?.Cancel();
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