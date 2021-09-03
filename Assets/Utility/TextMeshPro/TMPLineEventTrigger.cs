using TMPro;
using UnityEngine;

namespace Utility.TextMeshPro
{
    public class TMPLineEventTrigger : TMPTextEventTriggerBase<TMP_LineInfo>
    {
        protected override int FindIntersectingTextElement() =>
            TMP_TextUtilities.FindIntersectingLine(Text, Input.mousePosition, Camera);

        protected override TMP_LineInfo GetTextElementInfo(int index) => 
            Text.textInfo.lineInfo[index];
    }
}