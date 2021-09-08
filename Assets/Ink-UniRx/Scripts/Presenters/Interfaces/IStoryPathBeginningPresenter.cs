using System.Threading;
using Cysharp.Threading.Tasks;
using Ink.Runtime;

namespace InkUniRx.Presenters.Interfaces
{
    public interface IStoryPathBeginningPresenter
    {
        UniTask OnShowStoryPathBeginningAsync(Story story, CancellationToken ct);
    }
}