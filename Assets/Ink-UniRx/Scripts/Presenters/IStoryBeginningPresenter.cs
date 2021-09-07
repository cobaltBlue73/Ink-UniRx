using System.Threading;
using Cysharp.Threading.Tasks;
using Ink.Runtime;

namespace InkUniRx
{
    public interface IStoryBeginningPresenter
    {
        UniTask OnShowStoryBeginningAsync(Story story, CancellationToken ct);
    }
}