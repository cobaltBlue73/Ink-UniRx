using DG.Tweening;
using TMPro;

namespace Utility.DoTweenPro
{
    public static class TextMeshProExtensions
    {
        public static Tweener DOTextPerCharacter(this TMP_Text text, float duration)
        {
            text.ForceMeshUpdate();
            var target = text.textInfo.characterCount;
            text.maxVisibleCharacters = 0;
            return DOTween.To(() => text.maxVisibleCharacters,
                val => text.maxVisibleCharacters = val,
                target, duration);
        }
        
        public static Tweener DOTextPerWord(this TMP_Text text, float duration)
        {
            text.ForceMeshUpdate();
            var target = text.textInfo.wordCount;
            text.maxVisibleWords = 0;
            return DOTween.To(() => text.maxVisibleWords,
                val => text.maxVisibleWords = val,
                target, duration);
        }
        
        public static Tweener DOTextPerLine(this TMP_Text text, float duration)
        {
            text.ForceMeshUpdate();
            var target = text.textInfo.lineCount;
            text.maxVisibleLines = 0;
            return DOTween.To(() => text.maxVisibleLines,
                val => text.maxVisibleLines = val,
                target, duration);
        }
        
        public static Tweener DOTextPerCharacter(this TMP_Text text, string to, float duration)
        {
            text.text = to;
            return text.DOTextPerCharacter(duration);
        }
        
        public static Tweener DOTextPerWord(this TMP_Text text, string to, float duration)
        {
            text.text = to;
            return text.DOTextPerWord(duration);
        }
        
        public static Tweener DOTextPerLine(this TMP_Text text, string to, float duration)
        {
            text.text = to;
            return text.DOTextPerLine(duration);
        }
    }
}