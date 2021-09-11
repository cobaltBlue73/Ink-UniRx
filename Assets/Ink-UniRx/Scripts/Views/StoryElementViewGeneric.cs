using System;
using InkUniRx.ViewModels;
using TMPro;
using UnityEngine;

namespace InkUniRx.Views
{
    public abstract class StoryElementViewGeneric<T> : StoryElementViewBase where T : StoryElement
    {
        #region Properties

        public new T StoryElement => base.StoryElement as T;

        #endregion
    }
}