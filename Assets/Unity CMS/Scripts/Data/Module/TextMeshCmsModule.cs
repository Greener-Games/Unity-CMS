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

        public void Apply(TextMeshProUGUI textMeshProUgui)
        {
            if (textMeshProUgui != null)
            {
                textMeshProUgui.font = font;
                textMeshProUgui.color = color;
                textMeshProUgui.fontSize = fontSize;
            }
        }

        public TextMeshCmsModule Set(TextMeshProUGUI textMeshProUgui)
        {
            if (textMeshProUgui != null)
            {
                textMeshProUgui.font = textMeshProUgui.font;
                textMeshProUgui.color = textMeshProUgui.color;
                textMeshProUgui.fontSize = textMeshProUgui.fontSize;
            }

            return this;
        }
    }
}