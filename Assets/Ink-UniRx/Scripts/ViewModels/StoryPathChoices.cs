using System;
using Ink.Runtime;
using UniRx;
using UnityEngine;

namespace InkUniRx.ViewModels
{
    public class StoryPathChoices: StoryPathElement
    {
        public StoryPathChoices(Story story) 
            : base(story)
        {
            Choices = story.currentChoices.ToArray();
            SelectedChoice = null;
        }

        public Choice[] Choices { get; private set; }
        
        public Choice SelectedChoice { get; private set; }

        public IObservable<Choice> WhenSelected => Story.OnMakeChoiceAsObservable();

        public void Select(int choiceIndex)
        {
            Debug.Log(choiceIndex);
            if (SelectedChoice != null || 
                choiceIndex < 0 || 
                choiceIndex >= Choices.Length) return;

            
            SelectedChoice = Choices[choiceIndex];
            Story.ChooseChoiceIndex(choiceIndex);
        }
    }
}