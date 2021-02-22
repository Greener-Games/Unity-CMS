using GG.UnityCMS;
using TMPro;
using UnityEditor;
using UnityEngine;

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