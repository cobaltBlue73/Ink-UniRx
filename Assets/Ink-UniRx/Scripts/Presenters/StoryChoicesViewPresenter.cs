using Cysharp.Threading.Tasks;
using Ink.Runtime;
using InkUniRx.Presenters.Events;
using InkUniRx.Views;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace InkUniRx.Presenters
{
    [RequireComponent(typeof(StoryChoicesView))]
    public abstract class StoryChoicesViewPresenter<TChoicesView>: MonoBehaviour 
        where TChoicesView : StoryChoicesView
    {
        #region Inspector

        [SerializeField, Required] protected TChoicesView choicesView;

        #endregion

        #region Methods

        #region Unity Callbacks

        protected virtual void Reset()
        {
            if (!choicesView)
                choicesView = GetComponent<TChoicesView>();
        }

        protected virtual void Awake()
        {
            AsyncMessageBroker.Default
                .Subscribe<StoryPathSelectChoice>(choiceSelection =>
                    OnChoiceSelectionAsync(choiceSelection).ToObservable()).AddTo(this);
        }

        #endregion

        #region Members

        #endregion

        #region Story Callbacks
        protected abstract UniTask<Unit> OnChoiceSelectionAsync(StoryPathSelectChoice selectChoice);
        #endregion

        #endregion
    }
}