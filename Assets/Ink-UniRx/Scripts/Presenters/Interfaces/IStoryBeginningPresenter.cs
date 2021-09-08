using System.Threading;
using Cysharp.Threading.Tasks;
using Ink.Runtime;

namespace InkUniRx.Presenters.Interfaces
{
    public interface IStoryBeginningPresenter
    {
        UniTask OnShowStoryBeginningAsync(Story story, CancellationToken ct);
    }
}