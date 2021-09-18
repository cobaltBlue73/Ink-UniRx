using System.Threading;
using Ink.Runtime;

namespace InkUniRx.Presenters.Events
{
    public class StoryPathEnd: StoryEvent
    {
        public StoryPathEnd(Story story, CancellationToken cancelStoryToken, CancellationToken cancelAnimationToken)
            : base(story, cancelStoryToken, cancelAnimationToken) { }
    }
}