using EnhancedUI.EnhancedScroller;

namespace InkUniRx.ViewModels
{
    public class StoryElementCell: ScrollViewCell
    {
        public StoryElementCell(StoryElement storyElement, EnhancedScrollerCellView viewPrefab)
            : base(viewPrefab) => StoryElement = storyElement;
        public StoryElement StoryElement { get; }
    }
}