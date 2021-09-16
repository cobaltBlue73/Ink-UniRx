using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.UI;

namespace InkUniRx.ViewModels
{
    public abstract class ScrollViewWrapperCell<TViewModel>: ScrollViewCell
    {
        protected ScrollViewWrapperCell(int index, EnhancedScrollerCellView viewPrefab, TViewModel subViewModel) 
            : base(index, viewPrefab) => SubViewModel = subViewModel;

        public TViewModel SubViewModel { get; }
    }
}