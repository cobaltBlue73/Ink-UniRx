using System.Threading;
using Cysharp.Threading.Tasks;
using Ink.Runtime;

namespace InkUniRx
{
    public interface IStoryChoicePresenter
    {
        public UniTask OnShowStoryChoiceAsync(Story story, CancellationToken ct);
    }
}