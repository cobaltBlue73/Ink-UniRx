using System;
using System.Collections.Generic;
using Ink.Runtime;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace InkUniRx.PresentationModels
{
    [CreateAssetMenu(menuName = "Ink-UniRx/Presentation Models/Story")]
    public class StoryPresentationModel : ScriptableObject
    {
        #region Inspector

        [SerializeField, Required] private TextAsset storyAsset;
        [SerializeField] private bool continueMaximally;

        #endregion


        #region Properties

        public Story Story => _story ??= new Story(storyAsset.text);
        public string CurrentText { get; private set; } = string.Empty;
        
        #endregion

        #region Private Variables

        private Story _story;
      

        #endregion

        #region Unity Callbacks

        private void Reset()
        {
            
        }

        #endregion

        #region Public Methods

        public void Continue() => 
            CurrentText = continueMaximally ? 
                Story.ContinueMaximally() : 
                Story.Continue();

        #endregion
    }
}