using EnhancedUI.EnhancedScroller;

namespace InkUniRx.ViewModels
{
    public class StoryChoicesCell: ScrollViewWrapperCell<StoryChoices>
    {
        public StoryChoicesCell(int index, EnhancedScrollerCellView viewPrefab, StoryChoices subViewModel) : base(index, viewPrefab, subViewModel)
        {
        }
    }
}