using System.Threading;
using Ink.Runtime;

namespace InkUniRx.Presenters.Events
{
    public abstract class StoryEvent
    {
        protected StoryEvent(Story story, CancellationToken cancelStoryToken)
        {
            Story = story;
            CancelStoryToken = cancelStoryToken;
        }

        public Story Story { get; }
        public CancellationToken CancelStoryToken { get; }
        public CancellationToken CancelAnimationToken { get; private set; }
        
        public void SetCancelAnimationToken(CancellationToken cancellationToken) => 
            CancelAnimationToken = cancellationToken;
    }
}