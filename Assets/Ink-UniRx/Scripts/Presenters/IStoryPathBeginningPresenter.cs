using System.Threading;
using Cysharp.Threading.Tasks;
using Ink.Runtime;

namespace InkUniRx
{
    public interface IStoryPathBeginningPresenter
    {
        UniTask OnShowPathBeginningAsync(Story story, CancellationToken ct);
    }
}