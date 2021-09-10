using System.Threading;
using Cysharp.Threading.Tasks;
using Ink.Runtime;

namespace InkUniRx.Presenters.Interfaces
{
    public interface IStoryPathPreStartPresenter
    {
        UniTask OnShowStoryPathPreStartAsync(Story story, CancellationToken ct);
    }
}