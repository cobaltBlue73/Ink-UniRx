using System.Threading;
using Ink.Runtime;

namespace InkUniRx.Presenters.Events
{
    public class StoryPathNewText: StoryEvent
    {
        public StoryPathNewText(Story story, CancellationToken cancelStoryToken) 
            : base(story, cancelStoryToken) { }
    }
}