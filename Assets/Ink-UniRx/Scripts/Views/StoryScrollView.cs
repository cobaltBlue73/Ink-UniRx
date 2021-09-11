using EnhancedUI.EnhancedScroller;
using InkUniRx.ViewModels;
using UniRx;
using UnityEngine;

namespace InlUniRx.Views
{
    public class StoryScrollView : MonoBehaviour, IEnhancedScrollerDelegate
    {
        #region Inspector

        [SerializeField] private EnhancedScroller enhancedScroller;
        [SerializeField] private StoryScrollViewCellView storyContentCellViewPrefab;
        [SerializeField] private StoryScrollViewCellView storyChoiceCellViewPrefab;
        [SerializeField, HideInInspector] private RectTransform scrollViewRect;
        
        #endregion

        #region Properties

        // public IReactiveCollection<ScrollViewElement> Elements => _elements;
        
        #endregion

        #region Member Variables

        private readonly ReactiveCollection<StoryScrollViewCell> _cells = 
            new ReactiveCollection<StoryScrollViewCell>();

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

        public StoryScrollViewCell AddStoryElement(StoryContent storyContent, bool resize = true)
        {
            var cell = new StoryScrollViewCell(storyContent, storyContentCellViewPrefab);
            AddCell(cell, resize);
            
            return cell;
        }
        
        public StoryScrollViewCell AddStoryElement(StoryChoice storyChoice, bool resize = true)
        {
            var cell = new StoryScrollViewCell(storyChoice, storyChoiceCellViewPrefab);
            AddCell(cell, resize);
            
            return cell;
        }
        
        public void RemoveCell(StoryScrollViewCell cell, bool resize = true)
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

            var storyCellView = baseCellView as StoryScrollViewCellView;
            
            if (!storyCellView) return baseCellView;
           
            storyCellView.SetCell(cell, _resize);

            return storyCellView;
        }

        #endregion

        #region Private Methods
        
        private void AddCell(StoryScrollViewCell cell, bool resize = true)
        {
            _resize = resize;
            
            if (_resize)
                ResetScrollView();
            
            _cells.Add(cell);
            
            if(!_resize) return;
            
            Resize();
            JumpToLast();
        }

        private void InitScroller()
        {
            if (!enhancedScroller)
                enhancedScroller = GetComponentInChildren<EnhancedScroller>();
            
            if(!enhancedScroller) return;

            if (Application.isPlaying)
                enhancedScroller.Delegate = this;
            
            if(!scrollViewRect)
                scrollViewRect = enhancedScroller.transform as RectTransform;
        }

        private void InitCollection()
        {
            _cells.AddTo(this);
        }

        private void ResetScrollView()
        {
            // first, clear out the cells in the enhancedScroller so the new text transforms will be reset
            enhancedScroller.ClearAll();

            // reset the enhancedScroller's position so that it is not outside of the new bounds
            enhancedScroller.ScrollPosition = 0;

            // second, reset the data's cell view sizes
            foreach (var cell in _cells)
            {
                cell.CellViewSize = 0;
            }
        }

        private void Resize()
        {
            // capture the enhancedScroller dimensions so that we can reset them when we are done
            var size = scrollViewRect.sizeDelta;

            // set the dimensions to the largest size possible to acommodate all the cells
            scrollViewRect.sizeDelta = new Vector2(size.x, float.MaxValue);

            // First Pass: reload the enhancedScroller so that it can populate the text UI elements in the cell view.
            // The content size fitter will determine how big the cells need to be on subsequent passes.
            enhancedScroller.ReloadData();

            // reset the enhancedScroller size back to what it was originally
            scrollViewRect.sizeDelta = size;

            // Second Pass: reload the data once more with the newly set cell view sizes and enhancedScroller content size
            _resize = false;
            enhancedScroller.ReloadData();
        }

        private void JumpToLast() => 
            enhancedScroller.JumpToDataIndex(_cells.Count - 1, 1f, 1f);

        #endregion
    }
}