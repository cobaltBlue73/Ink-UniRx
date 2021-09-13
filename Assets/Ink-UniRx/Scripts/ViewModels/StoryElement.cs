using Ink.Runtime;

namespace InkUniRx.ViewModels
{
    public abstract class StoryElement
    {
        protected StoryElement(Story story)
        {
            Story = story;
            PathName = story.GetCurrentPathName();
        }
        
        public Story Story { get; }
        public string PathName { get; }
    }
}