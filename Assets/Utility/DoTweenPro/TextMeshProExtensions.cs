using DG.Tweening;
using TMPro;

namespace Utility.DoTweenPro
{
    public static class TextMeshProExtensions
    {
        public static Tweener DOVisibleCharacters(this TMP_Text text, float duration)
        {
            text.ForceMeshUpdate();
            var target = text.textInfo.characterCount;
            text.maxVisibleCharacters = 0;
            return DOTween.To(() => text.maxVisibleCharacters,
                val => text.maxVisibleCharacters = val,
                target, duration);
        }
        
        public static Tweener DOVisibleWords(this TMP_Text text, float duration)
        {
            text.ForceMeshUpdate();
            var target = text.textInfo.wordCount;
            text.maxVisibleWords = 0;
            return DOTween.To(() => text.maxVisibleWords,
                val => text.maxVisibleWords = val,
                target, duration);
        }
        
        public static Tweener DOVisibleLines(this TMP_Text text, float duration)
        {
            text.ForceMeshUpdate();
            var target = text.textInfo.lineCount;
            text.maxVisibleLines = 0;
            return DOTween.To(() => text.maxVisibleLines,
                val => text.maxVisibleLines = val,
                target, duration);
        }
        
        public static Tweener DOVisibleCharacters(this TMP_Text text, string newText, float duration)
        {
            text.text = newText;
            return text.DOVisibleCharacters(duration);
        }
        
        public static Tweener DOVisibleWords(this TMP_Text text, string newText, float duration)
        {
            text.text = newText;
            return text.DOVisibleWords(duration);
        }
        
        public static Tweener DOVisibleLines(this TMP_Text text, string newText, float duration)
        {
            text.text = newText;
            return text.DOVisibleLines(duration);
        }
    }
}