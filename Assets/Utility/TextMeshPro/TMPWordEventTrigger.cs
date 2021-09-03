using TMPro;
using UnityEngine;

namespace Utility.TextMeshPro
{
    public class TMPWordEventTrigger : TMPTextEventTriggerBase<TMP_WordInfo>
    {
        protected override int FindIntersectingTextElement() => 
            TMP_TextUtilities.FindIntersectingWord(Text, Input.mousePosition, Camera);


        protected override TMP_WordInfo GetTextElementInfo(int index) => 
            Text.textInfo.wordInfo[index];
    }
}