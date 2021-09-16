using EnhancedUI.EnhancedScroller;

namespace InkUniRx.ViewModels
{
    public class StoryContentCell: ScrollViewWrapperCell<StoryContent>
    {
        public StoryContentCell(int index, EnhancedScrollerCellView viewPrefab, StoryContent subViewModel) : base(index, viewPrefab, subViewModel)
        { }
    }
}