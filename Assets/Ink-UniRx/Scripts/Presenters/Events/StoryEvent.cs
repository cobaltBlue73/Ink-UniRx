using System.Threading;
using Ink.Runtime;

namespace InkUniRx.Presenters.Events
{
    public abstract class StoryEvent
    {
        protected StoryEvent(Story story, CancellationToken cancelStoryToken, CancellationToken cancelAnimationToken)
        {
            Story = story;
            CancelStoryToken = cancelStoryToken;
            CancelAnimationToken = cancelAnimationToken;
        }

        public Story Story { get; }
        public CancellationToken CancelStoryToken { get; }
        public CancellationToken CancelAnimationToken { get; }
    }
}