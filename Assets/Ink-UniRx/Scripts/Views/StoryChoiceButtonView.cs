using System;
using Ink.Runtime;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace InkUniRx.Views
{
    [RequireComponent(typeof(Button))]
    public class StoryChoiceButtonView : StoryChoiceView
    {
        #region Inspector

        [SerializeField, Required] private TMP_Text label;
        [SerializeField, Required] private Button button;

        #endregion

        #region Propeterties

        public override Choice Choice => _choice;

        public override string Text
        {
            get => label.text;
            set => label.text = value;
        }
        public override IObservable<Choice> WhenSelected => button.OnClickAsObservable().Select(_=> Choice);
        
        #endregion

        #region Member Vairables

        private Choice _choice;

        #endregion

        #region Unity Callbacks
        
        private void Reset()
        {
            if (!label)
                label = GetComponentInChildren<TMP_Text>();
            
            if (!button)
                button = GetComponentInChildren<Button>();
        }

        private void OnDisable()
        {
            _choice = null;
            label.text = string.Empty;
        }

        #endregion

        #region Methods

        public override void SetChoice(Choice choice)
        {
            _choice = choice;
            label.text = choice.text;
        }

        #endregion
    }
}