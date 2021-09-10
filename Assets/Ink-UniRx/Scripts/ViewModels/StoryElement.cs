using Ink.Runtime;

namespace InkUniRx.ViewModels
{
    public abstract class StoryElement
    {
        public StoryElement(Story story)
        {
            Story = story;
            PathName = story.GetCurrentPathName();
        }
        
        public Story Story { get; private set; }
        public string PathName { get; private set; }

        public abstract string Text { get; protected set; }
    }
}