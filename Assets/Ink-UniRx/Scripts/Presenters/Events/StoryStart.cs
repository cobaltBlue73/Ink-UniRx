using System.Threading;
using Ink.Runtime;

namespace InkUniRx.Presenters.Events
{
    public class StoryStart: StoryEvent
    {
        public StoryStart(Story story, CancellationToken cancelStoryToken, CancellationToken cancelAnimationToken) 
            : base(story, cancelStoryToken, cancelAnimationToken) { }
    }
}