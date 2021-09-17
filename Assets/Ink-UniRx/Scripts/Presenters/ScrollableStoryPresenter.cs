using System;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace InkUniRx.Presenters
{
    [RequireComponent(typeof(ScrollRect))]
    public abstract class ScrollableStoryPresenter : MonoBehaviour
    {
        #region Inspector

        [SerializeField, Required] protected ScrollRect scrollRect;

        #endregion

        #region Unity Callbacks

        protected virtual void Reset()
        {
            if (!scrollRect)
                scrollRect = GetComponent<ScrollRect>();
        }

        protected virtual void Awake()
        {
            scrollRect.onValueChanged.AsObservable()
                .Subscribe(OnScrollRectValueChanged)
                .AddTo(this);
        }

        #endregion

        protected abstract void OnScrollRectValueChanged(Vector2 value);
    }
}