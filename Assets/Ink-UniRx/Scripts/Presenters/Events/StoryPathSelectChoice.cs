using System.Threading;
using Ink.Runtime;

namespace InkUniRx.Presenters.Events
{
    public class StoryPathSelectChoice: StoryEvent
    {
        public StoryPathSelectChoice(Story story, CancellationToken cancelStoryToken) 
            : base(story, cancelStoryToken) { }
    }
}