using System.Threading;
using Ink.Runtime;

namespace InkUniRx.Presenters.Events
{
    public class StoryPathContinue: StoryEvent
    {
        public StoryPathContinue(Story story, CancellationToken cancelStoryToken, CancellationToken cancelAnimationToken) 
            : base(story, cancelStoryToken, cancelAnimationToken) { }
    }
}