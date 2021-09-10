using System.Threading;
using Cysharp.Threading.Tasks;
using Ink.Runtime;

namespace InkUniRx.Presenters.Interfaces
{
    public interface IStoryEndPresenter
    {
        UniTask OnShowStoryEndAsync(Story story, CancellationToken ct);
    }
}