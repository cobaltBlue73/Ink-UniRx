using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using InkUniRx.Animators.Animations;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Utility.TextMeshPro;

namespace InkUniRx.Animators
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextMeshAnimator : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private TMP_Text textMesh;
        [SerializeField] private TextAnimation[] textAnimations;

        #endregion

        #region Properties
        public TMP_Text TextMesh => textMesh;
        public DOTweenTMPAnimator TMPAnimator => _tmpAnimator ??= new DOTweenTMPAnimator(textMesh);

        #endregion

        #region Vairables

        private DOTweenTMPAnimator _tmpAnimator;

        #endregion

        #region Unity Callbacks

        protected virtual void Reset()
        {
            if (!textMesh)
                textMesh = GetComponent<TMP_Text>();
        }

        #endregion

        #region Methods

        #region Public
        
        public UniTask PlayAsync(int fromCharIndex, int toCharIndex, CancellationToken animationCancelToken) => 
            UniTask.WhenAll(textAnimations.Select(textAnimation => 
                textAnimation.PlayAsync(this, fromCharIndex, toCharIndex, animationCancelToken)));

        #endregion
        
        #endregion
    }
}