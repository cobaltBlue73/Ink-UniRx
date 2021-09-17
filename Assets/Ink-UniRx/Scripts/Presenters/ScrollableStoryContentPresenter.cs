using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using InkUniRx.Presenters.Interfaces;
using InkUniRx.ViewModels;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility.General;

namespace InkUniRx.Presenters
{
    public class ScrollableStoryContentPresenter : ScrollableStoryPresenter, IStoryContentPresenter
    {
        #region Inspector

        [SerializeField, Required, InlineEditor] private TextMeshProUGUI textMesh;
        [SerializeField, Required, InlineEditor] private LayoutElement textLayoutElement;
        
        #endregion

        #region Unity Callbacks

        protected override void Reset()
        {
            base.Reset();

            if (!textMesh)
                textMesh = GetComponentInChildren<TextMeshProUGUI>();

            if (textMesh && !textLayoutElement)
                textLayoutElement = textMesh.GetOrAddComponent<LayoutElement>();
        }

        private async void Start()
        {
            await UniTask.WaitForEndOfFrame();
            textLayoutElement.minHeight = scrollRect.viewport.rect.height;
        }

        #endregion

        #region Callbacks

        protected override void OnScrollRectValueChanged(Vector2 value)
        {
            Debug.Log(value);
        }

        public UniTask OnShowStoryContentAsync(StoryContent storyContent, CancellationToken ct)
        {
            textMesh.text += storyContent.Text;
            return UniTask.CompletedTask;
        }

        #endregion
       
      
    }
}