using EnhancedUI.EnhancedScroller;
using InkUniRx.ViewModels;

namespace InlUniRx.ViewControllers
{
    public abstract class StoryElementCellViewController : EnhancedScrollerCellView
    {
        public abstract void SetCell(ScrollViewCell cell);

        public abstract float GetCellViewSize();

    }
}