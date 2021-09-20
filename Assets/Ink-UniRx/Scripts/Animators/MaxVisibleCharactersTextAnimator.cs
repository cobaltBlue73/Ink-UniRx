using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using Utility.DoTweenPro;

namespace InkUniRx.Animators
{
    public class MaxVisibleCharactersTextAnimator: MaxVisibleTextAnimator
    {
        public override async UniTask PlayTextAnimationAsync(TMP_Text textMesh, int fromCharIndex, int toCharIndex,
            CancellationToken cancelAnimationToken)
        {
            textMesh.maxVisibleCharacters = fromCharIndex;
            var targetCount = toCharIndex + 1;
            await textMesh.DOMaxVisibleCharacters(targetCount, speed)
                .SetEase(textAnimationEase).SetSpeedBased().SetRecyclable()
                .WithCancellation(cancelAnimationToken).SuppressCancellationThrow();
            
            if(!cancelAnimationToken.IsCancellationRequested) return;

            textMesh.maxVisibleCharacters = targetCount;
        }
    }
}