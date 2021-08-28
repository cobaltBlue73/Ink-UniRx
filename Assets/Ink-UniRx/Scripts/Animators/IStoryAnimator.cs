using System.Threading;
using Cysharp.Threading.Tasks;

namespace InkUniRx
{
    public interface IStoryViewAnimator
    {
        public UniTask<bool> PlayAnimationAsync(CancellationToken ct);
    }
}