using System.Threading;
using Ink.Runtime;

namespace InkUniRx.Presenters.Events
{
    public class StoryPathStart: StoryEvent
    {
        public StoryPathStart(Story story, CancellationToken cancelStoryToken, CancellationToken cancelAnimationToken) 
            : base(story, cancelStoryToken, cancelAnimationToken) { }
    }
}