using System.Threading;
using Cysharp.Threading.Tasks;
using Ink.Runtime;

namespace InkUniRx.Presenters.Interfaces
{
    public interface IStoryStartPresenter
    {
        UniTask OnShowStoryStartAsync(Story story, CancellationToken ct);
    }
}