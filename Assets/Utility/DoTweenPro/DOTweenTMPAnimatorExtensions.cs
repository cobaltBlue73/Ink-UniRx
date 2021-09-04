using System;
using DG.Tweening;
using UnityEngine;
using Utility.TextMeshPro;

namespace Utility.DOTweenPro
{
    public static class DOTweenTMPAnimatorExtensions
    {
        #region Word Tweens

        private static Sequence GetWordSeq(DOTweenTMPAnimator animator, int wordIndex, Ease ease, bool from, Func<int, Tweener> tweenFunc)
        {
            var seq = DOTween.Sequence();
            var wordInfo = animator.textInfo.wordInfo[wordIndex];
            for (int i = wordInfo.firstCharacterIndex; i <= wordInfo.lastCharacterIndex; ++i)
            {
                var tweener = tweenFunc(i);
                if(tweener == null) continue;
                
                tweener.SetEase(ease);
                if (from) tweener.From();
                seq.Join(tweener);
            }
            
            return seq;
        }

        public static Sequence DOFadeWord(this DOTweenTMPAnimator animator, int wordIndex, float endValue, float duration, Ease ease = Ease.OutQuad, bool from = false) => 
            GetWordSeq(animator, wordIndex, ease, @from,i=> animator.DOFadeChar(i, endValue, duration));

        public static Sequence DOColorWord(this DOTweenTMPAnimator animator, int wordIndex, Color endValue, float duration, Ease ease = Ease.OutQuad, bool from = false) => 
            GetWordSeq(animator, wordIndex, ease, @from,i=> animator.DOColorChar(i, endValue, duration));

        public static Sequence DOOffsetWord(this DOTweenTMPAnimator animator, int wordIndex, Vector3 endValue, float duration, Ease ease = Ease.OutQuad, bool from = false) => 
            GetWordSeq(animator, wordIndex, ease, @from,i=> animator.DOOffsetChar(i, endValue, duration));

        public static Sequence DORotateWord(this DOTweenTMPAnimator animator, int wordIndex, Vector3 endValue, float duration, Ease ease = Ease.OutQuad, bool from = false, RotateMode mode = RotateMode.Fast) => 
            GetWordSeq(animator, wordIndex, ease, @from,i=> animator.DORotateChar(i, endValue, duration, mode));

        public static Sequence DOScaleWord(this DOTweenTMPAnimator animator, int wordIndex, float endValue, float duration, Ease ease = Ease.OutQuad, bool from = false) => 
            GetWordSeq(animator, wordIndex, ease, @from,i=> animator.DOScaleChar(i, endValue, duration));

        public static Sequence DOPunchWordOffset(this DOTweenTMPAnimator animator, int wordIndex, Vector3 punch, float duration, Ease ease = Ease.OutQuad, bool from = false, int vibrato = 10, float elasticity = 1f) => 
            GetWordSeq(animator, wordIndex, ease, @from,i=> animator.DOPunchCharOffset(i, punch, duration, vibrato, elasticity));

        public static Sequence DOPunchWordRotation(this DOTweenTMPAnimator animator, int wordIndex, Vector3 punch, float duration, Ease ease = Ease.OutQuad, bool from = false, int vibrato = 10, float elasticity = 1f) => 
            GetWordSeq(animator, wordIndex, ease, @from,i=> animator.DOPunchCharRotation(i, punch, duration, vibrato, elasticity));

        public static Sequence DOPunchWordScale(this DOTweenTMPAnimator animator, int wordIndex, float punch, float duration, Ease ease = Ease.OutQuad, bool from = false, int vibrato = 10, float elasticity = 1f) => 
            GetWordSeq(animator, wordIndex, ease, @from,i=> animator.DOPunchCharScale(i, punch, duration, vibrato, elasticity));

        public static Sequence DOShakeWordOffset(this DOTweenTMPAnimator animator, int wordIndex, Vector3 strength, float duration, Ease ease = Ease.OutQuad, bool from = false, int vibrato = 10, float randomness = 90f, bool fadeOut = true) => 
            GetWordSeq(animator, wordIndex, ease, @from,i=> animator.DOShakeCharOffset(i, duration, strength, vibrato, randomness, fadeOut));

        public static Sequence DOShakeWordRotation(this DOTweenTMPAnimator animator, int wordIndex, Vector3 strength, float duration, Ease ease = Ease.OutQuad, bool from = false, int vibrato = 10, float randomness = 90f, bool fadeOut = true) => 
            GetWordSeq(animator, wordIndex, ease, @from,i=> animator.DOShakeCharRotation(i, duration, strength, vibrato, randomness, fadeOut));

        public static Sequence DOShakeWordScale(this DOTweenTMPAnimator animator, int wordIndex, Vector3 strength, float duration, Ease ease = Ease.OutQuad, bool from = false, int vibrato = 10, float randomness = 90f, bool fadeOut = true) => 
            GetWordSeq(animator, wordIndex, ease, @from,i=> animator.DOShakeCharScale(i, duration, strength, vibrato, randomness, fadeOut));

        #endregion

        #region Line Tweens
        
        private static Sequence GetLineSeq(DOTweenTMPAnimator animator, int lineIndex, Ease ease, bool from, Func<int, Tweener> tweenFunc)
        {
            var seq = DOTween.Sequence();
            var lineInfo = animator.textInfo.lineInfo[lineIndex];
            for (int i = lineInfo.firstCharacterIndex; i <= lineInfo.lastCharacterIndex; ++i)
            {
                var tweener = tweenFunc(i);
                if(tweener == null) continue;
                
                tweener.SetEase(ease);
                if (from) tweener.From();
                seq.Join(tweener);
            }
            
            return seq;
        }
        
        public static Sequence DOFadeLine(this DOTweenTMPAnimator animator, int lineIndex, float endValue, float duration, Ease ease = Ease.OutQuad, bool from = false) => 
            GetLineSeq(animator, lineIndex, ease, @from, i=> animator.DOFadeChar(i, endValue, duration));

        public static Sequence DOColorLine(this DOTweenTMPAnimator animator, int lineIndex, Color endValue, float duration, Ease ease = Ease.OutQuad, bool from = false) => 
            GetLineSeq(animator, lineIndex, ease, @from, i=> animator.DOColorChar(i, endValue, duration));

        public static Sequence DOOffsetLine(this DOTweenTMPAnimator animator, int lineIndex, Vector3 endValue, float duration, Ease ease = Ease.OutQuad, bool from = false) => 
            GetLineSeq(animator, lineIndex, ease, @from, i=> animator.DOOffsetChar(i, endValue, duration));

        public static Sequence DORotateLine(this DOTweenTMPAnimator animator, int lineIndex, Vector3 endValue, float duration, Ease ease = Ease.OutQuad, bool from = false, RotateMode mode = RotateMode.Fast) => 
            GetLineSeq(animator, lineIndex, ease, @from, i=> animator.DORotateChar(i, endValue, duration, mode));

        public static Sequence DOScaleLine(this DOTweenTMPAnimator animator, int lineIndex, float endValue, float duration, Ease ease = Ease.OutQuad, bool from = false) => 
            GetLineSeq(animator, lineIndex, ease, @from, i=> animator.DOScaleChar(i, endValue, duration));

        public static Sequence DOPunchLineOffset(this DOTweenTMPAnimator animator, int lineIndex, Vector3 punch, float duration, Ease ease = Ease.OutQuad, bool from = false, int vibrato = 10, float elasticity = 1f) => 
            GetLineSeq(animator, lineIndex, ease, @from, i=> animator.DOPunchCharOffset(i, punch, duration, vibrato, elasticity));

        public static Sequence DOPunchLineRotation(this DOTweenTMPAnimator animator, int lineIndex, Vector3 punch, float duration, Ease ease = Ease.OutQuad, bool from = false, int vibrato = 10, float elasticity = 1f) => 
            GetLineSeq(animator, lineIndex, ease, @from, i=> animator.DOPunchCharRotation(i, punch, duration, vibrato, elasticity));

        public static Sequence DOPunchLineScale(this DOTweenTMPAnimator animator, int lineIndex, float punch, float duration, Ease ease = Ease.OutQuad, bool from = false, int vibrato = 10, float elasticity = 1f) => 
            GetLineSeq(animator, lineIndex, ease, @from, i=> animator.DOPunchCharScale(i, punch, duration, vibrato, elasticity));

        public static Sequence DOShakeLineOffset(this DOTweenTMPAnimator animator, int lineIndex, Vector3 strength, float duration, Ease ease = Ease.OutQuad, bool from = false, int vibrato = 10, float randomness = 90f, bool fadeOut = true) => 
            GetLineSeq(animator, lineIndex, ease, @from, i=> animator.DOShakeCharOffset(i, duration, strength, vibrato, randomness, fadeOut));

        public static Sequence DOShakeLineRotation(this DOTweenTMPAnimator animator, int lineIndex, Vector3 strength, float duration, Ease ease = Ease.OutQuad, bool from = false, int vibrato = 10, float randomness = 90f, bool fadeOut = true) => 
            GetLineSeq(animator, lineIndex, ease, @from, i=> animator.DOShakeCharRotation(i, duration, strength, vibrato, randomness, fadeOut));

        public static Sequence DOShakeLineScale(this DOTweenTMPAnimator animator, int lineIndex, Vector3 strength, float duration, Ease ease = Ease.OutQuad, bool from = false, int vibrato = 10, float randomness = 90f, bool fadeOut = true) => 
            GetLineSeq(animator, lineIndex, ease, @from, i=> animator.DOShakeCharScale(i, duration, strength, vibrato, randomness, fadeOut));

        #endregion

        #region Link Tweens
        
        private static Sequence GetLinkSeq(DOTweenTMPAnimator animator, int linkIndex, Ease ease, bool from, Func<int, Tweener> tweenFunc)
        {
            var seq = DOTween.Sequence();
            var linkInfo = animator.textInfo.linkInfo[linkIndex];
            for (int i = linkInfo.linkTextfirstCharacterIndex; i < linkInfo.linkTextLength; ++i)
            {
                var charIdx = linkInfo.linkTextfirstCharacterIndex + i;
                var tweener = tweenFunc(charIdx);
                if(tweener == null) continue;
                
                tweener.SetEase(ease);
                if (from) tweener.From();
                seq.Join(tweener);
            }
            
            return seq;
        }
        
        public static Sequence DOFadeLink(this DOTweenTMPAnimator animator, int linkIndex, float endValue, float duration, Ease ease = Ease.OutQuad, bool from = false) => 
            GetLinkSeq(animator, linkIndex, ease, @from, i=> animator.DOFadeChar(i, endValue, duration));

        public static Sequence DOColorLink(this DOTweenTMPAnimator animator, int linkIndex, Color endValue, float duration, Ease ease = Ease.OutQuad, bool from = false) => 
            GetLinkSeq(animator, linkIndex, ease, @from, i=> animator.DOColorChar(i, endValue, duration));

        public static Sequence DOOffsetLink(this DOTweenTMPAnimator animator, int linkIndex, Vector3 endValue, float duration, Ease ease = Ease.OutQuad, bool from = false) => 
            GetLinkSeq(animator, linkIndex, ease, @from, i=> animator.DOOffsetChar(i, endValue, duration));

        public static Sequence DORotateLink(this DOTweenTMPAnimator animator, int linkIndex, Vector3 endValue, float duration, Ease ease = Ease.OutQuad, bool from = false, RotateMode mode = RotateMode.Fast) => 
            GetLinkSeq(animator, linkIndex, ease, @from, i=> animator.DORotateChar(i, endValue, duration, mode));

        public static Sequence DOScaleLink(this DOTweenTMPAnimator animator, int linkIndex, float endValue, float duration, Ease ease = Ease.OutQuad, bool from = false) => 
            GetLinkSeq(animator, linkIndex, ease, @from, i=> animator.DOScaleChar(i, endValue, duration));

        public static Sequence DOPunchLinkOffset(this DOTweenTMPAnimator animator, int linkIndex, Vector3 punch, float duration, Ease ease = Ease.OutQuad, bool from = false, int vibrato = 10, float elasticity = 1f) => 
            GetLinkSeq(animator, linkIndex, ease, @from, i=> animator.DOPunchCharOffset(i, punch, duration, vibrato, elasticity));

        public static Sequence DOPunchLinkRotation(this DOTweenTMPAnimator animator, int linkIndex, Vector3 punch, float duration, Ease ease = Ease.OutQuad, bool from = false, int vibrato = 10, float elasticity = 1f) => 
            GetLinkSeq(animator, linkIndex, ease, @from, i=> animator.DOPunchCharRotation(i, punch, duration, vibrato, elasticity));

        public static Sequence DOPunchLinkScale(this DOTweenTMPAnimator animator, int linkIndex, float punch, float duration, Ease ease = Ease.OutQuad, bool from = false, int vibrato = 10, float elasticity = 1f) => 
            GetLinkSeq(animator, linkIndex, ease, @from, i=> animator.DOPunchCharScale(i, punch, duration, vibrato, elasticity));

        public static Sequence DOShakeLinkOffset(this DOTweenTMPAnimator animator, int linkIndex, Vector3 strength, float duration, Ease ease = Ease.OutQuad, bool from = false, int vibrato = 10, float randomness = 90f, bool fadeOut = true) => 
            GetLinkSeq(animator, linkIndex, ease, @from, i=> animator.DOShakeCharOffset(i, duration, strength, vibrato, randomness, fadeOut));

        public static Sequence DOShakeLinkRotation(this DOTweenTMPAnimator animator, int linkIndex, Vector3 strength, float duration, Ease ease = Ease.OutQuad, bool from = false, int vibrato = 10, float randomness = 90f, bool fadeOut = true) => 
            GetLinkSeq(animator, linkIndex, ease, @from, i=> animator.DOShakeCharRotation(i, duration, strength, vibrato, randomness, fadeOut));

        public static Sequence DOShakeLinkScale(this DOTweenTMPAnimator animator, int linkIndex, Vector3 strength, float duration, Ease ease = Ease.OutQuad, bool from = false, int vibrato = 10, float randomness = 90f, bool fadeOut = true) => 
            GetLinkSeq(animator, linkIndex, ease, @from, i=> animator.DOShakeCharScale(i, duration, strength, vibrato, randomness, fadeOut));

        #endregion
        
        #region Text Sequence Tweens

        #region Character Tweens
        
        private static Sequence GetCharactersSeq(DOTweenTMPAnimator animator, Ease ease, bool from, float easeDuration, float sequenceSpeed, Func<int, Tweener> tweenFunc)
        {
            var seq = DOTween.Sequence();
            animator.target.ForceMeshUpdate();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: easeDuration;
            for (int i = 0; i < animator.textInfo.characterCount; ++i)
            {
                var tweener = tweenFunc(i);
                if (tweener == null) continue;

                tweener.SetEase(ease);
                if (from) tweener.From();
                seq.Insert(i * seqInterval, tweener);
            }
            
            return seq;
        }
        
        public static Sequence DOFadeCharacters(this DOTweenTMPAnimator animator, float endValue, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0) => 
            GetCharactersSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOFadeChar(i, endValue, easeDuration));

        public static Sequence DOColorCharacters(this DOTweenTMPAnimator animator, Color endValue, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0) => 
            GetCharactersSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOColorChar(i, endValue, easeDuration));

        public static Sequence DOOffsetCharacters(this DOTweenTMPAnimator animator, Vector3 endValue, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0) => 
            GetCharactersSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOOffsetChar(i, endValue, easeDuration));

        public static Sequence DORotateCharacters(this DOTweenTMPAnimator animator, Vector3 endValue, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0, RotateMode mode = RotateMode.Fast) => 
            GetCharactersSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DORotateChar(i, endValue, easeDuration, mode));

        public static Sequence DOScaleCharacters(this DOTweenTMPAnimator animator, float endValue, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0) => 
            GetCharactersSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOScaleChar(i, endValue, easeDuration));

        public static Sequence DOPunchCharactersOffset(this DOTweenTMPAnimator animator, Vector3 punch, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0, int vibrato = 10, float elasticity = 1f) => 
            GetCharactersSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOPunchCharOffset(i, punch, easeDuration, vibrato, elasticity));

        public static Sequence DOPunchCharactersRotation(this DOTweenTMPAnimator animator, Vector3 punch, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0, int vibrato = 10, float elasticity = 1f) => 
            GetCharactersSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOPunchCharRotation(i, punch, easeDuration, vibrato, elasticity));

        public static Sequence DOPunchCharactersScale(this DOTweenTMPAnimator animator, float punch, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0, int vibrato = 10, float elasticity = 1f) => 
            GetCharactersSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOPunchCharScale(i, punch, easeDuration, vibrato, elasticity));

        public static Sequence DOShakeCharactersOffset(this DOTweenTMPAnimator animator, Vector3 strength, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0, int vibrato = 10, float randomness = 90f, bool fadeOut = true) => 
            GetCharactersSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOShakeCharOffset(i, easeDuration, strength, vibrato, randomness, fadeOut));

        public static Sequence DOShakeCharactersRotation(this DOTweenTMPAnimator animator, Vector3 strength, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0, int vibrato = 10, float randomness = 90f, bool fadeOut = true) => 
            GetCharactersSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOShakeCharRotation(i, easeDuration, strength, vibrato, randomness, fadeOut));

        public static Sequence DOShakeCharactersScale(this DOTweenTMPAnimator animator, Vector3 strength, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0, int vibrato = 10, float randomness = 90f, bool fadeOut = true) => 
            GetCharactersSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOShakeCharScale(i, easeDuration, strength, vibrato, randomness, fadeOut));

        #endregion

        #region Word Tweens
        
        private static Sequence GetWordsSeq(DOTweenTMPAnimator animator, Ease ease, bool from, float easeDuration, float sequenceSpeed, Func<int, Tweener> tweenFunc)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: easeDuration;
            for (int wordIdx = 0; wordIdx < animator.textInfo.wordCount; ++wordIdx)
            {
                var wordInfo = animator.textInfo.wordInfo[wordIdx];
                var atPos = wordIdx * seqInterval;
                Tweener tweener;
                for (int charIdx = wordInfo.firstCharacterIndex; charIdx <= wordInfo.lastCharacterIndex; ++charIdx)
                {
                    tweener = tweenFunc(charIdx);
                    if (tweener == null) continue;

                    tweener.SetEase(ease);
                    if (from) tweener.From();
                    seq.Insert(atPos, tweener);
                }
                
                if (wordInfo.lastCharacterIndex + 1 >= animator.textInfo.characterCount) continue;

                var charInfo = animator.textInfo.characterInfo[wordInfo.lastCharacterIndex + 1];
                if(!charInfo.isVisible || charInfo.IsLetterOrDigit()) continue;
                
                tweener = tweenFunc(wordInfo.lastCharacterIndex + 1);
                if (tweener == null) continue;

                tweener.SetEase(ease);
                if (from) tweener.From();
                seq.Insert(atPos, tweener);
            }
            
            return seq;
        }
        
        public static Sequence DOFadeWords(this DOTweenTMPAnimator animator, float endValue, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0) => 
            GetWordsSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOFadeChar(i, endValue, easeDuration));

        public static Sequence DOColorWords(this DOTweenTMPAnimator animator, Color endValue, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0) => 
            GetWordsSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOColorChar(i, endValue, easeDuration));

        public static Sequence DOOffsetWords(this DOTweenTMPAnimator animator, Vector3 endValue, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0) => 
            GetWordsSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOOffsetChar(i, endValue, easeDuration));

        public static Sequence DORotateWords(this DOTweenTMPAnimator animator, Vector3 endValue, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0, RotateMode mode = RotateMode.Fast) => 
            GetWordsSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DORotateChar(i, endValue, easeDuration, mode));

        public static Sequence DOScaleWords(this DOTweenTMPAnimator animator, float endValue, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0) => 
            GetWordsSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOScaleChar(i, endValue, easeDuration));

        public static Sequence DOPunchWordsOffset(this DOTweenTMPAnimator animator, Vector3 punch, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0, int vibrato = 10, float elasticity = 1f) => 
            GetWordsSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOPunchCharOffset(i, punch, easeDuration, vibrato, elasticity));

        public static Sequence DOPunchWordsRotation(this DOTweenTMPAnimator animator, Vector3 punch, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0, int vibrato = 10, float elasticity = 1f) => 
            GetWordsSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOPunchCharRotation(i, punch, easeDuration, vibrato, elasticity));

        public static Sequence DOPunchWordsScale(this DOTweenTMPAnimator animator, float punch, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0, int vibrato = 10, float elasticity = 1f) => 
            GetWordsSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOPunchCharScale(i, punch, easeDuration, vibrato, elasticity));

        public static Sequence DOShakeWordsOffset(this DOTweenTMPAnimator animator, Vector3 strength, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0, int vibrato = 10, float randomness = 90f, bool fadeOut = true) => 
            GetWordsSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOShakeCharOffset(i, easeDuration, strength, vibrato, randomness, fadeOut));

        public static Sequence DOShakeWordsRotation(this DOTweenTMPAnimator animator, Vector3 strength, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0, int vibrato = 10, float randomness = 90f, bool fadeOut = true) => 
            GetWordsSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOShakeCharRotation(i, easeDuration, strength, vibrato, randomness, fadeOut));

        public static Sequence DOShakeWordsScale(this DOTweenTMPAnimator animator, Vector3 strength, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0, int vibrato = 10, float randomness = 90f, bool fadeOut = true) => 
            GetWordsSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOShakeCharScale(i, easeDuration, strength, vibrato, randomness, fadeOut));

        #endregion

        #region Line Tweens
        
        private static Sequence GetLinesSeq(DOTweenTMPAnimator animator, Ease ease, bool from, float easeDuration, float sequenceSpeed, Func<int, Tweener> tweenFunc)
        {
            var seq = DOTween.Sequence();
            var seqInterval = sequenceSpeed > 0? 1 / sequenceSpeed: easeDuration;
            for (int lineIdx = 0; lineIdx < animator.textInfo.lineCount; ++lineIdx)
            {
                var lineInfo = animator.textInfo.lineInfo[lineIdx];
                var atPos = lineIdx * seqInterval;
                for (int charIdx = lineInfo.firstCharacterIndex; charIdx <= lineInfo.lastCharacterIndex; ++charIdx)
                {
                    var tweener = tweenFunc(charIdx);
                    if (tweener == null) continue;

                    tweener.SetEase(ease);
                    if (from) tweener.From();
                    seq.Insert(atPos, tweener);
                }
            }
            
            return seq;
        }
        
        public static Sequence DOFadeLines(this DOTweenTMPAnimator animator, float endValue, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0) => 
            GetLinesSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOFadeChar(i, endValue, easeDuration));

        public static Sequence DOColorLines(this DOTweenTMPAnimator animator, Color endValue, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0) => 
            GetLinesSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=>animator.DOColorChar(i, endValue, easeDuration));

        public static Sequence DOOffsetLines(this DOTweenTMPAnimator animator, Vector3 endValue, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0) => 
            GetLinesSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOOffsetChar(i, endValue, easeDuration));

        public static Sequence DORotateLines(this DOTweenTMPAnimator animator, Vector3 endValue, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0, RotateMode mode = RotateMode.Fast) => 
            GetLinesSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DORotateChar(i, endValue, easeDuration, mode));

        public static Sequence DOScaleLines(this DOTweenTMPAnimator animator, float endValue, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0) => 
            GetLinesSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOScaleChar(i, endValue, easeDuration));

        public static Sequence DOPunchLinesOffset(this DOTweenTMPAnimator animator, Vector3 punch, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0, int vibrato = 10, float elasticity = 1f) => 
            GetLinesSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOPunchCharOffset(i, punch, easeDuration, vibrato, elasticity));

        public static Sequence DOPunchLinesRotation(this DOTweenTMPAnimator animator, Vector3 punch, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0, int vibrato = 10, float elasticity = 1f) => 
            GetLinesSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOPunchCharRotation(i, punch, easeDuration, vibrato, elasticity));

        public static Sequence DOPunchLinesScale(this DOTweenTMPAnimator animator, float punch, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0, int vibrato = 10, float elasticity = 1f) => 
            GetLinesSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=>animator.DOPunchCharScale(i, punch, easeDuration, vibrato, elasticity));

        public static Sequence DOShakeLinesOffset(this DOTweenTMPAnimator animator, Vector3 strength, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0, int vibrato = 10, float randomness = 90f, bool fadeOut = true) => 
            GetLinesSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=>animator.DOShakeCharOffset(i, easeDuration, strength, vibrato, randomness, fadeOut));

        public static Sequence DOShakeLinesRotation(this DOTweenTMPAnimator animator, Vector3 strength, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0, int vibrato = 10, float randomness = 90f, bool fadeOut = true) => 
            GetLinesSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOShakeCharRotation(i, easeDuration, strength, vibrato, randomness, fadeOut));

        public static Sequence DOShakeLinesScale(this DOTweenTMPAnimator animator, Vector3 strength, float easeDuration, Ease ease = Ease.OutQuad, bool from = false, float sequenceSpeed = 0, int vibrato = 10, float randomness = 90f, bool fadeOut = true) => 
            GetLinesSeq(animator, ease, @from, easeDuration, sequenceSpeed, i=> animator.DOShakeCharScale(i, easeDuration, strength, vibrato, randomness, fadeOut));

        #endregion

        #endregion
        
    }
}