using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EnhancedUI.EnhancedScroller;
using InkUniRx.ViewModels;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Utility.EnhancedScrollerV2;

namespace InkUniRx.Views
{
    public class ScrollView : MonoBehaviour, IEnhancedScrollerDelegate
    {
        #region Inspector

        [SerializeField] private EnhancedScroller eScroller;
        [SerializeField] private StoryContentCellView storyContentCellPrefab;
        [SerializeField] private StoryChoicesCellView storyChoiceCellPrefab;
        [SerializeField, HideInInspector] private RectTransform scrollerRect;
        
        #region Jump Settings

        [SerializeField, BoxGroup("Jump Settings")]
        private EnhancedScroller.TweenType jumpEasing;
        
        [SerializeField, BoxGroup("Jump Settings"), MinValue(0), 
         HideIf("jumpEasing", EnhancedScroller.TweenType.immediate)] 
        private float jumpDuration;
        
        // [SerializeField, BoxGroup("Jump Settings"), MinValue(0)] 
        // private float scrollerOffset;
        //
        // [SerializeField, BoxGroup("Jump Settings"), MinValue(0)] 
        // private float cellOffset;
        
        [SerializeField, BoxGroup("Jump Settings")] 
        private bool includeSpacing;

        #endregion

        #region Spacer Settings

        [SerializeField, BoxGroup("Spacer Settings")] private bool addSpacer;
        [SerializeField, BoxGroup("Spacer Settings"), ShowIf("addSpacer")] 
        private SpacerCellView spacerPrefab;

        #endregion
        
        #endregion

        #region Properties

        // public IReactiveCollection<ScrollViewElement> Elements => _elements;
        
        #endregion

        #region Member Variables

        private readonly List<ScrollViewCell> _cells = new List<ScrollViewCell>();

        private SpacerCell _spacer;
        
        private float _totalCellSize;
        private float _oldScrollPosition;
        private RectTransform _scrollerLayoutRect;
        
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

        public StoryContentCell AddStoryElement(StoryContent storyContent, bool resize = true)
        {
            var cell = new StoryContentCell(_cells.Count, storyContentCellPrefab, storyContent);
            AddCell(cell, resize);
            return cell;
        }
        
        public StoryChoicesCell AddStoryElement(StoryChoices storyChoices, bool resize = true)
        {
            var cell = new StoryChoicesCell(_cells.Count, storyChoiceCellPrefab, storyChoices);
            AddCell(cell, resize);
            return cell;
        }
        
        public void RemoveCell(ScrollViewCell scrollViewCell, bool resize = true)
        {
            if (resize)
                ResetScroller();
            
            _cells.Remove(scrollViewCell);
            
            if(!resize) return;
            
            ResizeScroller();
        }
        
        public void RemoveAtIndex(int cellIndex, bool resize = true)
        {
            if(cellIndex < 0 || 
               cellIndex >= _cells.Count) return;
            
            if (resize)
                ResetScroller();
            
            _cells.RemoveAt(cellIndex);
            
            if(resize)
                ResizeScroller();
        }
        
        public void RemoveLastCell(bool resize = true) => RemoveAtIndex(_cells.Count - 1, resize);

        public void JumpToCell(int cellIndex, float scrollerOffset = 0, float cellOffset = 0)
        {
            eScroller.JumpToDataIndex(cellIndex, scrollerOffset, cellOffset, includeSpacing,
                EnhancedScroller.TweenType.immediate, jumpDuration, ResetSpacer);
        }

        public async UniTask JumpToCellAsync(int cellIndex, float scrollerOffset = 0, float cellOffset = 0)
        {
            await eScroller.JumpToIndexAsync(cellIndex, scrollerOffset, cellOffset,
                includeSpacing, jumpEasing, tweenTime: jumpDuration);
            ResetSpacer();
        }

        #endregion
        
        #region EnhancedScroller Callbacks

        public int GetNumberOfCells(EnhancedScroller scroller) => _cells.Count;

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex) => _cells[dataIndex].CellSize;

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var cell = _cells[dataIndex];
            var baseCellView = scroller.GetCellView(cell.ViewPrefab);

            var cellView = baseCellView as ScrollViewCellView;
            
            if (!cellView) return baseCellView;
           
            cellView.SetCell(cell);

            return cellView;
        }

        #endregion

        #region Private Methods
        
        private void AddCell(ScrollViewCell scrollViewCell, bool resize = true)
        {
            if (resize)
                ResetScroller();
            
            _cells.Add(scrollViewCell);
            
            if(!resize) return;
            
            ResizeScroller();
        }

        private void InitScroller()
        {
            if (!eScroller)
                eScroller = GetComponentInChildren<EnhancedScroller>();
            
            if(!eScroller) return;

            if (Application.isPlaying)
                eScroller.Delegate = this;
            
            if(!scrollerRect)
                scrollerRect = eScroller.transform as RectTransform;
        }

        private void InitCollection()
        {
          
        }
        
        private void InitSpacer()
        {
            if(!addSpacer || !spacerPrefab) return;

            _spacer = new SpacerCell(0, spacerPrefab);
            _cells.Add(_spacer);
            ResizeScroller();
        }

        private void ResetScroller()
        {
            // first, clear out the cells in the enhancedScroller so the new text transforms will be reset
            //eScroller.ClearAll();

            // reset the enhancedScroller's position so that it is not outside of the new bounds
            _oldScrollPosition = eScroller.ScrollPosition;
            eScroller.ScrollPosition = 0;

            // second, reset the data's cell view sizes
            // for (var index = 0; index < _cells.Count; index++)
            // {
            //     _cells[index].CellSize = 0;
            // }
        }

        private void ResizeScroller()
        {
            // capture the scroll rect size.
            // this will be used at the end of this method to determine the final scroll position
            var scrollRectSize = eScroller.ScrollRectSize;

            // capture the scroller's position so we can smoothly scroll from it to the new cell
            var offset = _oldScrollPosition - eScroller.ScrollSize;

            // capture the scroller dimensions so that we can reset them when we are done;
            var size = scrollerRect.sizeDelta;

            // set the dimensions to the largest size possible to acommodate all the cells
            scrollerRect.sizeDelta = new Vector2(size.x, float.MaxValue);

            // First Pass: reload the scroller so that it can populate the text UI elements in the cell view.
            // The content size fitter will determine how big the cells need to be on subsequent passes.
            eScroller.ReloadData();
            
            // reset the scroller size back to what it was originally
            scrollerRect.sizeDelta = size;

            if (addSpacer)
            {
                // calculate the total size required by all cells. This will be used when we determine
                // where to end up at after we reload the data on the second pass.
                _totalCellSize = eScroller.padding.top + eScroller.padding.bottom + 
                                 eScroller.spacing * (_cells.Count - 1);
            
                for (var i = addSpacer? 1: 0; i < _cells.Count; i++)
                {
                    _totalCellSize += _cells[i].CellSize;
                }

                // set the spacer to the entire scroller size.
                // this is necessary because we need some space to actually do a jump
                _spacer.CellSize = scrollRectSize;
            }
            
            // // Second Pass: reload the data once more with the newly set cell view sizes and scroller content size.
            eScroller.ReloadData();

            // set the scroll position to the previous cell (plus the offset of where the scroller currently is) so that we can jump to the new cell.
            if(addSpacer)
                eScroller.ScrollPosition = _totalCellSize - _cells[_cells.Count - 1].CellSize + offset;
            else
                JumpToCell(_cells.Count - 1, 1);    
        }

        private void ResetSpacer()
        {
            if(!addSpacer) return;
            
            // reset the spacer's cell size to the scroller's size minus the rest of the cell sizes
            // (or zero if the spacer is no longer needed)
            _spacer.CellSize = Mathf.Max(eScroller.ScrollRectSize - _totalCellSize, 0);

            // reload the data to set the new cell size
            eScroller.ReloadData(1.0f);
        }
        
        #endregion
    }
}