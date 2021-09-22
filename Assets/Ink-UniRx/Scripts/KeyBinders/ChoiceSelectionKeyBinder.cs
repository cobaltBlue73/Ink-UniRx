using System;
using Ink.Runtime;
using InkUniRx.Presenters.Events;
using UniRx;
using UnityEngine;

namespace Ink_UniRx.Scripts.KeyBinders
{
    public class ChoiceSelectionKeyBinder: KeyBinder
    {
        #region Variables
        
        private int _choiceCount = -1;
        private int _selectedChoiceIndex = -1;
     
        #endregion
      
        #region Callbacks

        protected override IObservable<Unit> OnStoryStart(StoryStart storyStart)
        {
            AsyncMessageBroker.Default.Subscribe<StoryPathSelectChoice>(selectChoice => {
                
                _choiceCount = selectChoice.Story.currentChoices.Count;
                return Observable.Empty<Unit>();
            }).AddTo(Disposables);
            
            return base.OnStoryStart(storyStart);
        }

        protected override bool GetKeyPress() =>
            _choiceCount >= 0 && 
            (GetKeys(KeyCode.Alpha1) || 
             GetKeys(KeyCode.Keypad1));

        protected override void OnKeyPress()
        {
            Story.ChooseChoiceIndex(_selectedChoiceIndex);
            _selectedChoiceIndex = _choiceCount = -1;
        }

        #region Private

        private bool GetKeys(KeyCode firstKey)
        {
            for (int i = 0; i < _choiceCount; ++i)
            {
                if (!Input.GetKey(firstKey + i)) continue;
                _selectedChoiceIndex = i;
                return true;
            }

            return false;
        }

        #endregion

        #endregion
      
    }
}