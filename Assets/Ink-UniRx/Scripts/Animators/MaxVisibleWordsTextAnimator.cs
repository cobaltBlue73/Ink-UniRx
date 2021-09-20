using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using Utility.DoTweenPro;
using Utility.TextMeshPro;

namespace InkUniRx.Animators
{
    public class MaxVisibleWordsTextAnimator: MaxVisibleTextAnimator
    {
        public override async UniTask PlayTextAnimationAsync(TMP_Text textMesh, int fromCharIndex, int toCharIndex,
            CancellationToken cancelAnimationToken)
        {
            textMesh.textInfo.GetFirstAndLastWordIndexFromCharacterIndexRange(fromCharIndex, toCharIndex,
                out var firstWordIndex, out var lastWordIndex);
            
            textMesh.maxVisibleWords = firstWordIndex;
            var targetCount = lastWordIndex + 1;

            await textMesh.DOVisibleWords(targetCount, speed)
                .SetEase(textAnimationEase).SetSpeedBased().SetRecyclable()
                .WithCancellation(cancelAnimationToken).SuppressCancellationThrow();
            
            if(!cancelAnimationToken.IsCancellationRequested) return;

            textMesh.maxVisibleWords = targetCount;
        }
    }
}