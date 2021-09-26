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
    [RequireComponent(typeof(StoryContentView))]
    public abstract class StoryContentViewPresenter<TContentView> : MonoBehaviour 
        where TContentView: StoryContentView
    {
        #region Inspector
        
        [SerializeField, Required] protected TContentView contentView;
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
            if (!contentView)
                contentView = GetComponent<TContentView>();
        }

        protected virtual void Awake()
        {
            AsyncMessageBroker.Default
                .Subscribe<StoryPathNewContent>(pathContinue => 
                    OnNewStoryContentAsync(pathContinue).ToObservable()).AddTo(this);

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

        protected abstract UniTask<Unit> OnNewStoryContentAsync(StoryPathNewContent newStoryContent);

        protected virtual async UniTask WaitForContinueAsync(StoryPathNewContent newStoryContent)
        {
            if (!newStoryContent.Story.canContinue)
                return;

            var whenContinue = _whenContinue;

            if (_autoContinue)
            {
                whenContinue = whenContinue.Merge(Observable.Timer(TimeSpan.FromSeconds(_autoContinueDelay))
                    .AsUnitObservable());
            }

            await whenContinue.ToUniTask(true, newStoryContent.CancelStoryToken)
                .SuppressCancellationThrow();
        }

        #endregion

        #endregion
    }
}