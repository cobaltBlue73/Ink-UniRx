using System.Threading;
using Ink.Runtime;

namespace InkUniRx.Presenters.Events
{
    public class StoryEnd: StoryEvent
    {
        public StoryEnd(Story story, CancellationToken cancelStoryToken, CancellationToken cancelAnimationToken) 
            : base(story, cancelStoryToken, cancelAnimationToken) { }
    }
}