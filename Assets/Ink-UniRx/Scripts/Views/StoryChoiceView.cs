using System;
using Ink.Runtime;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace InkUniRx
{
    [RequireComponent(typeof(Button))]
    public class StoryChoiceView : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI text;

        #endregion

        #region Properties

        public IObservable<Choice> WhenChoiceSelection => _choiceSelection.AsObservable();

        #endregion

        #region Member Variables

        private Choice _choice;
        private readonly Subject<Choice> _choiceSelection = new Subject<Choice>();
        
        #endregion

        #region Unity Callbacks

        private void OnValidate()
        {
            if (!button)
                button = GetComponent<Button>();

            if (text)
                text = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Awake()
        {
            _choiceSelection.AddTo(this);
            button.OnClickAsObservable().Select(_ => _choice)
                .Subscribe(_choiceSelection).AddTo(this);
        }

        #endregion

        #region Public Methods

        public void SetChoice(Choice choice)
        {
            _choice = choice;
            text.text = choice.text;
        }

        #endregion
    }
}