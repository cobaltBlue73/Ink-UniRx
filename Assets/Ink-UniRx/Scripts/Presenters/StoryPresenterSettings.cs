using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Utility.UniRx;

namespace InkUniRx
{
    [CreateAssetMenu(fileName = nameof(StoryPresenterSettings), menuName = "Ink-UniRx/Settings/StoryPresenter", order = 0), InlineEditor]
    public class StoryPresenterSettings : ScriptableObject
    {
        #region Inspector

        #region Story Continue Settings

        [SerializeField, FoldoutGroup("Story Continue Settings")] 
        private BoolReactiveProperty continueMaximally;
        
        [SerializeField, FoldoutGroup("Story Continue Settings"), 
         OnValueChanged(nameof(OnAutoContinueChanged))] 
        private bool autoContinue;
        
        [SerializeField, MinValue(0), FoldoutGroup("Story Continue Settings"), 
         OnValueChanged(nameof(OnAutoContinueDelayChanged))] 
        private float autoContinueDelay;

        #endregion

        #region Text Animation Settings

        [SerializeField, FoldoutGroup("Text Animation Settings"), 
         OnValueChanged(nameof(OnTextAnimationEasingChanged))] 
        private Ease textAnimationEasing = Ease.Linear;
        
        [SerializeField, FoldoutGroup("Text Animation Settings"), 
         OnValueChanged(nameof(OnTextAnimationSpeedChanged))] 
        private float textAnimationSpeed;

        #endregion
        
        #endregion

        #region Properties
        
        public IReactiveProperty<bool> AutoContinue => _autoContinue ??=
            Helpers.GetBooleanPlayerPrefAsReactiveProperty(nameof(autoContinue), autoContinue, Disposables);

        public IReactiveProperty<float> AutoContinueDelay => _autoContinueDelay ??=
                Helpers.GetFloatPlayerPrefAsReactiveProperty(nameof(autoContinueDelay), autoContinueDelay, Disposables);
        
        public IReactiveProperty<bool> ContinueMaximally => continueMaximally;
        
        public IReactiveProperty<float> TextAnimationSpeed => _textAnimationSpeed ??=
            Helpers.GetFloatPlayerPrefAsReactiveProperty(nameof(textAnimationSpeed), textAnimationSpeed, Disposables);
        
        public IReactiveProperty<Ease> TextAnimationEasing => 
            _textAnimationEasing ??= new ReactiveProperty<Ease>(textAnimationEasing);
        
        private CompositeDisposable Disposables => _disposables ??= new CompositeDisposable();
        
        #endregion

        #region Member Variables

        private ReactiveProperty<Ease> _textAnimationEasing;
        private ReactiveProperty<bool> _autoContinue;
        private ReactiveProperty<float> _autoContinueDelay;
        private ReactiveProperty<float> _textAnimationSpeed;
        private CompositeDisposable _disposables;

        #endregion

        #region OnValueChanged Callbacks

        private void OnTextAnimationEasingChanged() => TextAnimationEasing.Value = textAnimationEasing;

        private void OnAutoContinueChanged() => AutoContinue.Value = autoContinue;

        private void OnAutoContinueDelayChanged() => AutoContinueDelay.Value = autoContinueDelay;

        private void OnTextAnimationSpeedChanged() => TextAnimationSpeed.Value = textAnimationSpeed;

        #endregion

        #region Unity Callbacks

        private void OnValidate()
        {
          
        }

        private void OnDisable()
        {
            _disposables?.Clear();
        }

        #endregion
    }
}