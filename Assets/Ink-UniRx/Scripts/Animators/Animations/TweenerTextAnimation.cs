using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InkUniRx.Animators.Animations
{
    public abstract class TweenerTextAnimation: TextAnimation
    {
        #region Inspector
        
        [SerializeField] protected Ease textEase = Ease.Linear;

        #endregion

        #region Unity Callbacks

        protected override void Reset()
        {
            base.Reset();
            textEase = Ease.Linear;
        }

        #endregion
    }
}