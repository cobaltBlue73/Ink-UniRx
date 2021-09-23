using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
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

        private void Start() => ClearText();

        #endregion

        #region Public

        public override void ClearText()
        {
            base.ClearText();
            textMesh.text = string.Empty;
            textMesh.maxVisibleCharacters = 0;
        }

        public override void AddText(string text)
        {
            textMesh.text += IsEmpty ? text : $"\n{text}";
            textMesh.ForceMeshUpdate();
        }

        protected override UniTask PlayTextAnimationAsync(CancellationToken cancelAnimationToken)
        {
            var from = textMesh.maxVisibleCharacters;
            var to = textMesh.textInfo.characterCount - 1;
            textMesh.maxVisibleCharacters = textMesh.textInfo.characterCount;
            
            return UniTask.WhenAll(textAnimators.Select(animator =>
                animator.PlayTextAnimationAsync(textMesh, from, to, cancelAnimationToken)));
        }

        #endregion

        #region Private
        

        #endregion

        #endregion
        
    }
}