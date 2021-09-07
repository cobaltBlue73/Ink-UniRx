using System.Threading;
using Cysharp.Threading.Tasks;

namespace InkUniRx
{
    public interface ITextAnimator
    {
        public UniTask PlayTextAnimationAsync(CancellationToken ct);
    }
}