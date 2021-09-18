using System.Threading;
using Ink.Runtime;

namespace InkUniRx.Presenters.Events
{
    public class StoryPathChoiceSelection: StoryEvent
    {
        public StoryPathChoiceSelection(Story story, CancellationToken cancelStoryToken, CancellationToken cancelAnimationToken) 
            : base(story, cancelStoryToken, cancelAnimationToken) { }
    }
}