using System;
using EnhancedUI.EnhancedScroller;
using InkUniRx.ViewModels;
using UnityEngine;
using UnityEngine.UI;

namespace InkUniRx.Views
{
    public abstract class ScrollViewCellView : EnhancedScrollerCellView
    {
        public abstract void SetCell(ScrollViewCell scrollViewCell);
    }
}