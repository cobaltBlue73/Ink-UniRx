using System.Threading;
using Ink.Runtime;

namespace InkUniRx.Presenters.Events
{
    public class StoryPathStart: StoryEvent
    {
        public StoryPathStart(Story story, CancellationToken cancelStoryToken) 
            : base(story, cancelStoryToken) { }
    }
}