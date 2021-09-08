using Ink.Runtime;

namespace InkUniRx.ViewModels
{
    public abstract class StoryPathElement
    {
        public StoryPathElement(Story story)
        {
            Story = story;
            PathName = story.GetCurrentPathName();
        }
        
        public Story Story { get; private set; }
        public string PathName { get; private set; }
    }
}