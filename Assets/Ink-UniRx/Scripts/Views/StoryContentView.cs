using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using InkUniRx.Animators;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InkUniRx.Views
{
    public abstract class StoryContentView : MonoBehaviour
    {
        #region Properties
        public abstract string ContentText { get; }
        public abstract bool IsEmpty { get; }

        #endregion
        #region Methods

        #region Unity Callbacks
        
        #endregion

        #region Public
        public abstract void ClearContent();
        public abstract void AddContent(string contentText);
        #endregion
      
        #endregion
    }
}