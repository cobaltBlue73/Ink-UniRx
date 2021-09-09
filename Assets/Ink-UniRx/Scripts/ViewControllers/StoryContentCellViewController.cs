using System;
using InkUniRx.ViewModels;
using TMPro;
using UnityEngine;

namespace InlUniRx.ViewControllers
{
    public class StoryContentCellViewController : CellViewController
    {
        #region Inspector

        [SerializeField] private TextMeshProUGUI textMesh;

        #endregion

        #region Uniy Callbacks

        private void OnValidate()
        {
            if (!textMesh)
                textMesh = GetComponentInChildren<TextMeshProUGUI>();
        }

        #endregion
        
        public override void SetCell(ScrollViewCell cell)
        {
            if (cell.StoryPathElement is StoryPathContent content && textMesh)
                textMesh.text = content.Text;
        }

        public override float GetCellViewSize()
        {
            if (!textMesh) return 0;
            
            Canvas.ForceUpdateCanvases();
            
            return textMesh.rectTransform.rect.height;
        }
    }
}