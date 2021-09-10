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
        [SerializeField, HideInInspector] private RectTransform contentFitterRect;
        
        #endregion

        #region Unity Callbacks

        private void Reset()
        {
            if (!storyElementView)
                storyElementView = GetComponentInChildren<StoryElementViewBase>();

            if (!contentFitterRect)
            {
                var sizeFitter = GetComponentInChildren<ContentSizeFitter>();
                contentFitterRect = sizeFitter.transform as RectTransform;
            }
                
        }

        #endregion
        
        
        public void SetCell(StoryScrollViewCell cell)
        {
            if (storyElementView)
                storyElementView.SetStoryElement(cell.StoryElement);
        }

        public float GetCellViewSize()
        {
            if (!contentFitterRect) return 0;
            
            Canvas.ForceUpdateCanvases();

            return contentFitterRect.rect.height;
        }

    }
}