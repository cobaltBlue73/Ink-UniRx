using System;
using UniRx;
using UnityEngine;
using Utility.UniRx;

namespace InkUniRx
{
    [CreateAssetMenu(fileName = nameof(StoryPlayerSettings), menuName = "Ink-UniRx/StoryPlayer Settings", order = 0)]
    public class StoryPlayerSettings : ScriptableObject
    {
        #region Static
        public static StoryPlayerSettings Instance => _instance ??= 
            Resources.Load<StoryPlayerSettings>($"Settings/{nameof(StoryPlayerSettings)}");
        private static StoryPlayerSettings _instance;

        #endregion
        
        #region Inspector

        [SerializeField] private bool continueMaximally;
        [SerializeField] private bool autoContinue;
        [SerializeField, Min(0)] private float autoContinueDelay;
        [SerializeField, Min(0)] private float textAnimationSpeed;
        
        #endregion

        #region Properties
        
        public bool ContinueMaximally => continueMaximally;

        public IReactiveProperty<bool> AutoContinue => _autoContinue ??=
            Helpers.GetBooleanPlayerPrefAsReactiveProperty(nameof(autoContinue), autoContinue, Disposables);

        public IReactiveProperty<float> AutoContinueDelay => _autoContinueDelay ??=
                Helpers.GetFloatPlayerPrefAsReactiveProperty(nameof(autoContinueDelay), autoContinueDelay, Disposables);

        public IReactiveProperty<float> TextAnimationSpeed => _textAnimationSpeed ??=
            Helpers.GetFloatPlayerPrefAsReactiveProperty(nameof(textAnimationSpeed), textAnimationSpeed, Disposables);

        private CompositeDisposable Disposables => _disposables ??= new CompositeDisposable();
        #endregion

        #region Member Variables

        private ReactiveProperty<bool> _autoContinue;
        private ReactiveProperty<float> _autoContinueDelay;
        private ReactiveProperty<float> _textAnimationSpeed;
        private CompositeDisposable _disposables;

        #endregion

        #region Unity Callbacks

        private void OnDestroy()
        {
            _disposables?.Clear();
        }

        #endregion
    }
}