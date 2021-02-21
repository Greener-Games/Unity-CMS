using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace GG.UnityCMS
{
    public class CmsTextObject : CmsGameObject<CmsTextObject.TextMeshCmsModuleData>
    {
        [System.Serializable]
        public class TextMeshCmsModuleData : CmsModuleData
        {
            public TMP_FontAsset font;
            public Color color;
            public float fontSize;
            
            public override void Populate(CmsGameObject content)
            {
                TextMeshProUGUI textMeshProUgui = content.GetComponent<TextMeshProUGUI>();
                if (textMeshProUgui != null)
                {
                    font = textMeshProUgui.font ;
                    color = textMeshProUgui.color;
                    fontSize = textMeshProUgui.fontSize;
                }
            }
            
            #if UNITY_EDITOR
            public override void DrawEditor()
            {
                color = EditorGUILayout.ColorField("Color", color);
                font = EditorGUILayout.ObjectField("Font", font, typeof(TMP_FontAsset), false) as TMP_FontAsset;
                fontSize = EditorGUILayout.FloatField("Font Size", fontSize);
            }
            #endif
        }
        
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