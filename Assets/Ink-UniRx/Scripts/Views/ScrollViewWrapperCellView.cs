using InkUniRx.ViewModels;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace InkUniRx.Views
{
    public abstract class ScrollViewWrapperCellView<TCellWrapper, TSubViewModel, TSubView>: ScrollViewCellView 
        where TCellWrapper: ScrollViewWrapperCell<TSubViewModel>
        where TSubView: Component
    {
        #region Inspector

        [SerializeField] protected TSubView subView;
        [SerializeField, ReadOnly] protected RectTransform subViewRect;
        [SerializeField, ReadOnly] protected bool hasLayout;
        
        #endregion

        #region Unity Callbacks

        protected virtual void Reset()
        {
            if (!subView)
                subView = GetComponentInChildren<TSubView>();

            if (!subViewRect)
            {
                hasLayout = GetComponentInChildren<LayoutGroup>();
                subViewRect = subView.transform as RectTransform;
            }
        }

        #endregion

        #region Methods

        public override void SetCell(ScrollViewCell scrollViewCell)
        {
            if (!subView) return;

            if (!(scrollViewCell is TCellWrapper wrapper)) return;
            
            SetSubView(wrapper.SubViewModel);
            
            if (scrollViewCell.CellSize > 0) return;
            
            // if (hasLayout)
            //     LayoutRebuilder.ForceRebuildLayoutImmediate(subViewRect);
            // else
                Canvas.ForceUpdateCanvases();
            
            wrapper.CellSize = subViewRect.rect.height;
        }

        protected abstract void SetSubView(TSubViewModel subViewModel);

        #endregion
    }
}