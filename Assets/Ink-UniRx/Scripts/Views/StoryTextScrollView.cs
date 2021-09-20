using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace InkUniRx.Views
{
    [RequireComponent(typeof(ScrollRect))]
    public abstract class StoryTextScrollView: StoryTextView
    {
        #region Internals

        public enum StartingTextDirection
        {
            TopDown,
            BottomUp
        }

        #endregion
        #region Inspector
        
        [SerializeField, Required] protected ScrollRect scrollRect;
        [SerializeField] protected StartingTextDirection startTextDirection;
        [SerializeField, ToggleGroup(nameof(animateScrolling))] protected bool animateScrolling;
        [SerializeField, ToggleGroup(nameof(animateScrolling))] protected float scrollSpeed;
        [SerializeField, ToggleGroup(nameof(animateScrolling))] protected Ease scrollEase = Ease.InOutSine;

        #endregion

        #region Properties

        public StartingTextDirection StartTextDirection => startTextDirection;

        #endregion
        
        #region Methods
        
        #region Unity CallBacks

        protected override void Reset()
        {
            base.Reset();
            
            if (!scrollRect)
                scrollRect = GetComponent<ScrollRect>();

            scrollEase = Ease.InOutSine;
        }

        #endregion

        #region Protected

        protected async UniTask ScrollToAsync(float scrollPosition, CancellationToken cancelAnimationToken)
        {
            scrollPosition = Mathf.Clamp01(scrollPosition);
            
            if (!animateScrolling)
            {
                scrollRect.verticalNormalizedPosition = scrollPosition;
                return;
            }

            await scrollRect.DOVerticalNormalizedPos(scrollPosition, scrollSpeed)
                .SetEase(scrollEase).SetSpeedBased().SetRecyclable()
                .WithCancellation(cancelAnimationToken).SuppressCancellationThrow();
            
            if (!cancelAnimationToken.IsCancellationRequested) return;
            
            scrollRect.verticalNormalizedPosition = scrollPosition;
        }

        protected async UniTask ScrollToBottomAsync(CancellationToken cancelAnimationToken) => 
            await ScrollToAsync(1f, cancelAnimationToken);

        #endregion
        

        #endregion
       
    }
}