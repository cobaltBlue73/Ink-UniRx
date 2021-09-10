using System.Threading;
using Cysharp.Threading.Tasks;
using Ink.Runtime;

namespace InkUniRx.Presenters.Interfaces
{
    public interface IStoryPathPostEndPresenter
    {
        UniTask OnShowStoryPathPostEndAsync(Story story, CancellationToken ct);
    }
}