using System.Threading;
using Cysharp.Threading.Tasks;
using Utility.General;

namespace InkUniRx
{
    public interface IBeginStoryTransition
    {
        UniTask PlayBeginStoryTransitionAsync(CancellationToken ct);
    }

    public interface IBeginPathTransition
    {
        UniTask PlayBeginPathTransitionAsync(CancellationToken ct);
    }

    public interface IEndPathTransition
    {
        UniTask PlayEndPathTransitionAsync(CancellationToken ct);
    }

    public interface INewLineTransition
    {
        UniTask PlayNewLineTransitionAsync(CancellationToken ct);
    }
}