using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Utility.UniRx;

namespace InkUniRx
{
    [CreateAssetMenu(fileName = nameof(StoryPlayerSettings), menuName = "Ink-UniRx/StoryTextPresenter Settings", order = 0)]
    public class StoryTextPresenterSettings : ScriptableObject
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

        [SerializeField] private AnimationStyleType animationStyle;
        [SerializeField] private Ease easing = Ease.InOutExpo;
        [SerializeField, Min(0)] private float textAnimationSpeed;

        #endregion

        #region Properties

        public IReactiveProperty<float> TextAnimationSpeed => _textAnimationSpeed ??=
            Helpers.GetFloatPlayerPrefAsReactiveProperty(nameof(textAnimationSpeed), textAnimationSpeed, Disposables);

        private CompositeDisposable Disposables => _disposables ??= new CompositeDisposable();

        public AnimationStyleType AnimationStyle => animationStyle;

        public Ease Easing => easing;

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