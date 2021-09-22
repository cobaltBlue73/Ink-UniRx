using System;
using Ink.Runtime;
using InkUniRx.Presenters.Events;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Ink_UniRx.Scripts.KeyBinders
{
    public abstract class KeyBinder : MonoBehaviour
    {
        #region Unity Callbacks

        protected virtual void Awake()
        {
            AsyncMessageBroker.Default.Subscribe<StoryStart>(OnStoryStart).AddTo(this);
            AsyncMessageBroker.Default.Subscribe<StoryEnd>(OnStoryEnd).AddTo(this);
            Disposables.AddTo(this);
        }

        #endregion

        #region Variables

        protected Story Story;
        protected CompositeDisposable Disposables = new CompositeDisposable();

        #endregion

        #region Callbacks

        protected virtual IObservable<Unit> OnStoryStart(StoryStart storyStart)
        {
            Story = storyStart.Story;
            
            this.UpdateAsObservable()
                .Where(_ => GetKeyPress())
                .Subscribe(_ => OnKeyPress())
                .AddTo(Disposables);
            
            return Observable.Empty<Unit>();
        }

        protected virtual IObservable<Unit> OnStoryEnd(StoryEnd storyEnd)
        {
            Story = null;
            Disposables.Clear();
            return Observable.Empty<Unit>();
        }

        protected abstract bool GetKeyPress();

        protected abstract void OnKeyPress();

        #endregion
    }
}