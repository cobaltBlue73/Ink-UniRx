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
        [SerializeField, Required] protected StoryPresenterSettings settings;
        #endregion

        #region Variables

        protected bool autoContinue = true;
        protected float autoContinueDelay = 0;

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
                    .SetAndSubscribe(ref autoContinue, 
                        val => autoContinue = val)
                    .AddTo(this);

                settings.AutoContinueDelay
                    .SetAndSubscribe(ref autoContinueDelay, 
                        val => autoContinueDelay = val)
                    .AddTo(this);
            }
        }

        #endregion

        #region Story Callback

        protected abstract UniTask<Unit> OnNewStoryTextAsync(StoryPathNewText newStoryText);

        #endregion

        #endregion
    }
}