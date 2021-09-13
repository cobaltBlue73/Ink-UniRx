using InkUniRx.ViewModels;
using TMPro;
using UnityEngine;

namespace InkUniRx.Views
{
    public class StoryContentView : StoryElementViewBase
    {
        #region Inspector

        [SerializeField] protected TMP_Text textMesh;
        
        #endregion

        #region Properties

        public override StoryElement StoryElement => _storyContent; 

        #endregion

        #region Member Vairables

        private StoryContent _storyContent;

        #endregion

        #region Unity Callbacks

        protected void Reset()
        {
            if (!textMesh)
                textMesh = GetComponentInChildren<TMP_Text>();
        }

        protected void OnDisable()
        {
            _storyContent = null;
            textMesh.text = string.Empty;
        }

        #endregion

        #region Methods
        
        public override void SetStoryElement(StoryElement element)
        {
            _storyContent = element as StoryContent;
            textMesh.text = _storyContent?.Text;
        }
        
        #endregion
    }
}