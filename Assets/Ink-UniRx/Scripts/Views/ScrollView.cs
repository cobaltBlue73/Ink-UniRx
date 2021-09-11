using System;
using Cysharp.Threading.Tasks;
using EnhancedUI.EnhancedScroller;
using InkUniRx.ViewModels;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Utility.EnhancedScrollerV2;

namespace InkUniRx.Views
{
    public class ScrollView : MonoBehaviour, IEnhancedScrollerDelegate
    {
        #region Inspector

        [SerializeField] private EnhancedScroller.TweenType jumpEasing;
        [SerializeField] private float jumpDuration;
        [SerializeField] private EnhancedScroller enhancedScroller;
        [SerializeField] private StoryElementCellView storyContentPrefab;
        [SerializeField] private StoryElementCellView storyChoicePrefab;
        [SerializeField] private bool addSpacer;
        [SerializeField, ShowIf("addSpacer")] private SpacerCellView spacerPrefab;
        [SerializeField, HideInInspector] private RectTransform scrollerRect;
        
        #endregion

        #region Properties

        // public IReactiveCollection<ScrollViewElement> Elements => _elements;
        
        #endregion

        #region Member Variables

        private readonly ReactiveCollection<ScrollViewCell> _cells = 
            new ReactiveCollection<ScrollViewCell>();

        private SpacerCell _spacer;

        private bool _resize;
        private float _totalCellSize;
        private float _oldScrollPosition;
        
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
        
        private void Start()
        {
            InitSpacer();
        }

        #endregion

        #region Public Methods

        public async UniTask AddStoryElementAsync(StoryContent storyContent, bool resize = true)
        {
            var cell = new StoryElementCell(storyContent, storyContentPrefab);
            await AddCellAsync(cell, resize);
        }
        
        public async UniTask AddStoryElementAsync(StoryChoice storyChoice, bool resize = true)
        {
            var cell = new StoryElementCell(storyChoice, storyChoicePrefab);
            await AddCellAsync(cell, resize);
        }
        
        public async UniTask RemoveCellAsync(ScrollViewCell scrollViewCell, bool resize = true)
        {
            if (resize)
                ResetScrollView();
            
            _cells.Remove(scrollViewCell);
            
            if(!resize) return;
            
            ResizeScroller();
            await JumpToLastAsync();
        }
        
        public async UniTask RemoveLastCellAsync(bool resize = true)
        {
            if (resize)
                ResetScrollView();
            
            _cells.RemoveAt(_cells.Count - 1);
            
            if(!resize) return;
            
            ResizeScroller();
            await JumpToLastAsync();
        }

        #endregion

        #region Element Collection Callbacks
        
        #endregion

        #region EnhancedScroller Callbacks

        public int GetNumberOfCells(EnhancedScroller scroller) => _cells.Count;

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex) => _cells[dataIndex].CellSize;

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var cell = _cells[dataIndex];
            var baseCellView = scroller.GetCellView(cell.ViewPrefab);

            var storyCellView = baseCellView as ScrollViewCellView;
            
            if (!storyCellView) return baseCellView;
           
            storyCellView.SetCell(cell);

            return storyCellView;
        }

        #endregion

        #region Private Methods
        
        private async UniTask AddCellAsync(ScrollViewCell scrollViewCell, bool resize = true)
        {
            if (resize)
                ResetScrollView();
            
            _cells.Add(scrollViewCell);
            
            if(!resize) return;
            
            ResizeScroller();
            await JumpToLastAsync();
        }

        private void InitScroller()
        {
            if (!enhancedScroller)
                enhancedScroller = GetComponentInChildren<EnhancedScroller>();
            
            if(!enhancedScroller) return;

            if (Application.isPlaying)
                enhancedScroller.Delegate = this;
            
            if(!scrollerRect)
                scrollerRect = enhancedScroller.transform as RectTransform;
        }

        private void InitCollection()
        {
            _cells.AddTo(this);
        }
        
        private void InitSpacer()
        {
            if(!addSpacer || !spacerPrefab) return;

            _spacer = new SpacerCell(spacerPrefab);
            _cells.Add(_spacer);
            ResizeScroller();
        }

        private void ResetScrollView()
        {
            // first, clear out the cells in the enhancedScroller so the new text transforms will be reset
            enhancedScroller.ClearAll();

            // reset the enhancedScroller's position so that it is not outside of the new bounds
            _oldScrollPosition = enhancedScroller.ScrollPosition;
            enhancedScroller.ScrollPosition = 0;

            // second, reset the data's cell view sizes
            foreach (var cell in _cells)
            {
                cell.CellSize = 0;
            }
        }

        private void ResizeScroller()
        {
            // capture the scroll rect size.
            // this will be used at the end of this method to determine the final scroll position
            var scrollRectSize = enhancedScroller.ScrollRectSize;

            // capture the scroller's position so we can smoothly scroll from it to the new cell
            var offset = _oldScrollPosition - enhancedScroller.ScrollSize;

            // capture the scroller dimensions so that we can reset them when we are done;
            var size = scrollerRect.sizeDelta;

            // set the dimensions to the largest size possible to acommodate all the cells
            scrollerRect.sizeDelta = new Vector2(size.x, float.MaxValue);

            // First Pass: reload the scroller so that it can populate the text UI elements in the cell view.
            // The content size fitter will determine how big the cells need to be on subsequent passes.
            _resize = true;
            enhancedScroller.ReloadData();

            // calculate the total size required by all cells. This will be used when we determine
            // where to end up at after we reload the data on the second pass.
            _totalCellSize = enhancedScroller.padding.top + enhancedScroller.padding.bottom;
            for (var i = addSpacer? 1: 0; i < _cells.Count; i++)
            {
                _totalCellSize += _cells[i].CellSize + (i < _cells.Count - 1 ? enhancedScroller.spacing : 0);
            }

            // set the spacer to the entire scroller size.
            // this is necessary because we need some space to actually do a jump
            if(_spacer != null)
                _spacer.CellSize = scrollRectSize;

            // reset the scroller size back to what it was originally
            scrollerRect.sizeDelta = size;

            // Second Pass: reload the data once more with the newly set cell view sizes and scroller content size.
            _resize = false;
            enhancedScroller.ReloadData();

            // set the scroll position to the previous cell (plus the offset of where the scroller currently is) so that we can jump to the new cell.
            enhancedScroller.ScrollPosition = _totalCellSize - _cells[_cells.Count - 1].CellSize + offset;
        }
        
        private void ResetSpacer()
        {
            if(!addSpacer || _spacer == null) return;
            
            // reset the spacer's cell size to the scroller's size minus the rest of the cell sizes
            // (or zero if the spacer is no longer needed)
            _spacer.CellSize = Mathf.Max(enhancedScroller.ScrollRectSize - _totalCellSize, 0);

            // reload the data to set the new cell size
            enhancedScroller.ReloadData(1.0f);
        }

        private async UniTask JumpToLastAsync()
        {
            await enhancedScroller.JumpToIndexAsync(_cells.Count - 1, 1f, 1f, tweenType: jumpEasing, tweenTime: jumpDuration);
            ResetSpacer();
        }

        #endregion
    }
}