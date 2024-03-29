using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Utility.DoTweenPro;
using Utility.TextMeshPro;

namespace InkUniRx.Animators.Animations
{
    [CreateAssetMenu(menuName = "Ink-UniRx/Animations/Text/MaxVisibleLines")]
    public class MaxVisibleLinesTextAnimation: MaxVisibleTextAnimation
    {
        public override async UniTask PlayAsync(TextMeshAnimator animator, int fromCharIndex, int toCharIndex,
            CancellationToken cancelAnimationToken)
        {
            animator.TextMesh.textInfo.GetFirstAndLastLineIndexFromCharacterIndexRange(fromCharIndex, toCharIndex,
                out var firstLineIndex, out var lastLineIndex);
            
            animator.TextMesh.maxVisibleLines = firstLineIndex;
            var targetCount = lastLineIndex + 1;

            await animator.TextMesh.DOVisibleLines(targetCount, speed)
                .SetEase(textEase).SetSpeedBased().SetRecyclable()
                .WithCancellation(cancelAnimationToken).SuppressCancellationThrow();
            
            if(!cancelAnimationToken.IsCancellationRequested) return;

            animator.TextMesh.maxVisibleLines = targetCount;
        }
    }
}