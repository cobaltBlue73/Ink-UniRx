using TMPro;
using UnityEngine;

namespace Utility.TextMeshPro
{
    public class TMPLinkEventTrigger : TMPTextEventTriggerBase<TMP_LinkInfo>
    {
        protected override int FindIntersectingTextElement() => 
            TMP_TextUtilities.FindIntersectingLink(Text, Input.mousePosition, Camera);

        protected override TMP_LinkInfo GetTextElementInfo(int index) => 
            Text.textInfo.linkInfo[index];
    }
}