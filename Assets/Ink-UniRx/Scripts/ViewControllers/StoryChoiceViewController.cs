using System;
using Ink.Runtime;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace InlUniRx.ViewControllers
{
    public class StoryChoiceViewController : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private TMP_Text textMesh;
        [SerializeField] private Button selectButton;
        
        #endregion

        #region Propeterties

        public IObservable<Choice> WhenChoiceSelected => 
            selectButton? selectButton.OnClickAsObservable().Select(_ => _choice): null;
        
        
        #endregion

        #region Member Variables

        private Choice _choice;

        #endregion

        #region Unity Callbacks

        private void Reset()
        {
            if (!textMesh)
                textMesh = GetComponentInChildren<TMP_Text>();

            if (!selectButton)
                selectButton = GetComponentInChildren<Button>();
        }

        private void OnDisable()
        {
            textMesh.text = string.Empty;
            _choice = null;
        }

        #endregion

        #region Member Methods

        #region Public

        public void SetChoice(Choice choice)
        {
            _choice = choice;
            textMesh.text = choice.text;
        }

        #endregion

        #endregion
    }
}