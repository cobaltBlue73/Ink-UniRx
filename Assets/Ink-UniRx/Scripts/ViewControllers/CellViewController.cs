using System;
using EnhancedUI.EnhancedScroller;
using InkUniRx.ViewModels;
using UnityEngine;

namespace InlUniRx.ViewControllers
{
    public abstract class CellViewController : EnhancedScrollerCellView
    {
        public abstract void SetCell(ScrollViewCell cell);

        public abstract float GetCellViewSize();

    }
}