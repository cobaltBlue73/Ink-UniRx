using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Utility.TextMeshPro;

namespace InkUniRx.Animators
{
    public abstract class TextAnimator : MonoBehaviour
    {
        #region Internals

        public enum TextAnimationType
        {
            Characters,
            Words,
            Lines
        }

        #endregion
        
        #region Inspector

        [SerializeField, InlineEditor] private TMP_Text text;
        public TextAnimationType animationType;
        public float speed;
        
        #endregion

        #region Properties

        public TMP_Text Text => text;

        #endregion

        #region Unity Callbacks

        protected virtual void Reset()
        {
            if (!text)
                text = GetComponent<TMP_Text>();
        }

        #endregion

        #region Methods

        #region Public

        public UniTask PlayTextAnimationAsync(TMP_Text textMesh, int fromCharIndex, int toCharIndex, CancellationToken cancelAnimationToken)
        {
            switch (animationType)
            {
                case TextAnimationType.Words:
                    textMesh.textInfo.GetFirstAndLastWordIndexFromCharacterIndexRange(fromCharIndex, toCharIndex, out var firstWordIndex, out var lastWordIndex);
                    return PlayWordAnimationAsync(textMesh, firstWordIndex, lastWordIndex, cancelAnimationToken);
                case TextAnimationType.Lines:
                    textMesh.textInfo.GetFirstAndLastLineIndexFromCharacterIndexRange(fromCharIndex, toCharIndex, out var firstLineIndex, out var lastLineIndex);
                    return PlayLineAnimationAsync(textMesh, firstLineIndex, lastLineIndex, cancelAnimationToken);
                case TextAnimationType.Characters:
                default:
                    return PlayCharAnimationAsync(textMesh, fromCharIndex, toCharIndex, cancelAnimationToken);
            }
        }

        public UniTask PlayTextAnimationAsync(int fromCharIndex, int toCharIndex, CancellationToken cancelAnimationToken) => 
            PlayTextAnimationAsync(text, fromCharIndex, toCharIndex, cancelAnimationToken);

        #endregion

        #region Private

        protected abstract UniTask PlayCharAnimationAsync(TMP_Text textMesh, int fromCharIndex, int toCharIndex,
            CancellationToken cancelAnimationToken);
        
        protected abstract UniTask PlayWordAnimationAsync(TMP_Text textMesh, int fromWordIndex, int toWordIndex,
            CancellationToken cancelAnimationToken);
        
        protected abstract UniTask PlayLineAnimationAsync(TMP_Text textMesh, int fromLineIndex, int toLineIndex,
            CancellationToken cancelAnimationToken);

        #endregion
      

        #endregion
    }
}