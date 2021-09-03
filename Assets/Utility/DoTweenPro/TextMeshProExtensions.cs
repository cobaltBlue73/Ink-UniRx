using DG.Tweening;
using TMPro;

namespace Utility.DoTweenPro
{
    public static class TextMeshProExtensions
    {
        public static Tweener DOVisibleCharacters(this TMP_Text text, int endValue, float duration)
        {
            return DOTween.To(() => text.maxVisibleCharacters,
                val => text.maxVisibleCharacters = val,
                endValue, duration);
        }
        
        public static Tweener DOVisibleWords(this TMP_Text text, int endValue, float duration)
        {
            return DOTween.To(() => text.maxVisibleWords,
                val => text.maxVisibleWords = val,
                endValue, duration);
        }
        
        public static Tweener DOVisibleLines(this TMP_Text text,int endValue, float duration)
        {
            return DOTween.To(() => text.maxVisibleLines,
                val => text.maxVisibleLines = val,
                endValue, duration);
        }
        
        public static Tweener DOVisibleCharacters(this TMP_Text text, string newText,int endValue, float duration)
        {
            text.text = newText;
            return text.DOVisibleCharacters(endValue, duration);
        }
        
        public static Tweener DOVisibleWords(this TMP_Text text, string newText,int endValue, float duration)
        {
            text.text = newText;
            return text.DOVisibleWords(endValue, duration);
        }
        
        public static Tweener DOVisibleLines(this TMP_Text text, string newText,int endValue, float duration)
        {
            text.text = newText;
            return text.DOVisibleLines(endValue, duration);
        }
    }
}