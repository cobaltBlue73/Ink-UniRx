using InkUniRx.ViewModels;
using TMPro;
using UnityEngine;

namespace InlUniRx.Views
{
    public abstract class StoryElementViewBase : MonoBehaviour
    {
        #region Inspector

        [SerializeField] protected TMP_Text textMesh;
        [SerializeField] protected string styleTag = "Normal";

        #endregion

        #region Properties

        public virtual StoryElement StoryElement { get; private set; }

        #endregion

        #region Unity Callbacks

        protected virtual void Reset()
        {
            if (!textMesh)
                textMesh = GetComponentInChildren<TMP_Text>();
        }

        protected virtual void OnDisable()
        {
            StoryElement = null;
            textMesh.text = string.Empty;
        }

        #endregion

        #region Methods
        
        public virtual void SetStoryElement(StoryElement element)
        {
            StoryElement = element;
            textMesh.text = GetStyledText();
        }

        protected virtual string GetStyledText() => $"<style=\"{styleTag}\">{StoryElement.Text}</style>";

        #endregion
    }
}