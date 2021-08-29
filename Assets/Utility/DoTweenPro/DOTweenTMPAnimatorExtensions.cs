using DG.Tweening;
using UnityEngine;

namespace Utility.DoTweenPro
{
    public static class DOTweenTMPAnimatorExtensions
    {
        public static Sequence DoFadeWord(this DOTweenTMPAnimator animator, int wordInfoIndex, float endValue, float duration, float charInterval = 0)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordInfoIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                var atPosition = charInterval * (i - wordInfo.firstCharacterIndex);
                seq.Insert(atPosition,animator.DOFadeChar(i, endValue, duration));
            }
            
            return seq;
        }
        
        public static Sequence DoColorWord(this DOTweenTMPAnimator animator, int wordInfoIndex, Color endValue, float duration, float charInterval = 0)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordInfoIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                var atPosition = charInterval * (i - wordInfo.firstCharacterIndex);
                seq.Insert(atPosition,animator.DOColorChar(i, endValue, duration));
            }
            
            return seq;
        }
        
        public static Sequence DOOffsetWord(this DOTweenTMPAnimator animator, int wordInfoIndex, Vector3 endValue, float duration, float charInterval = 0)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordInfoIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                var atPosition = charInterval * (i - wordInfo.firstCharacterIndex);
                seq.Insert(atPosition,animator.DOOffsetChar(i, endValue, duration));
            }
            
            return seq;
        }
        
        public static Sequence DORotateWord(this DOTweenTMPAnimator animator, int wordInfoIndex, Vector3 endValue, float duration, RotateMode mode = RotateMode.Fast, float charInterval = 0)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordInfoIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                var atPosition = charInterval * (i - wordInfo.firstCharacterIndex);
                seq.Insert(atPosition,animator.DORotateChar(i, endValue, duration, mode));
            }
            
            return seq;
        }
        
        public static Sequence DOScaleWord(this DOTweenTMPAnimator animator, int wordInfoIndex, float endValue, float duration, float charInterval = 0)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordInfoIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                var atPosition = charInterval * (i - wordInfo.firstCharacterIndex);
                seq.Insert(atPosition,animator.DOScaleChar(i, endValue, duration));
            }
            
            return seq;
        }
        
        public static Sequence DOPunchWordOffset(this DOTweenTMPAnimator animator, int wordInfoIndex, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f, float charInterval = 0)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordInfoIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                var atPosition = charInterval * (i - wordInfo.firstCharacterIndex);
                seq.Insert(atPosition,animator.DOPunchCharOffset(i, punch, duration, vibrato, elasticity));
            }
            
            return seq;
        }
        
        public static Sequence DOPunchWordRotation(this DOTweenTMPAnimator animator, int wordInfoIndex, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f, float charInterval = 0)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordInfoIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                var atPosition = charInterval * (i - wordInfo.firstCharacterIndex);
                seq.Insert(atPosition,animator.DOPunchCharRotation(i, punch, duration, vibrato, elasticity));
            }
            
            return seq;
        }
        
        public static Sequence DOPunchWordScale(this DOTweenTMPAnimator animator, int wordInfoIndex, float punch, float duration, int vibrato = 10, float elasticity = 1f, float charInterval = 0)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordInfoIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                var atPosition = charInterval * (i - wordInfo.firstCharacterIndex);
                seq.Insert(atPosition,animator.DOPunchCharScale(i, punch, duration, vibrato, elasticity));
            }
            
            return seq;
        }
        
        public static Sequence DOShakeWordOffset(this DOTweenTMPAnimator animator, int wordInfoIndex, Vector3 strength, float duration, int vibrato = 10, float randomness = 90f, bool fadeOut = true, float charInterval = 0)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordInfoIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                var atPosition = charInterval * (i - wordInfo.firstCharacterIndex);
                seq.Insert(atPosition,animator.DOShakeCharOffset(i, duration, strength, vibrato, randomness, fadeOut));
            }
            
            return seq;
        }
        
        public static Sequence DOShakeWordRotation(this DOTweenTMPAnimator animator, int wordInfoIndex, Vector3 strength, float duration, int vibrato = 10, float randomness = 90f, bool fadeOut = true, float charInterval = 0)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordInfoIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                var atPosition = charInterval * (i - wordInfo.firstCharacterIndex);
                seq.Insert(atPosition,animator.DOShakeCharRotation(i, duration, strength, vibrato, randomness, fadeOut));
            }
            
            return seq;
        }
        
        public static Sequence DOShakeWordScale(this DOTweenTMPAnimator animator, int wordInfoIndex, Vector3 strength, float duration, int vibrato = 10, float randomness = 90f, bool fadeOut = true, float charInterval = 0)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordInfoIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                var atPosition = charInterval * (i - wordInfo.firstCharacterIndex);
                seq.Insert(atPosition,animator.DOShakeCharScale(i, duration, strength, vibrato, randomness, fadeOut));
            }
            
            return seq;
        }
    }
}