using System.Threading;
using Ink.Runtime;

namespace InkUniRx.Presenters.Events
{
    public class StoryPathNewContent: StoryEvent
    {
        public StoryPathNewContent(Story story, CancellationToken cancelStoryToken) 
            : base(story, cancelStoryToken) { }
    }
}