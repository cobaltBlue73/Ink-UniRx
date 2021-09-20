using System;
using TMPro;
using UniRx;
using Utility.General;

namespace Utility.TextMeshPro
{
    public static class TextInfoExtensions
    {
        public static void GetFirstAndLastWordIndexFromCharacterIndexRange(this TMP_TextInfo textInfo, int fromCharIndex,  int toCharIndex, out int firstWordIndex, out int lastWordIndex)
        {
            for (firstWordIndex = 0; firstWordIndex < textInfo.wordCount; ++firstWordIndex)
            {
                var wordInfo = textInfo.wordInfo[firstWordIndex];
                
                if(wordInfo.firstCharacterIndex >= fromCharIndex) break;
            }
            
            for (lastWordIndex = firstWordIndex; lastWordIndex < textInfo.wordCount; ++lastWordIndex)
            {
                var wordInfo = textInfo.wordInfo[lastWordIndex];
                
                if(wordInfo.lastCharacterIndex >= toCharIndex) break;
            }
        }
        
        public static void GetFirstAndLastLineIndexFromCharacterIndexRange(this TMP_TextInfo textInfo, int fromCharIndex,  int toCharIndex, out int firstLineIndex, out int lastLineIndex)
        {
            for (firstLineIndex = 0; firstLineIndex < textInfo.lineCount; ++firstLineIndex)
            {
                var lineInfo = textInfo.lineInfo[firstLineIndex];
                
                if(lineInfo.firstCharacterIndex >= fromCharIndex) break;
            }
            
            for (lastLineIndex = firstLineIndex; lastLineIndex < textInfo.lineCount; ++lastLineIndex)
            {
                var lineInfo = textInfo.wordInfo[lastLineIndex];
                
                if(lineInfo.lastCharacterIndex >= toCharIndex) break;
            }
        }
        
        
        public static void GetFirstAndLastWordIndexOnLine(this TMP_TextInfo textInfo, int lineIndex, out int firstWordIndex, out int lastWordIndex)
        {
            var lineInfo = textInfo.lineInfo[lineIndex];
            
            for (firstWordIndex = 0; firstWordIndex < textInfo.wordCount; ++firstWordIndex)
            {
                var wordInfo = textInfo.wordInfo[firstWordIndex];
                
                if(wordInfo.firstCharacterIndex >= lineInfo.firstCharacterIndex) break;
            }
            
            for (lastWordIndex = firstWordIndex; lastWordIndex < textInfo.wordCount; ++lastWordIndex)
            {
                var wordInfo = textInfo.wordInfo[lastWordIndex];
                
                if(wordInfo.firstCharacterIndex > lineInfo.lastCharacterIndex) break;
            }

            --lastWordIndex;
        }
        
        public static void GetFirstAndLastWordIndexOnPage(this TMP_TextInfo textInfo, int pageIndex, out int firstWordIndex, out int lastWordIndex)
        {
            var pageInfo = textInfo.pageInfo[pageIndex];
            
            for (firstWordIndex = 0; firstWordIndex < textInfo.wordCount; ++firstWordIndex)
            {
                var wordInfo = textInfo.wordInfo[firstWordIndex];
                
                if(wordInfo.firstCharacterIndex >= pageInfo.firstCharacterIndex) break;
            }
            
            for (lastWordIndex = firstWordIndex; lastWordIndex < textInfo.wordCount; ++lastWordIndex)
            {
                var wordInfo = textInfo.wordInfo[lastWordIndex];
                
                if(wordInfo.firstCharacterIndex > pageInfo.lastCharacterIndex) break;
            }

            --lastWordIndex;
        }
        
        public static void GetFirstAndLastLineIndexOnPage(this TMP_TextInfo textInfo, int pageIndex, out int firstLineIndex, out int lastLineIndex)
        {
            var pageInfo = textInfo.pageInfo[pageIndex];
            
            for (firstLineIndex = 0; firstLineIndex < textInfo.lineCount; ++firstLineIndex)
            {
                var lineInfo = textInfo.lineInfo[firstLineIndex];
                
                if(lineInfo.firstCharacterIndex >= pageInfo.firstCharacterIndex) break;
            }
            
            for (lastLineIndex = firstLineIndex; lastLineIndex < textInfo.lineCount; ++lastLineIndex)
            {
                var lineInfo = textInfo.lineInfo[lastLineIndex];
                
                if(lineInfo.firstCharacterIndex > pageInfo.lastCharacterIndex) break;
            }

            --lastLineIndex;
        }

        public static IObservable<TMP_CharacterInfo> OnPointerEnterCharacterAsObservable(this TMP_TextInfo info)
        {
            var trigger = info.textComponent.GetOrAddComponent<TMPCharacterEventTrigger>();
            return trigger.OnPointerEnter.AsObservable();
        }
        
        public static IObservable<TMP_CharacterInfo> OnPointerExitCharacterAsObservable(this TMP_TextInfo info)
        {
            var trigger = info.textComponent.GetOrAddComponent<TMPCharacterEventTrigger>();
            return trigger.OnPointerExit.AsObservable();
        }
        
        public static IObservable<TMP_CharacterInfo> OnPointerClickedCharacterAsObservable(this TMP_TextInfo info)
        {
            var trigger = info.textComponent.GetOrAddComponent<TMPCharacterEventTrigger>();
            return trigger.OnPointerClicked.AsObservable();
        }
        
        public static IObservable<TMP_WordInfo> OnPointerEnterWordAsObservable(this TMP_TextInfo info)
        {
            var trigger = info.textComponent.GetOrAddComponent<TMPWordEventTrigger>();
            return trigger.OnPointerEnter.AsObservable();
        }
        
        public static IObservable<TMP_WordInfo> OnPointerExitWordAsObservable(this TMP_TextInfo info)
        {
            var trigger = info.textComponent.GetOrAddComponent<TMPWordEventTrigger>();
            return trigger.OnPointerExit.AsObservable();
        }
        
        public static IObservable<TMP_WordInfo> OnPointerClickedWordAsObservable(this TMP_TextInfo info)
        {
            var trigger = info.textComponent.GetOrAddComponent<TMPWordEventTrigger>();
            return trigger.OnPointerClicked.AsObservable();
        }
        
        public static IObservable<TMP_LineInfo> OnPointerEnterLineAsObservable(this TMP_TextInfo info)
        {
            var trigger = info.textComponent.GetOrAddComponent<TMPLineEventTrigger>();
            return trigger.OnPointerEnter.AsObservable();
        }
        
        public static IObservable<TMP_LineInfo> OnPointerExitLineAsObservable(this TMP_TextInfo info)
        {
            var trigger = info.textComponent.GetOrAddComponent<TMPLineEventTrigger>();
            return trigger.OnPointerExit.AsObservable();
        }
        
        public static IObservable<TMP_LineInfo> OnPointerClickedLineAsObservable(this TMP_TextInfo info)
        {
            var trigger = info.textComponent.GetOrAddComponent<TMPLineEventTrigger>();
            return trigger.OnPointerClicked.AsObservable();
        }
        
        public static IObservable<TMP_LinkInfo> OnPointerEnterLinkAsObservable(this TMP_TextInfo info)
        {
            var trigger = info.textComponent.GetOrAddComponent<TMPLinkEventTrigger>();
            return trigger.OnPointerEnter.AsObservable();
        }
        
        public static IObservable<TMP_LinkInfo> OnPointerExitLinkAsObservable(this TMP_TextInfo info)
        {
            var trigger = info.textComponent.GetOrAddComponent<TMPLinkEventTrigger>();
            return trigger.OnPointerExit.AsObservable();
        }
        
        public static IObservable<TMP_LinkInfo> OnPointerClickedLinkAsObservable(this TMP_TextInfo info)
        {
            var trigger = info.textComponent.GetOrAddComponent<TMPLinkEventTrigger>();
            return trigger.OnPointerClicked.AsObservable();
        }

    }
}