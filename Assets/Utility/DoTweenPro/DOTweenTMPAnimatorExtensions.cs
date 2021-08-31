using DG.Tweening;
using UnityEngine;

namespace Utility.DOTweenPro
{
    public static class DOTweenTMPAnimatorExtensions
    {
        #region Word Tweens

        public static Sequence DOFadeWord(this DOTweenTMPAnimator animator, int wordIndex, float endValue, float duration,  Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                seq.Join(animator.DOFadeChar(i, endValue, duration).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOColorWord(this DOTweenTMPAnimator animator, int wordIndex, Color endValue, float duration,  Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                seq.Join(animator.DOColorChar(i, endValue, duration).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOOffsetWord(this DOTweenTMPAnimator animator, int wordIndex, Vector3 endValue, float duration,  Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                seq.Join(animator.DOOffsetChar(i, endValue, duration).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DORotateWord(this DOTweenTMPAnimator animator, int wordIndex, Vector3 endValue, float duration, Ease ease = Ease.OutQuad, RotateMode mode = RotateMode.Fast)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                seq.Join(animator.DORotateChar(i, endValue, duration, mode).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOScaleWord(this DOTweenTMPAnimator animator, int wordIndex, float endValue, float duration,  Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                seq.Join(animator.DOScaleChar(i, endValue, duration).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOPunchWordOffset(this DOTweenTMPAnimator animator, int wordIndex, Vector3 punch, float duration, Ease ease = Ease.OutQuad, int vibrato = 10, float elasticity = 1f)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                seq.Join(animator.DOPunchCharOffset(i, punch, duration, vibrato, elasticity).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOPunchWordRotation(this DOTweenTMPAnimator animator, int wordIndex, Vector3 punch, float duration, Ease ease = Ease.OutQuad, int vibrato = 10, float elasticity = 1f)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                seq.Join(animator.DOPunchCharRotation(i, punch, duration, vibrato, elasticity).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOPunchWordScale(this DOTweenTMPAnimator animator, int wordIndex, float punch, float duration, Ease ease = Ease.OutQuad, int vibrato = 10, float elasticity = 1f)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                seq.Join(animator.DOPunchCharScale(i, punch, duration, vibrato, elasticity).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOShakeWordOffset(this DOTweenTMPAnimator animator, int wordIndex, Vector3 strength, float duration, Ease ease = Ease.OutQuad, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                seq.Join(animator.DOShakeCharOffset(i, duration, strength, vibrato, randomness, fadeOut).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOShakeWordRotation(this DOTweenTMPAnimator animator, int wordIndex, Vector3 strength, float duration, Ease ease = Ease.OutQuad, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                seq.Join(animator.DOShakeCharRotation(i, duration, strength, vibrato, randomness, fadeOut).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOShakeWordScale(this DOTweenTMPAnimator animator, int wordIndex, Vector3 strength, float duration, Ease ease = Ease.OutQuad, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordIndex];
            for (int i = wordInfo.firstCharacterIndex; i < wordInfo.characterCount; i++)
            {
                seq.Join(animator.DOShakeCharScale(i, duration, strength, vibrato, randomness, fadeOut).SetEase(ease));
            }
            
            return seq;
        }

        #endregion

        #region Line Tweens
        
        public static Sequence DOFadeLine(this DOTweenTMPAnimator animator, int lineIndex, float endValue, float duration,  Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            
            for (int lineIdx = 0, wordIdx = 0; lineIdx <= lineIndex && 
                                               lineIdx < animator.textInfo.lineCount; lineIdx++)
            {
                if (lineIdx < lineIndex)
                {
                    wordIdx += animator.textInfo.lineInfo[lineIdx].wordCount;
                    continue;
                }
                
                var lineInfo = animator.textInfo.lineInfo[lineIdx];
                for (int i = wordIdx; i < lineInfo.wordCount; i++)
                {
                    seq.Join(animator.DOFadeWord(i, endValue, duration, ease));
                }
                break;
            }
            
            return seq;
        }
        
        public static Sequence DOColorLine(this DOTweenTMPAnimator animator, int lineIndex, Color endValue, float duration,  Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            
            for (int lineIdx = 0, wordIdx = 0; lineIdx <= lineIndex && 
                                               lineIdx < animator.textInfo.lineCount; lineIdx++)
            {
                if (lineIdx < lineIndex)
                {
                    wordIdx += animator.textInfo.lineInfo[lineIdx].wordCount;
                    continue;
                }
                
                var lineInfo = animator.textInfo.lineInfo[lineIdx];
                for (int i = wordIdx; i < lineInfo.wordCount; i++)
                {
                    seq.Join(animator.DOColorWord(i, endValue, duration, ease));
                }
                break;
            }

            return seq;
        }
        
        public static Sequence DOOffsetLine(this DOTweenTMPAnimator animator, int lineIndex, Vector3 endValue, float duration,  Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            
            for (int lineIdx = 0, wordIdx = 0; lineIdx <= lineIndex && 
                                               lineIdx < animator.textInfo.lineCount; lineIdx++)
            {
                if (lineIdx < lineIndex)
                {
                    wordIdx += animator.textInfo.lineInfo[lineIdx].wordCount;
                    continue;
                }
                
                var lineInfo = animator.textInfo.lineInfo[lineIdx];
                for (int i = wordIdx; i < lineInfo.wordCount; i++)
                {
                    seq.Join(animator.DOOffsetWord(i, endValue, duration, ease));
                }
                break;
            }
            
            return seq;
        }
        
        public static Sequence DORotateLine(this DOTweenTMPAnimator animator, int lineIndex, Vector3 endValue, float duration, Ease ease = Ease.OutQuad, RotateMode mode = RotateMode.Fast)
        {
            var seq = DOTween.Sequence();
            for (int lineIdx = 0, wordIdx = 0; lineIdx <= lineIndex && 
                                               lineIdx < animator.textInfo.lineCount; lineIdx++)
            {
                if (lineIdx < lineIndex)
                {
                    wordIdx += animator.textInfo.lineInfo[lineIdx].wordCount;
                    continue;
                }
                
                var lineInfo = animator.textInfo.lineInfo[lineIdx];
                for (int i = wordIdx; i < lineInfo.wordCount; i++)
                {
                    seq.Join(animator.DORotateWord(i, endValue, duration, ease, mode));
                }
                break;
            }

            return seq;
        }
        
        public static Sequence DOScaleLine(this DOTweenTMPAnimator animator, int lineIndex, float endValue, float duration,  Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            
            for (int lineIdx = 0, wordIdx = 0; lineIdx <= lineIndex && 
                                               lineIdx < animator.textInfo.lineCount; lineIdx++)
            {
                if (lineIdx < lineIndex)
                {
                    wordIdx += animator.textInfo.lineInfo[lineIdx].wordCount;
                    continue;
                }
                
                var lineInfo = animator.textInfo.lineInfo[lineIdx];
                for (int i = wordIdx; i < lineInfo.wordCount; i++)
                {
                    seq.Join(animator.DOScaleWord(i, endValue, duration, ease));
                }
                break;
            }       
            
            return seq;
        }
        
        public static Sequence DOPunchLineOffset(this DOTweenTMPAnimator animator, int lineIndex, Vector3 punch, float duration, Ease ease = Ease.OutQuad, int vibrato = 10, float elasticity = 1f)
        {
            var seq = DOTween.Sequence();
          
            for (int lineIdx = 0, wordIdx = 0; lineIdx <= lineIndex && 
                                               lineIdx < animator.textInfo.lineCount; lineIdx++)
            {
                if (lineIdx < lineIndex)
                {
                    wordIdx += animator.textInfo.lineInfo[lineIdx].wordCount;
                    continue;
                }
                
                var lineInfo = animator.textInfo.lineInfo[lineIdx];
                for (int i = wordIdx; i < lineInfo.wordCount; i++)
                {
                    seq.Join(animator.DOPunchWordOffset(i, punch, duration, ease, vibrato, elasticity));
                }
                break;
            }      
            
            return seq;
        }
        
        public static Sequence DOPunchLineRotation(this DOTweenTMPAnimator animator, int lineIndex, Vector3 punch, float duration, Ease ease = Ease.OutQuad, int vibrato = 10, float elasticity = 1f)
        {
            var seq = DOTween.Sequence();

            for (int lineIdx = 0, wordIdx = 0; lineIdx <= lineIndex && 
                                               lineIdx < animator.textInfo.lineCount; lineIdx++)
            {
                if (lineIdx < lineIndex)
                {
                    wordIdx += animator.textInfo.lineInfo[lineIdx].wordCount;
                    continue;
                }
                
                var lineInfo = animator.textInfo.lineInfo[lineIdx];
                for (int i = wordIdx; i < lineInfo.wordCount; i++)
                {
                    seq.Join(animator.DOPunchWordRotation(i, punch, duration, ease, vibrato, elasticity));
                }
                break;
            }

            return seq;
        }
        
        public static Sequence DOPunchLineScale(this DOTweenTMPAnimator animator, int lineIndex, float punch, float duration, Ease ease = Ease.OutQuad, int vibrato = 10, float elasticity = 1f)
        {
            var seq = DOTween.Sequence();

            for (int lineIdx = 0, wordIdx = 0; lineIdx <= lineIndex && 
                                               lineIdx < animator.textInfo.lineCount; lineIdx++)
            {
                if (lineIdx < lineIndex)
                {
                    wordIdx += animator.textInfo.lineInfo[lineIdx].wordCount;
                    continue;
                }
                
                var lineInfo = animator.textInfo.lineInfo[lineIdx];
                for (int i = wordIdx; i < lineInfo.wordCount; i++)
                {
                    seq.Join(animator.DOPunchWordScale(i, punch, duration, ease, vibrato, elasticity));
                }
                break;
            }

            return seq;
        }
        
        public static Sequence DOShakeLineOffset(this DOTweenTMPAnimator animator, int lineIndex, Vector3 strength, float duration, Ease ease = Ease.OutQuad, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
        {
            var seq = DOTween.Sequence();
            
            for (int lineIdx = 0, wordIdx = 0; lineIdx <= lineIndex && 
                                               lineIdx < animator.textInfo.lineCount; lineIdx++)
            {
                if (lineIdx < lineIndex)
                {
                    wordIdx += animator.textInfo.lineInfo[lineIdx].wordCount;
                    continue;
                }
                
                var lineInfo = animator.textInfo.lineInfo[lineIdx];
                for (int i = wordIdx; i < lineInfo.wordCount; i++)
                {
                    seq.Join(animator.DOShakeWordOffset(i, strength, duration, ease, vibrato, randomness, fadeOut));
                }
                break;
            }

            return seq;
        }
        
        public static Sequence DOShakeLineRotation(this DOTweenTMPAnimator animator, int lineIndex, Vector3 strength, float duration, Ease ease = Ease.OutQuad, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
        {
            var seq = DOTween.Sequence();
            
            for (int lineIdx = 0, wordIdx = 0; lineIdx <= lineIndex && 
                                               lineIdx < animator.textInfo.lineCount; lineIdx++)
            {
                if (lineIdx < lineIndex)
                {
                    wordIdx += animator.textInfo.lineInfo[lineIdx].wordCount;
                    continue;
                }
                
                var lineInfo = animator.textInfo.lineInfo[lineIdx];
                for (int i = wordIdx; i < lineInfo.wordCount; i++)
                {
                    seq.Join(animator.DOShakeWordRotation(i, strength, duration, ease, vibrato, randomness, fadeOut));
                }
                break;
            }         
            
            return seq;
        }
        
        public static Sequence DOShakeLineScale(this DOTweenTMPAnimator animator, int lineIndex, Vector3 strength, float duration, Ease ease = Ease.OutQuad, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
        {
            var seq = DOTween.Sequence();
            
            for (int lineIdx = 0, wordIdx = 0; lineIdx <= lineIndex && 
                                               lineIdx < animator.textInfo.lineCount; lineIdx++)
            {
                if (lineIdx < lineIndex)
                {
                    wordIdx += animator.textInfo.lineInfo[lineIdx].wordCount;
                    continue;
                }

                var lineInfo = animator.textInfo.lineInfo[lineIdx];
                for (int i = wordIdx; i < lineInfo.wordCount; i++)
                {
                    seq.Join(animator.DOShakeWordScale(i,  strength, duration, ease, vibrato, randomness, fadeOut));
                }
                break;
            }          
            
            return seq;
        }

        #endregion

        #region Text Sequence Tweens

        #region Character Tweens
        
        public static Sequence DOFadeCharacters(this DOTweenTMPAnimator animator, float endValue, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int i = 0; i < animator.textInfo.characterCount; i++)
            {
                seq.Insert(i * seqInterval,animator.DOFadeChar(i, endValue, easeDuration).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOColorCharacters(this DOTweenTMPAnimator animator, Color endValue, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int i = 0; i < animator.textInfo.characterCount; i++)
            {
                seq.Insert(i * seqInterval,animator.DOColorChar(i, endValue, easeDuration).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOOffsetCharacters(this DOTweenTMPAnimator animator, Vector3 endValue, float easeDuration, float sequenceSpeed,  Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int i = 0; i < animator.textInfo.characterCount; i++)
            {
                seq.Insert(i * seqInterval,animator.DOOffsetChar(i, endValue, easeDuration).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DORotateCharacters(this DOTweenTMPAnimator animator, Vector3 endValue, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad, RotateMode mode = RotateMode.Fast)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int i = 0; i < animator.textInfo.characterCount; i++)
            {
                seq.Insert(i * seqInterval,animator.DORotateChar(i, endValue, easeDuration, mode).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOScaleCharacters(this DOTweenTMPAnimator animator, float endValue, float easeDuration, float sequenceSpeed,  Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int i = 0; i < animator.textInfo.characterCount; i++)
            {
                seq.Insert(i * seqInterval,animator.DOScaleChar(i, endValue, easeDuration).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOPunchCharactersOffset(this DOTweenTMPAnimator animator, Vector3 punch, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad, int vibrato = 10, float elasticity = 1f)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int i = 0; i < animator.textInfo.characterCount; i++)
            {
                seq.Insert(i * seqInterval,animator.DOPunchCharOffset(i, punch, easeDuration, vibrato, elasticity).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOPunchCharactersRotation(this DOTweenTMPAnimator animator, Vector3 punch, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad, int vibrato = 10, float elasticity = 1f)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int i = 0; i < animator.textInfo.characterCount; i++)
            {
                seq.Insert(i * seqInterval,animator.DOPunchCharRotation(i, punch, easeDuration, vibrato, elasticity).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOPunchCharactersScale(this DOTweenTMPAnimator animator, float punch, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad, int vibrato = 10, float elasticity = 1f)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int i = 0; i < animator.textInfo.characterCount; i++)
            {
                seq.Insert(i * seqInterval,animator.DOPunchCharScale(i, punch, easeDuration, vibrato, elasticity).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOShakeCharactersOffset(this DOTweenTMPAnimator animator, Vector3 strength, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int i = 0; i < animator.textInfo.characterCount; i++)
            {
                seq.Insert(i * seqInterval,animator.DOShakeCharOffset(i, easeDuration, strength, vibrato, randomness, fadeOut).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOShakeCharactersRotation(this DOTweenTMPAnimator animator, Vector3 strength, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int i = 0; i < animator.textInfo.characterCount; i++)
            {
                seq.Insert(i * seqInterval,animator.DOShakeCharRotation(i, easeDuration, strength, vibrato, randomness, fadeOut).SetEase(ease));
            }
            
            return seq;
        }
        
        public static Sequence DOShakeCharactersScale(this DOTweenTMPAnimator animator, Vector3 strength, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int i = 0; i < animator.textInfo.characterCount; i++)
            {
                seq.Insert(i * seqInterval,animator.DOShakeCharScale(i, easeDuration, strength, vibrato, randomness, fadeOut).SetEase(ease));
            }
            
            return seq;
        }
        
        #endregion

        #region Word Tweens
        
        public static Sequence DOFadeWords(this DOTweenTMPAnimator animator, float endValue, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int wordIdx = 0; wordIdx < animator.textInfo.wordCount; wordIdx++)
            {
                var wordInfo = animator.textInfo.wordInfo[wordIdx];
                var atPos = wordIdx * seqInterval;
                for (int charIdx = wordInfo.firstCharacterIndex; charIdx < wordInfo.characterCount; charIdx++)
                {
                    seq.Insert(atPos,animator.DOFadeChar(charIdx, endValue, easeDuration).SetEase(ease));
                }
            }
            
            return seq;
        }
        
        public static Sequence DOColorWords(this DOTweenTMPAnimator animator, Color endValue, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int wordIdx = 0; wordIdx < animator.textInfo.wordCount; wordIdx++)
            {
                var wordInfo = animator.textInfo.wordInfo[wordIdx];
                var atPos = wordIdx * seqInterval;
                for (int charIdx = wordInfo.firstCharacterIndex; charIdx < wordInfo.characterCount; charIdx++)
                {
                    seq.Insert(atPos, animator.DOColorChar(charIdx, endValue, easeDuration).SetEase(ease));
                }
            }
            
            return seq;
        }
        
        public static Sequence DOOffsetWords(this DOTweenTMPAnimator animator, Vector3 endValue, float easeDuration, float sequenceSpeed,  Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int wordIdx = 0; wordIdx < animator.textInfo.wordCount; wordIdx++)
            {
                var wordInfo = animator.textInfo.wordInfo[wordIdx];
                var atPos = wordIdx * seqInterval;
                for (int charIdx = wordInfo.firstCharacterIndex; charIdx < wordInfo.characterCount; charIdx++)
                {
                    seq.Insert(atPos, animator.DOOffsetChar(charIdx, endValue, easeDuration).SetEase(ease));
                }
            }
            
            return seq;
        }
        
        public static Sequence DORotateWords(this DOTweenTMPAnimator animator, Vector3 endValue, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad, RotateMode mode = RotateMode.Fast)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int wordIdx = 0; wordIdx < animator.textInfo.wordCount; wordIdx++)
            {
                var wordInfo = animator.textInfo.wordInfo[wordIdx];
                var atPos = wordIdx * seqInterval;
                for (int charIdx = wordInfo.firstCharacterIndex; charIdx < wordInfo.characterCount; charIdx++)
                {
                    seq.Insert(atPos, animator.DORotateChar(charIdx, endValue, easeDuration, mode).SetEase(ease));
                }
            }
            
            return seq;
        }
        
        public static Sequence DOScaleWords(this DOTweenTMPAnimator animator, float endValue, float easeDuration, float sequenceSpeed,  Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int wordIdx = 0; wordIdx < animator.textInfo.wordCount; wordIdx++)
            {
                var wordInfo = animator.textInfo.wordInfo[wordIdx];
                var atPos = wordIdx * seqInterval;
                for (int charIdx = wordInfo.firstCharacterIndex; charIdx < wordInfo.characterCount; charIdx++)
                {
                    seq.Insert(atPos, animator.DOScaleChar(charIdx, endValue, easeDuration).SetEase(ease));
                }
            }
            
            return seq;
        }
        
        public static Sequence DOPunchWordsOffset(this DOTweenTMPAnimator animator, Vector3 punch, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad, int vibrato = 10, float elasticity = 1f)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int wordIdx = 0; wordIdx < animator.textInfo.wordCount; wordIdx++)
            {
                var wordInfo = animator.textInfo.wordInfo[wordIdx];
                var atPos = wordIdx * seqInterval;
                for (int charIdx = wordInfo.firstCharacterIndex; charIdx < wordInfo.characterCount; charIdx++)
                {
                    seq.Insert(atPos,
                        animator.DOPunchCharOffset(charIdx, punch, easeDuration, vibrato, elasticity).SetEase(ease));
                }
            }
            
            return seq;
        }
        
        public static Sequence DOPunchWordsRotation(this DOTweenTMPAnimator animator, Vector3 punch, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad, int vibrato = 10, float elasticity = 1f)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int wordIdx = 0; wordIdx < animator.textInfo.wordCount; wordIdx++)
            {
                var wordInfo = animator.textInfo.wordInfo[wordIdx];
                var atPos = wordIdx * seqInterval;
                for (int charIdx = wordInfo.firstCharacterIndex; charIdx < wordInfo.characterCount; charIdx++)
                {
                    seq.Insert(atPos,
                        animator.DOPunchCharRotation(charIdx, punch, easeDuration, vibrato, elasticity).SetEase(ease));
                }
            }
            
            return seq;
        }
        
        public static Sequence DOPunchWordsScale(this DOTweenTMPAnimator animator, float punch, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad, int vibrato = 10, float elasticity = 1f)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int wordIdx = 0; wordIdx < animator.textInfo.wordCount; wordIdx++)
            {
                var wordInfo = animator.textInfo.wordInfo[wordIdx];
                var atPos = wordIdx * seqInterval;
                for (int charIdx = wordInfo.firstCharacterIndex; charIdx < wordInfo.characterCount; charIdx++)
                {
                    seq.Insert(atPos,
                        animator.DOPunchCharScale(charIdx, punch, easeDuration, vibrato, elasticity).SetEase(ease));
                }
            }
            
            return seq;
        }
        
        public static Sequence DOShakeWordsOffset(this DOTweenTMPAnimator animator, Vector3 strength, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int wordIdx = 0; wordIdx < animator.textInfo.wordCount; wordIdx++)
            {
                var wordInfo = animator.textInfo.wordInfo[wordIdx];
                var atPos = wordIdx * seqInterval;
                for (int charIdx = wordInfo.firstCharacterIndex; charIdx < wordInfo.characterCount; charIdx++)
                {
                    seq.Insert(atPos,
                        animator.DOShakeCharOffset(charIdx, easeDuration, strength, vibrato, randomness, fadeOut)
                            .SetEase(ease));
                }
            }
            
            return seq;
        }
        
        public static Sequence DOShakeWordsRotation(this DOTweenTMPAnimator animator, Vector3 strength, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int wordIdx = 0; wordIdx < animator.textInfo.wordCount; wordIdx++)
            {
                var wordInfo = animator.textInfo.wordInfo[wordIdx];
                var atPos = wordIdx * seqInterval;
                for (int charIdx = wordInfo.firstCharacterIndex; charIdx < wordInfo.characterCount; charIdx++)
                {
                    seq.Insert(atPos,
                        animator.DOShakeCharRotation(charIdx, easeDuration, strength, vibrato, randomness, fadeOut)
                            .SetEase(ease));
                }
            }
            
            return seq;
        }
        
        public static Sequence DOShakeWordsScale(this DOTweenTMPAnimator animator, Vector3 strength, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int wordIdx = 0; wordIdx < animator.textInfo.wordCount; wordIdx++)
            {
                var wordInfo = animator.textInfo.wordInfo[wordIdx];
                var atPos = wordIdx * seqInterval;
                for (int charIdx = wordInfo.firstCharacterIndex; charIdx < wordInfo.characterCount; charIdx++)
                {
                    seq.Insert(atPos,
                        animator.DOShakeCharScale(charIdx, easeDuration, strength, vibrato, randomness, fadeOut)
                            .SetEase(ease));
                }
            }
            
            return seq;
        }
        
        #endregion

        #region Line Tweens
        
        public static Sequence DOFadeLines(this DOTweenTMPAnimator animator, float endValue, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int lineIdx = 0; lineIdx < animator.textInfo.lineCount; lineIdx++)
            {
                var lineInfo = animator.textInfo.lineInfo[lineIdx];
                var atPos = lineIdx * seqInterval;
                for (int charIdx = lineInfo.firstCharacterIndex; charIdx <= lineInfo.lastCharacterIndex; charIdx++)
                {
                    seq.Insert(atPos,animator.DOFadeChar(charIdx, endValue, easeDuration).SetEase(ease));
                }
            }

            return seq;
        }
        
        public static Sequence DOColorLines(this DOTweenTMPAnimator animator, Color endValue, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int lineIdx = 0; lineIdx < animator.textInfo.lineCount; lineIdx++)
            {
                var lineInfo = animator.textInfo.lineInfo[lineIdx];
                var atPos = lineIdx * seqInterval;
                for (int charIdx = lineInfo.firstCharacterIndex; charIdx <= lineInfo.lastCharacterIndex; charIdx++)
                {
                    seq.Insert(atPos,animator.DOColorChar(charIdx, endValue, easeDuration).SetEase(ease));
                }
            }

            return seq;
        }
        
        public static Sequence DOOffsetLines(this DOTweenTMPAnimator animator, Vector3 endValue, float easeDuration, float sequenceSpeed,  Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int lineIdx = 0; lineIdx < animator.textInfo.lineCount; lineIdx++)
            {
                var lineInfo = animator.textInfo.lineInfo[lineIdx];
                var atPos = lineIdx * seqInterval;
                for (int charIdx = lineInfo.firstCharacterIndex; charIdx <= lineInfo.lastCharacterIndex; charIdx++)
                {
                    seq.Insert(atPos,animator.DOOffsetChar(charIdx, endValue, easeDuration).SetEase(ease));                }
            }

            return seq;
        }
        
        public static Sequence DORotateLines(this DOTweenTMPAnimator animator, Vector3 endValue, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad, RotateMode mode = RotateMode.Fast)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int lineIdx = 0; lineIdx < animator.textInfo.lineCount; lineIdx++)
            {
                var lineInfo = animator.textInfo.lineInfo[lineIdx];
                var atPos = lineIdx * seqInterval;
                for (int charIdx = lineInfo.firstCharacterIndex; charIdx <= lineInfo.lastCharacterIndex; charIdx++)
                {
                    seq.Insert(atPos,animator.DORotateChar(charIdx, endValue, easeDuration, mode).SetEase(ease));
                }
            }

            return seq;
        }
        
        public static Sequence DOScaleLines(this DOTweenTMPAnimator animator, float endValue, float easeDuration, float sequenceSpeed,  Ease ease = Ease.OutQuad)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int lineIdx = 0; lineIdx < animator.textInfo.lineCount; lineIdx++)
            {
                var lineInfo = animator.textInfo.lineInfo[lineIdx];
                var atPos = lineIdx * seqInterval;
                for (int charIdx = lineInfo.firstCharacterIndex; charIdx <= lineInfo.lastCharacterIndex; charIdx++)
                {
                    seq.Insert(atPos,animator.DOScaleChar(charIdx, endValue, easeDuration).SetEase(ease));                }
            }
            
            return seq;
        }
        
        public static Sequence DOPunchLinesOffset(this DOTweenTMPAnimator animator, Vector3 punch, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad, int vibrato = 10, float elasticity = 1f)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int lineIdx = 0; lineIdx < animator.textInfo.lineCount; lineIdx++)
            {
                var lineInfo = animator.textInfo.lineInfo[lineIdx];
                var atPos = lineIdx * seqInterval;
                for (int charIdx = lineInfo.firstCharacterIndex; charIdx <= lineInfo.lastCharacterIndex; charIdx++)
                {
                    seq.Insert(atPos,animator.DOPunchCharOffset(charIdx, punch, easeDuration, vibrato, elasticity).SetEase(ease));
                }
            }

            return seq;
        }
        
        public static Sequence DOPunchLinesRotation(this DOTweenTMPAnimator animator, Vector3 punch, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad, int vibrato = 10, float elasticity = 1f)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int lineIdx = 0; lineIdx < animator.textInfo.lineCount; lineIdx++)
            {
                var lineInfo = animator.textInfo.lineInfo[lineIdx];
                var atPos = lineIdx * seqInterval;
                for (int charIdx = lineInfo.firstCharacterIndex; charIdx <= lineInfo.lastCharacterIndex; charIdx++)
                {
                    seq.Insert(atPos,animator.DOPunchCharRotation(charIdx, punch, easeDuration, vibrato, elasticity).SetEase(ease));
                }
            }
       
            return seq;
        }
        
        public static Sequence DOPunchLinesScale(this DOTweenTMPAnimator animator, float punch, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad, int vibrato = 10, float elasticity = 1f)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int lineIdx = 0; lineIdx < animator.textInfo.lineCount; lineIdx++)
            {
                var lineInfo = animator.textInfo.lineInfo[lineIdx];
                var atPos = lineIdx * seqInterval;
                for (int charIdx = lineInfo.firstCharacterIndex; charIdx <= lineInfo.lastCharacterIndex; charIdx++)
                {
                    seq.Insert(atPos,animator.DOPunchCharScale(charIdx, punch, easeDuration, vibrato, elasticity).SetEase(ease));
                }
            }

            return seq;
        }
        
        public static Sequence DOShakeLinesOffset(this DOTweenTMPAnimator animator, Vector3 strength, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int lineIdx = 0; lineIdx < animator.textInfo.lineCount; lineIdx++)
            {
                var lineInfo = animator.textInfo.lineInfo[lineIdx];
                var atPos = lineIdx * seqInterval;
                for (int charIdx = lineInfo.firstCharacterIndex; charIdx <= lineInfo.lastCharacterIndex; charIdx++)
                {
                    seq.Insert(atPos,animator.DOShakeCharOffset(charIdx, easeDuration, strength, vibrato, randomness, fadeOut).SetEase(ease));
                }
            }

            return seq;
        }
        
        public static Sequence DOShakeLinesRotation(this DOTweenTMPAnimator animator, Vector3 strength, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int lineIdx = 0; lineIdx < animator.textInfo.lineCount; lineIdx++)
            {
                var lineInfo = animator.textInfo.lineInfo[lineIdx];
                var atPos = lineIdx * seqInterval;
                for (int charIdx = lineInfo.firstCharacterIndex; charIdx <= lineInfo.lastCharacterIndex; charIdx++)
                {
                    seq.Insert(atPos,animator.DOShakeCharRotation(charIdx, easeDuration, strength, vibrato, randomness, fadeOut).SetEase(ease));
                }
            }
        
            return seq;
        }
        
        public static Sequence DOShakeLinesScale(this DOTweenTMPAnimator animator, Vector3 strength, float easeDuration, float sequenceSpeed, Ease ease = Ease.OutQuad, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: 0;
            for (int lineIdx = 0; lineIdx < animator.textInfo.lineCount; lineIdx++)
            {
                var lineInfo = animator.textInfo.lineInfo[lineIdx];
                var atPos = lineIdx * seqInterval;
                for (int charIdx = lineInfo.firstCharacterIndex; charIdx <= lineInfo.lastCharacterIndex; charIdx++)
                {
                    seq.Insert(atPos,animator.DOShakeCharScale(charIdx, easeDuration, strength, vibrato, randomness, fadeOut).SetEase(ease));
                }
            }

            return seq;
        }
        
        #endregion

        #endregion
    
        
        
    }
}