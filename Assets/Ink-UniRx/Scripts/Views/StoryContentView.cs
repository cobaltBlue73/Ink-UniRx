using InkUniRx.ViewModels;
using TMPro;
using UnityEngine;

namespace InlUniRx.Views
{
    public class StoryContentView : StoryElementViewGeneric<StoryContent>
    {
        #region Unity Callbacks

        protected override void Reset()
        {
            base.Reset();
            
            if (!textMesh)
                textMesh = GetComponentInChildren<TextMeshProUGUI>();
        }

        #endregion
        
    }
}