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
}