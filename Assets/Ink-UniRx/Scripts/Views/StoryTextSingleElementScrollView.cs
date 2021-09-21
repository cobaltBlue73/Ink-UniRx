using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utility.General;

namespace InkUniRx.Views
{
    public class StoryTextSingleElementScrollView: StoryTextScrollView
    {
        #region Inspector

        [SerializeField, InlineEditor] private TMP_Text textMesh;
        [SerializeField, InlineEditor] private LayoutElement textLayoutElement;

        #endregion

        #region Properties

        public override string Text => textMesh.text;
        public override bool IsEmpty => Text.Length <= 0;

        #endregion

        #region Variables

        private int _prevCharCount;
        private IReadOnlyReactiveProperty<float> _textHeightProp;
        private Tweener _scrollTween;

        #endregion

        #region Methods

        #region Unity Callbacks

        protected override void Reset()
        {
            base.Reset();

            if (!textMesh)
                textMesh = GetComponentInChildren<TMP_Text>();

            if (textMesh && !textLayoutElement)
                textLayoutElement = textMesh.GetOrAddComponent<LayoutElement>();
        }
        
        private void Awake()
        {
            ClearText();
            textMesh.rectTransform
                .ObserveEveryValueChanged(rt => rt.rect.height)
                .Subscribe(OnContentHeightChanged)
                .AddTo(this);
        }
        
        #endregion

        #region Public

        public override void ClearText()
        {
            _prevCharCount = 0;
            textMesh.text = string.Empty;
        }

        public override void AddText(string text)
        {
            _prevCharCount = textMesh.textInfo.characterCount;
            textMesh.text += IsEmpty? text: $"\n{text}";
            textMesh.ForceMeshUpdate();
            textMesh.maxVisibleCharacters = _prevCharCount;
        }
        
        public override async UniTask ShowNewTextAsync(CancellationToken cancelAnimationToken)
        {
            // textMesh.maxVisibleCharacters = textMesh.textInfo.characterCount;
            // textMesh.ForceMeshUpdate();
            // var newHeight = await _textHeightProp.First();
            // textLayoutElement.minHeight = newHeight;
            
            var animationTasks = UniTask.WhenAll(textAnimators.Select(animator =>
                animator.PlayTextAnimationAsync(textMesh, _prevCharCount, 
                    textMesh.textInfo.characterCount - 1,
                    cancelAnimationToken)));

            await animationTasks;

            if (_scrollTween != null)
                await _scrollTween;
        }


        #endregion

        #region Private

        private void OnContentHeightChanged(float height)
        {
            textLayoutElement.minHeight = height;

            if(scrollRect.verticalNormalizedPosition <= 0) return;
            _scrollTween?.Kill();
            _scrollTween = scrollRect.DOVerticalNormalizedPos(0, scrollSpeed)
                .SetEase(scrollEase).SetRecyclable()
                .OnKill(()=> _scrollTween = null);
        }

        #endregion

        #endregion
        
    }
}