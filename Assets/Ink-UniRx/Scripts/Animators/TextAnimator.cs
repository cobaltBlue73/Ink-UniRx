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
        #region Inspector

        [SerializeField, InlineEditor] private TMP_Text text;
        [SerializeField, InlineButton(nameof(SyncSpeed))] protected float speed;
        
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

        public abstract UniTask PlayTextAnimationAsync(TMP_Text textMesh, int fromCharIndex, int toCharIndex,
            CancellationToken cancelAnimationToken);
        
        public UniTask PlayTextAnimationAsync(int fromCharIndex, int toCharIndex, CancellationToken cancelAnimationToken) => 
            PlayTextAnimationAsync(text, fromCharIndex, toCharIndex, cancelAnimationToken);

        #endregion

        #region Editor Only
#if UNITY_EDITOR
        private void SyncSpeed()
        {
            foreach (var animator in GetComponents<TextAnimator>())
            {
                animator.speed = speed;
            }
        }
#endif
        #endregion
        
        #endregion
    }
}