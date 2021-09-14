using EnhancedUI.EnhancedScroller;

namespace InkUniRx.ViewModels
{
    public class StoryElementCell: ScrollViewCell
    {
        public StoryElementCell(int index, StoryElement storyElement, EnhancedScrollerCellView viewPrefab)
            : base(index, viewPrefab) => StoryElement = storyElement;
        public StoryElement StoryElement { get; }
    }
}