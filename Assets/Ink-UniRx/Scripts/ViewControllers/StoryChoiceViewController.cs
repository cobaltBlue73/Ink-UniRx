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
        [SerializeField] private string bulletStyleName;
        [SerializeField] private string choiceStyleName;
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
            var bullet = string.IsNullOrEmpty(bulletStyleName)? "": $"<style=\"{bulletStyleName}\">{choice.index + 1}.</style>";
            textMesh.text = $"{bullet}<style=\"{choiceStyleName}\">{choice.text}</style>";
        }

        #endregion

        #endregion
    }
}