using System;
using Ink.Runtime;
using InkUniRx.ViewModels;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace InkUniRx.Views
{
    public class StoryChoiceView : StoryElementViewGeneric<StoryChoice>
    {
        #region Inspector
        
        [SerializeField] private Button selectButton;
        [SerializeField] private string numberingStyleTag = "ChoiceNo";

        #endregion

        #region Propeterties

        
        #endregion

        #region Member Variables

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        #endregion
        
        #region Unity Callbacks

        private void Awake()
        {
            _disposables.AddTo(this);
        }

        protected override void Reset()
        {
            base.Reset();
            
            styleTag = "Choice";
            
            if (!selectButton)
                selectButton = GetComponentInChildren<Button>();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _disposables.Clear();
        }

        #endregion

        #region Member Methods

        #region Public

        public override void SetStoryElement(StoryElement element)
        {
            base.SetStoryElement(element);
            selectButton.OnClickAsObservable()
                .Subscribe(_ => StoryElement?.Select())
                .AddTo(_disposables);
        }

        #endregion

        #region Protected

        protected override string GetStyledText() => 
            $"<style=\"{numberingStyleTag}\">{StoryElement.Index + 1}.</style>{base.GetStyledText()}";

        #endregion

        #endregion
    }
}