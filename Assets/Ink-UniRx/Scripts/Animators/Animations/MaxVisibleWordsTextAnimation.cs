using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Utility.DoTweenPro;
using Utility.TextMeshPro;

namespace InkUniRx.Animators.Animations
{
    [CreateAssetMenu(fileName = nameof(MaxVisibleWordsTextAnimation), menuName = "Ink-UniRx/Animations/Text/MaxVisibleWords", order = 0)]
    public class MaxVisibleWordsTextAnimation: MaxVisibleTextAnimation
    {
        public override async UniTask AnimateTextAsync(TextMeshAnimator animator, int fromCharIndex, int toCharIndex,
            CancellationToken cancelAnimationToken)
        {
            animator.TextMesh.textInfo.GetFirstAndLastWordIndexFromCharacterIndexRange(fromCharIndex, toCharIndex,
                out var firstWordIndex, out var lastWordIndex);
            
            animator.TextMesh.maxVisibleWords = firstWordIndex;
            var targetCount = lastWordIndex + 1;

            await animator.TextMesh.DOVisibleWords(targetCount, speed)
                .SetEase(textEase).SetSpeedBased().SetRecyclable()
                .WithCancellation(cancelAnimationToken).SuppressCancellationThrow();
            
            if(!cancelAnimationToken.IsCancellationRequested) return;

            animator.TextMesh.maxVisibleWords = targetCount;
        }
    }
}