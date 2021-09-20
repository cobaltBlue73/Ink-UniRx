using System.Threading;
using Ink.Runtime;

namespace InkUniRx.Presenters.Events
{
    public class StoryPathEnd: StoryEvent
    {
        public StoryPathEnd(Story story, CancellationToken cancelStoryToken)
            : base(story, cancelStoryToken) { }
    }
}