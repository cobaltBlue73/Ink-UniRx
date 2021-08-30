using DG.Tweening;
using UnityEngine;

namespace Utility.DOTweenPro
{
    public static class DOTweenTMPAnimatorExtensions
    {
        public static Sequence DOFadeWord(this DOTweenTMPAnimator animator, int wordInfoIndex, float endValue, float duration,  Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordInfoIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                seq.Join(animator.DOFadeChar(i, endValue, duration).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOColorWord(this DOTweenTMPAnimator animator, int wordInfoIndex, Color endValue, float duration,  Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordInfoIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                seq.Join(animator.DOColorChar(i, endValue, duration).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOOffsetWord(this DOTweenTMPAnimator animator, int wordInfoIndex, Vector3 endValue, float duration,  Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordInfoIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                seq.Join(animator.DOOffsetChar(i, endValue, duration).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DORotateWord(this DOTweenTMPAnimator animator, int wordInfoIndex, Vector3 endValue, float duration, Ease ease = Ease.OutQuad, RotateMode mode = RotateMode.Fast)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordInfoIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                seq.Join(animator.DORotateChar(i, endValue, duration, mode).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOScaleWord(this DOTweenTMPAnimator animator, int wordInfoIndex, float endValue, float duration,  Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordInfoIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                seq.Join(animator.DOScaleChar(i, endValue, duration).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOPunchWordOffset(this DOTweenTMPAnimator animator, int wordInfoIndex, Vector3 punch, float duration, Ease ease = Ease.OutQuad, int vibrato = 10, float elasticity = 1f)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordInfoIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                seq.Join(animator.DOPunchCharOffset(i, punch, duration, vibrato, elasticity).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOPunchWordRotation(this DOTweenTMPAnimator animator, int wordInfoIndex, Vector3 punch, float duration, Ease ease = Ease.OutQuad, int vibrato = 10, float elasticity = 1f)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordInfoIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                seq.Join(animator.DOPunchCharRotation(i, punch, duration, vibrato, elasticity).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOPunchWordScale(this DOTweenTMPAnimator animator, int wordInfoIndex, float punch, float duration, Ease ease = Ease.OutQuad, int vibrato = 10, float elasticity = 1f)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordInfoIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                seq.Join(animator.DOPunchCharScale(i, punch, duration, vibrato, elasticity).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOShakeWordOffset(this DOTweenTMPAnimator animator, int wordInfoIndex, Vector3 strength, float duration, Ease ease = Ease.OutQuad, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordInfoIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                seq.Join(animator.DOShakeCharOffset(i, duration, strength, vibrato, randomness, fadeOut).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOShakeWordRotation(this DOTweenTMPAnimator animator, int wordInfoIndex, Vector3 strength, float duration, Ease ease = Ease.OutQuad, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordInfoIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                seq.Join(animator.DOShakeCharRotation(i, duration, strength, vibrato, randomness, fadeOut).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOShakeWordScale(this DOTweenTMPAnimator animator, int wordInfoIndex, Vector3 strength, float duration, Ease ease = Ease.OutQuad, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordInfoIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                seq.Join(animator.DOShakeCharScale(i, duration, strength, vibrato, randomness, fadeOut).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOFadeLine(this DOTweenTMPAnimator animator, int lineInfoIndex, float endValue, float duration,  Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            
            for (int lineIdx = 0, wordIdx = 0; lineIdx <= lineInfoIndex; lineIdx++)
            {
                if (lineIdx < lineInfoIndex)
                {
                    wordIdx += animator.textInfo.lineInfo[lineIdx].wordCount;
                    continue;
                }
                
                for (int i = wordIdx; i < animator.textInfo.lineInfo[lineIdx].wordCount; i++)
                {
                    seq.Join(animator.DOFadeWord(i, endValue, duration, ease));
                }
                break;
            }
            
            return seq;
        }
        
        public static Sequence DOColorLine(this DOTweenTMPAnimator animator, int lineInfoIndex, Color endValue, float duration,  Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            
            for (int lineIdx = 0, wordIdx = 0; lineIdx <= lineInfoIndex; lineIdx++)
            {
                if (lineIdx < lineInfoIndex)
                {
                    wordIdx += animator.textInfo.lineInfo[lineIdx].wordCount;
                    continue;
                }
                
                for (int i = wordIdx; i < animator.textInfo.lineInfo[lineIdx].wordCount; i++)
                {
                    seq.Join(animator.DOColorWord(i, endValue, duration, ease));
                }
                break;
            }

            return seq;
        }
        
        public static Sequence DOOffsetLine(this DOTweenTMPAnimator animator, int lineInfoIndex, Vector3 endValue, float duration,  Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            
            for (int lineIdx = 0, wordIdx = 0; lineIdx <= lineInfoIndex; lineIdx++)
            {
                if (lineIdx < lineInfoIndex)
                {
                    wordIdx += animator.textInfo.lineInfo[lineIdx].wordCount;
                    continue;
                }
                
                for (int i = wordIdx; i < animator.textInfo.lineInfo[lineIdx].wordCount; i++)
                {
                    seq.Join(animator.DOOffsetWord(i, endValue, duration, ease));
                }
                break;
            }
            
            return seq;
        }
        
        public static Sequence DORotateLine(this DOTweenTMPAnimator animator, int lineInfoIndex, Vector3 endValue, float duration, Ease ease = Ease.OutQuad, RotateMode mode = RotateMode.Fast)
        {
            var seq = DOTween.Sequence();
            for (int lineIdx = 0, wordIdx = 0; lineIdx <= lineInfoIndex; lineIdx++)
            {
                if (lineIdx < lineInfoIndex)
                {
                    wordIdx += animator.textInfo.lineInfo[lineIdx].wordCount;
                    continue;
                }
                
                for (int i = wordIdx; i < animator.textInfo.lineInfo[lineIdx].wordCount; i++)
                {
                    seq.Join(animator.DORotateWord(i, endValue, duration, ease, mode));
                }
                break;
            }

            return seq;
        }
        
        public static Sequence DOScaleLine(this DOTweenTMPAnimator animator, int lineInfoIndex, float endValue, float duration,  Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            
            for (int lineIdx = 0, wordIdx = 0; lineIdx <= lineInfoIndex; lineIdx++)
            {
                if (lineIdx < lineInfoIndex)
                {
                    wordIdx += animator.textInfo.lineInfo[lineIdx].wordCount;
                    continue;
                }
                
                for (int i = wordIdx; i < animator.textInfo.lineInfo[lineIdx].wordCount; i++)
                {
                    seq.Join(animator.DOScaleWord(i, endValue, duration, ease));
                }
                break;
            }       
            
            return seq;
        }
        
        public static Sequence DOPunchLineOffset(this DOTweenTMPAnimator animator, int lineInfoIndex, Vector3 punch, float duration, Ease ease = Ease.OutQuad, int vibrato = 10, float elasticity = 1f)
        {
            var seq = DOTween.Sequence();
          
            for (int lineIdx = 0, wordIdx = 0; lineIdx <= lineInfoIndex; lineIdx++)
            {
                if (lineIdx < lineInfoIndex)
                {
                    wordIdx += animator.textInfo.lineInfo[lineIdx].wordCount;
                    continue;
                }
                
                for (int i = wordIdx; i < animator.textInfo.lineInfo[lineIdx].wordCount; i++)
                {
                    seq.Join(animator.DOPunchWordOffset(i, punch, duration, ease, vibrato, elasticity));
                }
                break;
            }      
            
            return seq;
        }
        
        public static Sequence DOPunchLineRotation(this DOTweenTMPAnimator animator, int lineInfoIndex, Vector3 punch, float duration, Ease ease = Ease.OutQuad, int vibrato = 10, float elasticity = 1f)
        {
            var seq = DOTween.Sequence();

            for (int lineIdx = 0, wordIdx = 0; lineIdx <= lineInfoIndex; lineIdx++)
            {
                if (lineIdx < lineInfoIndex)
                {
                    wordIdx += animator.textInfo.lineInfo[lineIdx].wordCount;
                    continue;
                }
                
                for (int i = wordIdx; i < animator.textInfo.lineInfo[lineIdx].wordCount; i++)
                {
                    seq.Join(animator.DOPunchWordRotation(i, punch, duration, ease, vibrato, elasticity));
                }
                break;
            }

            return seq;
        }
        
        public static Sequence DOPunchLineScale(this DOTweenTMPAnimator animator, int lineInfoIndex, float punch, float duration, Ease ease = Ease.OutQuad, int vibrato = 10, float elasticity = 1f)
        {
            var seq = DOTween.Sequence();

            for (int lineIdx = 0, wordIdx = 0; lineIdx <= lineInfoIndex; lineIdx++)
            {
                if (lineIdx < lineInfoIndex)
                {
                    wordIdx += animator.textInfo.lineInfo[lineIdx].wordCount;
                    continue;
                }
                
                for (int i = wordIdx; i < animator.textInfo.lineInfo[lineIdx].wordCount; i++)
                {
                    seq.Join(animator.DOPunchWordScale(i, punch, duration, ease, vibrato, elasticity));
                }
                break;
            }

            return seq;
        }
        
        public static Sequence DOShakeLineOffset(this DOTweenTMPAnimator animator, int lineInfoIndex, Vector3 strength, float duration, Ease ease = Ease.OutQuad, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
        {
            var seq = DOTween.Sequence();
            
            for (int lineIdx = 0, wordIdx = 0; lineIdx <= lineInfoIndex; lineIdx++)
            {
                if (lineIdx < lineInfoIndex)
                {
                    wordIdx += animator.textInfo.lineInfo[lineIdx].wordCount;
                    continue;
                }
                
                for (int i = wordIdx; i < animator.textInfo.lineInfo[lineIdx].wordCount; i++)
                {
                    seq.Join(animator.DOShakeWordOffset(i, strength, duration, ease, vibrato, randomness, fadeOut));
                }
                break;
            }

            return seq;
        }
        
        public static Sequence DOShakeLineRotation(this DOTweenTMPAnimator animator, int lineInfoIndex, Vector3 strength, float duration, Ease ease = Ease.OutQuad, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
        {
            var seq = DOTween.Sequence();
            
            for (int lineIdx = 0, wordIdx = 0; lineIdx <= lineInfoIndex; lineIdx++)
            {
                if (lineIdx < lineInfoIndex)
                {
                    wordIdx += animator.textInfo.lineInfo[lineIdx].wordCount;
                    continue;
                }
                
                for (int i = wordIdx; i < animator.textInfo.lineInfo[lineIdx].wordCount; i++)
                {
                    seq.Join(animator.DOShakeWordRotation(i, strength, duration, ease, vibrato, randomness, fadeOut));
                }
                break;
            }         
            
            return seq;
        }
        
        public static Sequence DOShakeLineScale(this DOTweenTMPAnimator animator, int lineInfoIndex, Vector3 strength, float duration, Ease ease = Ease.OutQuad, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
        {
            var seq = DOTween.Sequence();
            
            for (int lineIdx = 0, wordIdx = 0; lineIdx <= lineInfoIndex; lineIdx++)
            {
                if (lineIdx < lineInfoIndex)
                {
                    wordIdx += animator.textInfo.lineInfo[lineIdx].wordCount;
                    continue;
                }
                
                for (int i = wordIdx; i < animator.textInfo.lineInfo[lineIdx].wordCount; i++)
                {
                    seq.Join(animator.DOShakeWordScale(i,  strength, duration, ease, vibrato, randomness, fadeOut));
                }
                break;
            }          
            
            return seq;
        }
    }
}