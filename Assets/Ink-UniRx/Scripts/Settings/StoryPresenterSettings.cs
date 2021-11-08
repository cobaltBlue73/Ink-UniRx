using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Utility.UniRx;
using ReactivePropertyExtensions = Utility.UniRx.ReactivePropertyExtensions;

namespace InkUniRx.Settings
{
    [CreateAssetMenu(menuName = "Ink-UniRx/Settings/StoryPresenter"), InlineEditor]
    public class StoryPresenterSettings : ScriptableObject
    {
        #region Inspector

        #region Story Continue Settings

        [SerializeField, FoldoutGroup("Story Continue Settings")] 
        private BoolReactiveProperty continueMaximally;
        
        [SerializeField, FoldoutGroup("Story Continue Settings")] 
        private BoolReactiveProperty autoContinue;
        
        [SerializeField, FoldoutGroup("Story Continue Settings")] 
        private FloatReactiveProperty autoContinueDelay;

        #endregion

        #region Text Animation Settings

        [SerializeField, FoldoutGroup("Text Animation Settings"), 
         OnValueChanged(nameof(OnTextAnimationEasingChanged))] 
        private Ease textAnimationEasing = Ease.Linear;
        
        [SerializeField, FoldoutGroup("Text Animation Settings")] 
        private FloatReactiveProperty textAnimationSpeed;

        #endregion
        
        #endregion

        #region Properties

        public IReactiveProperty<bool> AutoContinue => _autoContinue ??=
            autoContinue.AsPlayerPrefReactiveProperty(nameof(autoContinue), Disposables);

        public IReactiveProperty<float> AutoContinueDelay => _autoContinueDelay ??=
                autoContinueDelay.AsPlayerPrefReactiveProperty(nameof(autoContinueDelay), Disposables);
        
        public IReactiveProperty<bool> ContinueMaximally => continueMaximally;
        
        public IReactiveProperty<float> TextAnimationSpeed => _textAnimationSpeed ??=
            textAnimationSpeed.AsPlayerPrefReactiveProperty(nameof(textAnimationSpeed), Disposables);
        
        public IReactiveProperty<Ease> TextAnimationEasing => 
            _textAnimationEasing ??= new ReactiveProperty<Ease>(textAnimationEasing);
        
        private CompositeDisposable Disposables => _disposables ??= new CompositeDisposable();
        
        #endregion

        #region Member Variables

        private IReactiveProperty<Ease> _textAnimationEasing;
        private IReactiveProperty<bool> _autoContinue;
        private IReactiveProperty<float> _autoContinueDelay;
        private IReactiveProperty<float> _textAnimationSpeed;
        private CompositeDisposable _disposables;

        #endregion

        #region OnValueChanged Callbacks
        
        private void OnTextAnimationEasingChanged() => TextAnimationEasing.Value = textAnimationEasing;

        #endregion

        #region Unity Callbacks

        private void OnValidate()
        {
            autoContinueDelay.Value = Mathf.Max(0, autoContinueDelay.Value);
            textAnimationSpeed.Value = Mathf.Max(0, textAnimationSpeed.Value);
        }
        
        private void OnDisable()
        {
            PlayerPrefs.Save();
            _disposables?.Clear();
        }

        #endregion
    }
}