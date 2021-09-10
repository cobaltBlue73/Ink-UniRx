using System.Linq.Expressions;
using Ink.Runtime;

namespace InkUniRx.ViewModels
{
    public class StoryChoice: StoryElement
    {
        public StoryChoice(Story story, Choice choice) : base(story)
        {
            Text = choice.text;
            Index = choice.index;
            IsSelected = false;
        }

        public sealed override string Text { get; protected set; }
        
        public int Index { get; private set; }

        public bool IsSelected { get; private set; }

        public void Select()
        {
            IsSelected = true;
            Story.ChooseChoiceIndex(Index);
        }
    }
}