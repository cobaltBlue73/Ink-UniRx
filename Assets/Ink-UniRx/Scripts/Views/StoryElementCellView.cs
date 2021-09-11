using InkUniRx.ViewModels;
using UnityEngine;

namespace InkUniRx.Views
{
    public class StoryElementCellView : ScrollViewCellView
    {
        #region Inspector

        [SerializeField] private StoryElementViewBase storyElementView;
        [SerializeField, HideInInspector] private RectTransform storyElementRect;
        
        #endregion

        #region Unity Callbacks

        private void Reset()
        {
            if (!storyElementView)
                storyElementView = GetComponentInChildren<StoryElementViewBase>();

            if (!storyElementRect && storyElementView)
                storyElementRect = storyElementView.transform as RectTransform;
        }

        #endregion

        #region Methods

        public override void SetCell(ScrollViewCell scrollViewCell)
        {
            if (!storyElementView) return;
            
            if (scrollViewCell is StoryElementCell storyCell)
                storyElementView.SetStoryElement(storyCell.StoryElement);
            
            if (!storyElementRect || scrollViewCell.CellSize > 0) return;
            
            Canvas.ForceUpdateCanvases();
            scrollViewCell.CellSize = storyElementRect.rect.height;
        }
        
        #endregion

    }
}