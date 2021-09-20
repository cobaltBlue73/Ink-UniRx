using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using Utility.DoTweenPro;
using Utility.TextMeshPro;

namespace InkUniRx.Animators
{
    public class MaxVisibleLinesTextAnimator: MaxVisibleTextAnimator
    {
        public override async UniTask PlayTextAnimationAsync(TMP_Text textMesh, int fromCharIndex, int toCharIndex,
            CancellationToken cancelAnimationToken)
        {
            textMesh.textInfo.GetFirstAndLastLineIndexFromCharacterIndexRange(fromCharIndex, toCharIndex,
                out var firstLineIndex, out var lastLineIndex);
            
            textMesh.maxVisibleLines = firstLineIndex;
            var targetCount = lastLineIndex + 1;

            await textMesh.DOVisibleLines(targetCount, speed)
                .SetEase(textAnimationEase).SetSpeedBased().SetRecyclable()
                .WithCancellation(cancelAnimationToken).SuppressCancellationThrow();
            
            if(!cancelAnimationToken.IsCancellationRequested) return;

            textMesh.maxVisibleLines = targetCount;
        }
    }
}