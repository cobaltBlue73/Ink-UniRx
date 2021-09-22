using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Utility.General;

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
        [SerializeField, ReadOnly] private ObservableRectTransformTrigger scrollRectContentRectTrigger;
        [SerializeField] protected StartingTextDirection startTextDirection;
        [SerializeField, ToggleGroup(nameof(animateScrolling))] protected bool animateScrolling;
        [SerializeField, ToggleGroup(nameof(animateScrolling))] protected float scrollDuration;
        [SerializeField, ToggleGroup(nameof(animateScrolling))] protected Ease scrollEase = Ease.InOutSine;

        #endregion

        #region Properties

        public StartingTextDirection StartTextDirection => startTextDirection;

        #endregion

        #region Variables

        private Tween _scrollTween;

        #endregion
        
        #region Methods
        
        #region Unity CallBacks

        protected override void Reset()
        {
            base.Reset();
            
            if (!scrollRect)
                scrollRect = GetComponent<ScrollRect>();

            if (scrollRect && !scrollRectContentRectTrigger)
                scrollRectContentRectTrigger = scrollRect.content
                    .GetOrAddComponent<ObservableRectTransformTrigger>();

            animateScrolling = true;
            scrollEase = Ease.InOutSine;
            scrollDuration = .3f;
        }

        protected virtual void Awake()
        {
            scrollRectContentRectTrigger.OnRectTransformDimensionsChangeAsObservable()
                .Select(_ => scrollRect.content).Subscribe(OnContentSizeChanged).AddTo(this);
        }
        
        #endregion

        #region Public

        public virtual async UniTask ShowNewTextAsync(CancellationToken cancelAnimationToken)
        {
            await PlayTextAnimation(cancelAnimationToken);
            await WaitForEndScrollAsync(cancelAnimationToken);
        }

        #endregion

        #region Protected

        protected abstract UniTask PlayTextAnimation(CancellationToken cancelAnimationToken);

        protected virtual async UniTask WaitForEndScrollAsync(CancellationToken cancelAnimationToken)
        {
            if (_scrollTween != null)
                await _scrollTween.WithCancellation(cancelAnimationToken);
            
            if (!cancelAnimationToken.IsCancellationRequested) return;
            
            scrollRect.verticalNormalizedPosition = 0;
        }

        protected virtual void OnContentSizeChanged(RectTransform content)
        {
            ScrollToBottom();
        }

        protected virtual void ScrollToBottom()
        {
            if (!animateScrolling ||  scrollRect.verticalNormalizedPosition <= 0)
            {
                scrollRect.verticalNormalizedPosition = 0;
                return;
            }
            
            _scrollTween?.Kill();  
            _scrollTween = scrollRect.DOVerticalNormalizedPos(0, scrollDuration)
                .SetEase(scrollEase).SetRecyclable().OnKill(() => _scrollTween = null);
        }
        
        #endregion
        

        #endregion
       
    }
}