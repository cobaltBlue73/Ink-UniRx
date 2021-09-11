using System;
using EnhancedUI.EnhancedScroller;
using InkUniRx.ViewModels;
using UnityEngine;
using UnityEngine.UI;

namespace InlUniRx.Views
{
    public class StoryScrollViewCellView : EnhancedScrollerCellView
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
        
        
        public void SetCell(StoryScrollViewCell cell, bool updateSize)
        {
            if (storyElementView)
                storyElementView.SetStoryElement(cell.StoryElement);

            if (!updateSize || !storyElementRect) return;
            
            Canvas.ForceUpdateCanvases();
            cell.CellViewSize = storyElementRect.rect.height;
        }
    }
}