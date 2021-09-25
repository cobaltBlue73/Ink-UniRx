using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace InkUniRx.Animators.Animations
{
    [InlineEditor]
    public abstract class TextAnimation : ScriptableObject
    {
        #region Inspector

        [SerializeField, MinValue(0)] protected float speed = 60f;

        #endregion

        #region Unity Callbcaks

        protected virtual void Reset() => speed = 60f;

        #endregion
        
        public abstract UniTask AnimateTextAsync(TextMeshAnimator animator, int fromCharIndex, int toCharIndex,
            CancellationToken cancelAnimationToken);
    }
}