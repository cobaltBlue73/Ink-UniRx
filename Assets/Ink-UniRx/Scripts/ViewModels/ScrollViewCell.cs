using EnhancedUI.EnhancedScroller;

namespace InkUniRx.ViewModels
{
    public abstract class ScrollViewCell
    {
        public float CellSize = 0;

        protected ScrollViewCell(int index, EnhancedScrollerCellView viewPrefab)
        { 
            Index = index;
            ViewPrefab = viewPrefab;
        }

        public int Index { get; }
        public EnhancedScrollerCellView ViewPrefab { get; private set; }
    }
}