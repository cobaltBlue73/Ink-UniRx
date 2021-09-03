using TMPro;

namespace Utility.TextMeshPro
{
    public static class CharacterInfoExtensions
    {
        public static bool IsLetterOrDigit(this TMP_CharacterInfo info) => char.IsLetterOrDigit(info.character);
        
        public static bool IsLetter(this TMP_CharacterInfo info) => char.IsLetter(info.character);
        
        public static bool IsDigit(this TMP_CharacterInfo info) => char.IsDigit(info.character);
        
        public static bool IsNumber(this TMP_CharacterInfo info) => char.IsNumber(info.character);
        
        public static bool IsControl(this TMP_CharacterInfo info) => char.IsControl(info.character);
        
        public static bool IsLower(this TMP_CharacterInfo info) => char.IsLower(info.character);
        
        public static bool IsPunctuation(this TMP_CharacterInfo info) => char.IsPunctuation(info.character);
        
        public static bool IsSeparator(this TMP_CharacterInfo info) => char.IsSeparator(info.character);
        
        public static bool IsSurrogate(this TMP_CharacterInfo info) => char.IsSurrogate(info.character);
        
        public static bool IsUpper(this TMP_CharacterInfo info) => char.IsUpper(info.character);
        
        public static bool IsHighSurrogate(this TMP_CharacterInfo info) => char.IsHighSurrogate(info.character);
        
        public static bool IsLowSurrogate(this TMP_CharacterInfo info) => char.IsLowSurrogate(info.character);
        
        public static bool IsSymbol(this TMP_CharacterInfo info) => char.IsSymbol(info.character);
        
        public static bool IsWhiteSpace(this TMP_CharacterInfo info) => char.IsWhiteSpace(info.character);
        
    }
}