using System;
using UniRx;
using UnityEngine;
using Utility.UniRx;

namespace InkUniRx
{
    [CreateAssetMenu(fileName = nameof(StoryPlayerSettings), menuName = "Ink-UniRx/StoryPlayer Settings", order = 0)]
    public class StoryPlayerSettings : ScriptableObject
    {
        #region Inspector

        [SerializeField] private bool continueEachLine;
        [SerializeField] private bool autoContinue;
        [SerializeField, Min(0)] private float autoContinueDelay;
        
        #endregion

        #region Properties

        public IReactiveProperty<bool> AutoContinue => _autoContinue ??=
            Helpers.GetBooleanPlayerPrefAsReactiveProperty(nameof(autoContinue), autoContinue, Disposables);

        public IReactiveProperty<float> AutoContinueDelay => _autoContinueDelay ??=
                Helpers.GetFloatPlayerPrefAsReactiveProperty(nameof(autoContinueDelay), autoContinueDelay, Disposables);
        
        private CompositeDisposable Disposables => _disposables ??= new CompositeDisposable();
        
        public bool ContinueEachLine => continueEachLine;

        #endregion

        #region Member Variables

        private ReactiveProperty<bool> _autoContinue;
        private ReactiveProperty<float> _autoContinueDelay;
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