using System;
using Ink.Runtime;

namespace InkUniRx.ViewModels
{
    public class StoryChoices: StoryElement
    {
        private readonly Choice[] _choices;
        
        public StoryChoices(Story story) : base(story)
        {
            _choices = story.currentChoices.ToArray();
            SelectedChoice = null;
        }

        public Choice this[int i] => _choices[i];
        public int Count => _choices.Length;
        public Choice SelectedChoice { get; private set; }
        public IObservable<Choice> WhenChoiceSelected => Story.OnMakeChoiceAsObservable();

        public void SelectChoice(int choiceIndex)
        {
            if(SelectedChoice != null || 
               choiceIndex < 0 || 
               choiceIndex >= _choices.Length) return;

            SelectedChoice = _choices[choiceIndex];
            Story.ChooseChoiceIndex(choiceIndex);
        }
    }
}