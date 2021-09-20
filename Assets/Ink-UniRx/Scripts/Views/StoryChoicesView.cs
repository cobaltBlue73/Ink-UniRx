using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Ink.Runtime;
using UnityEngine;

namespace InkUniRx.Views
{
    public abstract class StoryChoicesView : MonoBehaviour
    {
        #region Properties
        public abstract bool Interactable { get; set; }
        public abstract IObservable<Choice> WhenChoiceSelected { get; }
        #endregion
        
        #region Methods
        public abstract void SetChoices(IEnumerable<Choice> choices);
        #endregion
        
       
    }
}