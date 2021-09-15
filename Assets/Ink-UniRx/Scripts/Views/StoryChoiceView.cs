using System;
using Ink.Runtime;
using InkUniRx.ViewModels;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace InkUniRx.Views
{
    public class StoryChoiceView : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private TMP_Text textMesh;
        [SerializeField] private Button selectButton;
        [SerializeField] private string choiceStyleTag = "Choice";
        //[SerializeField] private bool enumerate;
        [SerializeField] private string numberingStyleTag = "ChoiceNo";

        #endregion

        #region Propeterties

        public Choice Choice { get; private set; }

        public IObservable<int> WhenSelected => selectButton.OnClickAsObservable().Select(_=> Choice.index);
        
        #endregion

        #region Member Variables

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        #endregion
        
        #region Unity Callbacks

        private void Awake()
        {
            _disposables.AddTo(this);
        }

        private void Reset()
        {
            if (!textMesh)
                textMesh = GetComponentInChildren<TMP_Text>();
            
            if (!selectButton)
                selectButton = GetComponentInChildren<Button>();
        }

        protected void OnDisable()
        {
            Choice = null;
            _disposables.Clear();
        }

        #endregion

        #region Member Methods

        #region Public

        public void SetChoice(Choice choice)
        {
            Choice = choice;
            textMesh.text = $"<style=\"{numberingStyleTag}\">{choice.index + 1}.</style><style=\"{choiceStyleTag}\">{choice.text}</style>";
        }

        #endregion

        #region Private
        
        #endregion

        #endregion
    }
}