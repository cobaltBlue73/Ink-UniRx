using System.Threading;
using Cysharp.Threading.Tasks;
using Ink.Runtime;

namespace InkUniRx
{
    public interface IStoryPathEndingPresenter
    {
        UniTask OnShowPathEndingAsync(Story story, CancellationToken ct);
    }
}