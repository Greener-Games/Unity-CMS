using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GG.UnityCMS
{
    public class TextMeshCmsModule : CmsModule
    {
        public TMP_FontAsset font;
        public Color color;
        public float fontSize;

        public override void Apply(GameObject content)
        {
            TextMeshProUGUI textMeshProUgui = content.GetComponent<TextMeshProUGUI>();
            if (textMeshProUgui != null)
            {
                textMeshProUgui.font = font;
                textMeshProUgui.color = color;
                textMeshProUgui.fontSize = fontSize;
            }
        }

        public override void Populate(GameObject content)
        {
            TextMeshProUGUI textMeshProUgui = content.GetComponent<TextMeshProUGUI>();
            if (textMeshProUgui != null)
            {
                textMeshProUgui.font = textMeshProUgui.font;
                textMeshProUgui.color = textMeshProUgui.color;
                textMeshProUgui.fontSize = textMeshProUgui.fontSize;
            }
        }
    }
}