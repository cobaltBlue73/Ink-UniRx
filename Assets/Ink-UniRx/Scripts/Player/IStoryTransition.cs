using System.Threading;
using Cysharp.Threading.Tasks;
using Utility.General;

namespace InkUniRx
{
    public interface IBeginStoryTransition
    {
        UniTask RunBeginStoryTransitionAsync(CancellationToken ct);
    }

    public interface IBeginPathTransition
    {
        UniTask RunBeginPathTransitionAsync(CancellationToken ct);
    }

    public interface IEndPathTransition
    {
        UniTask RunEndPathTransitionAsync(CancellationToken ct);
    }

    public interface INewLineTransition
    {
        UniTask RunNewLineTransitionAsync(CancellationToken ct);
    }
}