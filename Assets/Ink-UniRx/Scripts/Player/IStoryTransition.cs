using System.Threading;
using Cysharp.Threading.Tasks;
using Utility.General;

namespace InkUniRx
{
    public interface IStoryTransition : IPriority
    {
        UniTask RunStoryTransitionAsync(CancellationToken ct);
    }
    
    public interface IBeginStoryTransition : IStoryTransition { }

    public interface IBeginPathTransition : IStoryTransition { }
    
    public interface IEndPathTransition : IStoryTransition { }
    
    public interface INewLineTransition : IStoryTransition { }
}