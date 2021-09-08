using System.Threading;
using Cysharp.Threading.Tasks;
using Ink.Runtime;

namespace InkUniRx.Presenters.Interfaces
{
    public interface IStoryPathEndingPresenter
    {
        UniTask OnShowStoryPathEndingAsync(Story story, CancellationToken ct);
    }
}