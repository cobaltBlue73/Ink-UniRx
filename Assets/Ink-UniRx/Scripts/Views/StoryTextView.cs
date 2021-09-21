using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using InkUniRx.Animators;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InkUniRx.Views
{
    public abstract class StoryTextView : MonoBehaviour
    {
        #region Inspector
        
        [SerializeField] protected TextAnimator[] textAnimators;
        
        #endregion
        #region Properties
        public abstract string Text { get; }
        public abstract bool IsEmpty { get; }

        #endregion
        #region Methods

        #region Unity Callbacks

        protected virtual void Reset()
        {
            if (textAnimators == null || textAnimators.Length <= 0)
                textAnimators = GetComponents<TextAnimator>();
        }

        #endregion

        #region Public
        public abstract void ClearText();
        public abstract void AddText(string text);
        #endregion
      
        #endregion
    }
}