using DG.Tweening;
using TMPro;

namespace Utility.DoTweenPro
{
    public static class TextMeshProExtensions
    {
        public static Tweener DOTextPerCharacter(this TextMeshPro text, string to, float duration)
        {
            text.maxVisibleCharacters = 0;
            text.text = to;
            return DOTween.To(() => text.maxVisibleCharacters, 
                val => text.maxVisibleCharacters = val,
                text.textInfo.characterCount, duration);
        }
        
        public static Tweener DOTextPerWord(this TextMeshPro text, string to, float duration)
        {
            text.maxVisibleWords = 0;
            text.text = to;
            return DOTween.To(() => text.maxVisibleWords, 
                val => text.maxVisibleWords = val,
                text.textInfo.wordCount, duration);
        }
        
        public static Tweener DOTextPerLine(this TextMeshPro text, string to, float duration)
        {
            text.maxVisibleLines = 0;
            text.text = to;
            return DOTween.To(() => text.maxVisibleLines, 
                val => text.maxVisibleLines = val,
                text.textInfo.lineCount, duration);
        }
        
        public static Tweener DOTextPerCharacter(this TextMeshProUGUI text, string to, float duration)
        {
            text.maxVisibleCharacters = 0;
            text.text = to;
            return DOTween.To(() => text.maxVisibleCharacters, 
                val => text.maxVisibleCharacters = val,
                text.textInfo.characterCount, duration);
        }
        
        public static Tweener DOTextPerWord(this TextMeshProUGUI text, string to, float duration)
        {
            text.maxVisibleWords = 0;
            text.text = to;
            return DOTween.To(() => text.maxVisibleWords, 
                val => text.maxVisibleWords = val,
                text.textInfo.wordCount, duration);
        }
        
        public static Tweener DOTextPerLine(this TextMeshProUGUI text, string to, float duration)
        {
            text.maxVisibleLines = 0;
            text.text = to;
            return DOTween.To(() => text.maxVisibleLines, 
                val => text.maxVisibleLines = val,
                text.textInfo.lineCount, duration);
        }
    }
}