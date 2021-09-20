using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InkUniRx.Animators
{
    public abstract class TweenerTextAnimator: TextAnimator
    {
        #region Inspector

        [SerializeField, InlineButton(nameof(SyncEase))] 
        protected Ease textAnimationEase = Ease.Linear;

        #endregion

        #region Unity Callbacks

        protected override void Reset()
        {
            base.Reset();
            textAnimationEase = Ease.Linear;
        }

        #endregion

        #region Editor Only

#if UNITY_EDITOR
        private void SyncEase()
        {
            foreach (var animator in GetComponents<TweenerTextAnimator>())
            {
                animator.textAnimationEase = textAnimationEase;
            }
        }
#endif
        

        #endregion
    }
}