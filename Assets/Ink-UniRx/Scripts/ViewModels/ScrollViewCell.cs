using EnhancedUI.EnhancedScroller;

namespace InkUniRx.ViewModels
{
    public abstract class ScrollViewCell
    {
        public float CellSize = 0;
        
        protected ScrollViewCell(EnhancedScrollerCellView viewPrefab) => ViewPrefab = viewPrefab;

        public EnhancedScrollerCellView ViewPrefab { get; private set; }
    }
}