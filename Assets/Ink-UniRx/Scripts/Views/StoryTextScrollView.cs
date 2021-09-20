using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace InkUniRx.Views
{
    [RequireComponent(typeof(ScrollRect))]
    public abstract class StoryTextScrollView: StoryTextView
    {
        #region Internals

        public enum StartingTextDirection
        {
            TopDown,
            BottomUp
        }

        #endregion
        #region Inspector
        
        [SerializeField, Required] protected ScrollRect scrollRect;
        [SerializeField] protected StartingTextDirection startTextDirection;

        #endregion

        #region Properties

        public StartingTextDirection StartTextDirection => startTextDirection;

        #endregion

        #region Unity CallBacks

        protected virtual void Reset()
        {
            if (!scrollRect)
                scrollRect = GetComponent<ScrollRect>();
        }

        #endregion
       
    }
}