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

        protected override void Reset()
        {
            base.Reset();
            
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
                .Select(_ => scrollRect.content).Subscribe(OnContentSizeChanged)
                .AddTo(this);
            
            spacerElement.gameObject.SetActive(false);

            if (startTextDirection == StartingTextDirection.BottomUp)
            {
                scrollRect.viewport.OnRectTransformDimensionsChangeAsObservable()
                    .First().Select(_=> scrollRect.viewport).Subscribe(InitSpacer)
                    .AddTo(this);
            }
        }
        
        #endregion

        #region Public

        public virtual async UniTask ShowNewTextAsync(CancellationToken cancelAnimationToken)
        {
            await PlayTextAnimation(cancelAnimationToken);
            await WaitForEndScrollAsync(cancelAnimationToken);
            
            if(startTextDirection == StartingTextDirection.BottomUp)
                ResizeSpacer();
        }

        #endregion

        #region Protected

        protected abstract UniTask PlayTextAnimation(CancellationToken cancelAnimationToken);

        private async UniTask WaitForEndScrollAsync(CancellationToken cancelAnimationToken)
        {
            if (_scrollTween != null)
                await _scrollTween.WithCancellation(cancelAnimationToken)
                    .SuppressCancellationThrow();
            
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
        
        protected virtual void InitSpacer(RectTransform viewport)
        {
            spacerElement.minHeight = viewport.rect.height;
            spacerElement.transform.SetAsFirstSibling();
            spacerElement.gameObject.SetActive(true);
        }

        protected virtual void ResizeSpacer()
        {
            if (!spacerElement || spacerElement.minHeight <= 0) return;

            spacerElement.minHeight -= scrollRect.content.rect.height - 
                                       scrollRect.viewport.rect.height;
            
            if (spacerElement.minHeight <= 0)
                spacerElement.gameObject.SetActive(false);
        }

        #endregion

        #region Private

        
        #endregion

        #endregion

    }
}