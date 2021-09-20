using System;
using Ink.Runtime;
using UnityEngine;

namespace InkUniRx.Views
{
    public abstract class StoryChoiceView : MonoBehaviour
    {
        #region Propeterties
        public abstract Choice Choice { get; }
        public abstract string Text { get; set; }
        public abstract IObservable<Choice> WhenSelected { get; }

        #endregion
        
        #region Methods

        public abstract void SetChoice(Choice choice);

        #endregion
    }
}