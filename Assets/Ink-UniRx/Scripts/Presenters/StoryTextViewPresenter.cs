using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using InkUniRx.Presenters.Events;
using InkUniRx.Settings;
using InkUniRx.Views;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Utility.UniRx;

namespace InkUniRx.Presenters
{
    [RequireComponent(typeof(StoryTextView))]
    public abstract class StoryTextViewPresenter<TTextView> : MonoBehaviour 
        where TTextView: StoryTextView
    {
        #region Inspector
        
        [SerializeField, Required] protected TTextView textView;
        [SerializeField] protected bool trim;
        [SerializeField] protected bool ignoreWhiteSpaceText;
        [SerializeField, Required] protected StoryPresenterSettings settings;
        #endregion

        #region Variables

        private bool _autoContinue = true;
        private float _autoContinueDelay = 0;
        private IObservable<Unit> _whenContinue;

        #endregion

        #region Methods

        #region Unity Callbacks

        protected virtual void Reset()
        {
            if (!textView)
                textView = GetComponent<TTextView>();
        }

        protected virtual void Awake()
        {
            AsyncMessageBroker.Default
                .Subscribe<StoryPathNewText>(pathContinue => 
                    OnNewStoryTextAsync(pathContinue).ToObservable()).AddTo(this);

            if (settings)
            {
                settings.AutoContinue
                    .SetAndSubscribe(ref _autoContinue, 
                        val => _autoContinue = val)
                    .AddTo(this);

                settings.AutoContinueDelay
                    .SetAndSubscribe(ref _autoContinueDelay, 
                        val => _autoContinueDelay = val)
                    .AddTo(this);
            }

            _whenContinue = MessageBroker.Default.Receive<ContinueStory>().AsUnitObservable();
        }

        #endregion

        #region Story Callback

        protected abstract UniTask<Unit> OnNewStoryTextAsync(StoryPathNewText newStoryText);

        protected virtual async UniTask WaitForContinueAsync(StoryPathNewText newStoryText)
        {
            if (!newStoryText.Story.canContinue)
                return;

            var whenContinue = _whenContinue;

            if (_autoContinue)
            {
                whenContinue = whenContinue.Merge(Observable.Timer(TimeSpan.FromSeconds(_autoContinueDelay))
                    .AsUnitObservable());
            }

            await whenContinue.ToUniTask(true, newStoryText.CancelStoryToken)
                .SuppressCancellationThrow();
        }

        #endregion

        #endregion
    }
}