using EnhancedUI.EnhancedScroller;
using InkUniRx.ViewModels;
using UniRx;
using UnityEngine;

namespace InlUniRx.ViewControllers
{
    public class ScrollViewController : MonoBehaviour, IEnhancedScrollerDelegate
    {
        #region Inspector

        [SerializeField] private EnhancedScroller scrollView;
        [SerializeField, HideInInspector] private RectTransform scrollViewRect;
        
        #endregion

        #region Properties

        // public IReactiveCollection<ScrollViewElement> Elements => _elements;
        
        #endregion

        #region Member Variables

        private readonly ReactiveCollection<ScrollViewCell> _cells = 
            new ReactiveCollection<ScrollViewCell>();

        private bool _resize;
        
        #endregion

        #region Unity CallBacks

        private void Reset()
        {
            InitScroller();
        }

        private void Awake()
        {
            InitScroller();
            InitCollection();
        }

        #endregion

        #region Public Methods

        public void AddCell(ScrollViewCell cell, bool resize = true)
        {
            _resize = resize;
            
            if (_resize)
                ResetScrollView();
            
            _cells.Add(cell);
            
            if(!_resize) return;
            
            Resize();
            JumpToLast();
        }

        public void RemoveCell(ScrollViewCell cell, bool resize = true)
        {
            _resize = resize;
            
            if (_resize)
                ResetScrollView();
            
            _cells.Remove(cell);
            
            if(!_resize) return;
            
            Resize();
            JumpToLast();
        }
        
        public void RemoveLastCell(bool resize = true)
        {
            _resize = resize;
            
            if (_resize)
                ResetScrollView();
            
            _cells.RemoveAt(_cells.Count - 1);
            
            if(!_resize) return;
            
            Resize();
            JumpToLast();
        }

        #endregion

        #region Element Collection Callbacks
        
        #endregion

        #region EnhancedScroller Callbacks

        public int GetNumberOfCells(EnhancedScroller scroller) => _cells.Count;

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex) => _cells[dataIndex].CellViewSize;

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var cell = _cells[dataIndex];
            var baseCellView = scroller.GetCellView(cell.CellViewPrefab);

            var storyCellView = baseCellView as StoryElementCellViewController;
            
            if (!storyCellView) return baseCellView;
           
            storyCellView.SetCell(cell);

            if (_resize)
                cell.CellViewSize = storyCellView.GetCellViewSize();

            return storyCellView;
        }

        #endregion

        #region Private Methods

        private void InitScroller()
        {
            if (!scrollView)
                scrollView = GetComponentInChildren<EnhancedScroller>();
            
            if(!scrollView) return;

            if (Application.isPlaying)
                scrollView.Delegate = this;
            
            if(!scrollViewRect)
                scrollViewRect = scrollView.transform as RectTransform;
        }

        private void InitCollection()
        {
            _cells.AddTo(this);
        }

        private void ResetScrollView()
        {
            // first, clear out the cells in the scroller so the new text transforms will be reset
            scrollView.ClearAll();

            // reset the scroller's position so that it is not outside of the new bounds
            scrollView.ScrollPosition = 0;

            // second, reset the data's cell view sizes
            foreach (var cell in _cells)
            {
                cell.CellViewSize = 0;
            }
        }

        private void Resize()
        {
            // capture the scroller dimensions so that we can reset them when we are done
            var size = scrollViewRect.sizeDelta;

            // set the dimensions to the largest size possible to acommodate all the cells
            scrollViewRect.sizeDelta = new Vector2(size.x, float.MaxValue);

            // First Pass: reload the scroller so that it can populate the text UI elements in the cell view.
            // The content size fitter will determine how big the cells need to be on subsequent passes.
            scrollView.ReloadData();

            // reset the scroller size back to what it was originally
            scrollViewRect.sizeDelta = size;

            // Second Pass: reload the data once more with the newly set cell view sizes and scroller content size
            _resize = false;
            scrollView.ReloadData();
        }

        private void JumpToLast() => 
            scrollView.JumpToDataIndex(_cells.Count - 1, 1f, 1f);

        #endregion
    }
}