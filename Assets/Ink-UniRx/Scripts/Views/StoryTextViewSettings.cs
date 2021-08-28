using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Utility.UniRx;

namespace InkUniRx
{
    [CreateAssetMenu(fileName = nameof(StoryTextViewSettings), menuName = "Ink-UniRx/Settings/StoryTextView", order = 0)]
    public class StoryTextViewSettings : ScriptableObject
    {
        #region Internal

        public enum  AnimationStyleType
        {
            None,
            ByLetter,
            ByWord
        }
      
        #endregion
        
        #region Inpsector

        [SerializeField] private bool trim = true;
        [SerializeField] private AnimationStyleType animationStyle;
        [SerializeField] private Ease easing = Ease.OutQuint;
        [SerializeField, Min(0)] private float textAnimationSpeed;

        #endregion

        #region Properties

        public IReactiveProperty<float> TextAnimationSpeed => _textAnimationSpeed ??=
            Helpers.GetFloatPlayerPrefAsReactiveProperty(nameof(textAnimationSpeed), textAnimationSpeed, Disposables);

        private CompositeDisposable Disposables => _disposables ??= new CompositeDisposable();

        public AnimationStyleType AnimationStyle => animationStyle;

        public Ease Easing => easing;

        public bool Trim => trim;

        #endregion

        #region Member Variables

        private ReactiveProperty<float> _textAnimationSpeed;
        private CompositeDisposable _disposables;

        #endregion

        #region Unity Callbacks

        private void OnDisable()
        {
            _disposables?.Clear();
        }

        #endregion
    }
}