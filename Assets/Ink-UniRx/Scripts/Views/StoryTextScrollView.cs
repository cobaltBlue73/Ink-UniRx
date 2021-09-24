using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
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
        [SerializeField, ShowIf("startTextDirection", StartingTextDirection.BottomUp)] 
        protected LayoutElement spacerElement;
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

        protected virtual void Reset()
        {
            if (!scrollRect)
                scrollRect = GetComponent<ScrollRect>();

            if (scrollRect && !spacerElement)
            {
                spacerElement = scrollRect.content.Find("Spacer")
                    .GetComponent<LayoutElement>();
            }
            
            animateScrolling = true;
            scrollEase = Ease.InOutSine;
            scrollDuration = .3f;
        }

        protected virtual void Awake()
        {
            scrollRect.content.OnRectTransformDimensionsChangeAsObservable()
                .Subscribe(_ => OnContentSizeChanged())
                .AddTo(this);
            
            spacerElement.gameObject.SetActive(false);

            if (startTextDirection == StartingTextDirection.BottomUp)
            {
                scrollRect.viewport.OnRectTransformDimensionsChangeAsObservable()
                    .First().Subscribe(_ => InitSpacer())
                    .AddTo(this);
            }
        }

        private void Start() => ClearText();

        #endregion

        #region Public

        public override void ClearText() => InitSpacer();

        public virtual async UniTask ShowNewTextAsync(CancellationToken cancelAnimationToken)
        {
            await PlayTextAnimationsAsync(cancelAnimationToken);
            await WaitForEndScrollAsync(cancelAnimationToken);
        }

        #endregion

        #region Protected

        protected abstract UniTask PlayTextAnimationsAsync(CancellationToken cancelAnimationToken);

        private async UniTask WaitForEndScrollAsync(CancellationToken cancelAnimationToken)
        {
            if (_scrollTween != null)
                await _scrollTween.WithCancellation(cancelAnimationToken)
                    .SuppressCancellationThrow();
            
            if (!cancelAnimationToken.IsCancellationRequested) return;
            
            ResizeSpacer();
            scrollRect.verticalNormalizedPosition = 0;
        }

        protected virtual void OnContentSizeChanged() => ScrollToVerticalPos(0);

        private void ScrollToVerticalPos(float pos)
        {
            if (!animateScrolling ||  
                Math.Abs(scrollRect.verticalNormalizedPosition - pos) <= Mathf.Epsilon)
            {
                scrollRect.verticalNormalizedPosition = pos;
                ResizeSpacer();
                return;
            }
            
            _scrollTween?.Kill();
            _scrollTween = scrollRect.DOVerticalNormalizedPos(pos, scrollDuration)
                .SetEase(scrollEase).SetRecyclable().OnComplete(ResizeSpacer)
                .OnKill(()=> _scrollTween = null);
            
        }
        
        #endregion

        #region Private

        private void InitSpacer()
        {
            if (startTextDirection == StartingTextDirection.TopDown) return;
            
            spacerElement.minHeight = scrollRect.viewport.rect.height;
            spacerElement.transform.SetAsFirstSibling();
            spacerElement.gameObject.SetActive(true);
        }

        private void ResizeSpacer()
        {
            if (startTextDirection == StartingTextDirection.TopDown ||
                !spacerElement.isActiveAndEnabled) return;

            spacerElement.minHeight -= scrollRect.content.rect.height - 
                                       scrollRect.viewport.rect.height;
            
            if (spacerElement.minHeight <= 0)
                spacerElement.gameObject.SetActive(false);
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
        }
        
        #endregion

        #endregion

    }
}