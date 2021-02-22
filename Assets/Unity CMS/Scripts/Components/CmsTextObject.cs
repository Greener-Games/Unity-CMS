using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace GG.UnityCMS
{
    [DataAttribute(typeof(TextMeshCmsModuleData))]
    public class CmsTextObject : CmsGameObject<TextMeshCmsModuleData>
    {
        protected override void Apply(TextMeshCmsModuleData content)
        {
            TextMeshProUGUI textMeshProUgui = GetComponent<TextMeshProUGUI>();

            if (textMeshProUgui != null)
            {
                textMeshProUgui.font = content.font;
                textMeshProUgui.color = content.color;
                textMeshProUgui.fontSize = content.fontSize;
            }
        }
    }
}