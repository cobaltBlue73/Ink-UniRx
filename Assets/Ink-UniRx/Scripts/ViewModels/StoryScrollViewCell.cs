using EnhancedUI.EnhancedScroller;

namespace InkUniRx.ViewModels
{
    public class StoryScrollViewCell
    {
        public float CellViewSize = 0;
        
        public StoryScrollViewCell(StoryElement storyElement, EnhancedScrollerCellView cellViewPrefab)
        {
            StoryElement = storyElement;
            CellViewPrefab = cellViewPrefab;
        }

        public StoryElement StoryElement { get; private set; }
        public EnhancedScrollerCellView CellViewPrefab { get; private set; }
    }
}