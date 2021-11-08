using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Utility.DoTweenPro;

namespace InkUniRx.Animators.Animations
{
    [CreateAssetMenu(menuName = "Ink-UniRx/Animations/Text/MaxVisibleCharacters")]
    public class MaxVisibleCharactersTextAnimation: MaxVisibleTextAnimation
    {
        public override async UniTask PlayAsync(TextMeshAnimator animator, int fromCharIndex, int toCharIndex,
            CancellationToken cancelAnimationToken)
        {
            animator.TextMesh.maxVisibleCharacters = fromCharIndex;
            var targetCount = toCharIndex + 1;
            
            await animator.TextMesh.DOMaxVisibleCharacters(targetCount, speed)
                .SetEase(textEase).SetSpeedBased().SetRecyclable()
                .WithCancellation(cancelAnimationToken).SuppressCancellationThrow();
            
            if(!cancelAnimationToken.IsCancellationRequested) return;

            animator.TextMesh.maxVisibleCharacters = targetCount;
        }
    }
}