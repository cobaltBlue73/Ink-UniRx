using System.Threading;
using Cysharp.Threading.Tasks;
using Ink.Runtime;

namespace InkUniRx
{
    public interface IStoryEndingPresenter
    {
        UniTask OnShowStoryEndingAsync(Story story, CancellationToken ct);
    }
}