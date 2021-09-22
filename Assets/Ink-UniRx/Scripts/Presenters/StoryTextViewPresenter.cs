using System;
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

        protected bool AutoContinue = true;
        protected float AutoContinueDelay = 0;
        protected IObservable<Unit> WhenContinue;

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
                    .SetAndSubscribe(ref AutoContinue, 
                        val => AutoContinue = val)
                    .AddTo(this);

                settings.AutoContinueDelay
                    .SetAndSubscribe(ref AutoContinueDelay, 
                        val => AutoContinueDelay = val)
                    .AddTo(this);
            }

            WhenContinue = MessageBroker.Default.Receive<ContinueStory>().AsUnitObservable();
        }

        #endregion

        #region Story Callback

        protected abstract UniTask<Unit> OnNewStoryTextAsync(StoryPathNewText newStoryText);

        #endregion

        #endregion
    }
}