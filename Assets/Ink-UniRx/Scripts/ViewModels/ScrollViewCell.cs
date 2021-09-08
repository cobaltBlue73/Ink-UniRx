using EnhancedUI.EnhancedScroller;

namespace InkUniRx.ViewModels
{
    public class ScrollViewCell
    {
        public float CellViewSize = 0;
        
        public ScrollViewCell(StoryPathElement storyPathElement, EnhancedScrollerCellView cellViewPrefab)
        {
            StoryPathElement = storyPathElement;
            CellViewPrefab = cellViewPrefab;
        }

        public StoryPathElement StoryPathElement { get; private set; }
        public EnhancedScrollerCellView CellViewPrefab { get; private set; }
    }
}