using TMPro;
using UnityEngine;

namespace Utility.TextMeshPro
{
    public class TMPCharacterEventTrigger : TMPTextEventTriggerBase<TMP_CharacterInfo>
    {
        protected override int FindIntersectingTextElement() => 
            TMP_TextUtilities.FindIntersectingCharacter(Text, Input.mousePosition, Camera, true);

        protected override TMP_CharacterInfo GetTextElementInfo(int index) => 
            Text.textInfo.characterInfo[index];
    }
}