using System.Threading;
using Cysharp.Threading.Tasks;
using Ink.Runtime;

namespace InkUniRx
{
    public interface IStoryTextPresenter
    {
        UniTask OnShowStoryTextAsync(Story story, CancellationToken ct);
    }

}