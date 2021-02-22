using GG.UnityCMS;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ImageCmsModuleData : CmsModuleData
{
    public Color color = Color.white;
        
    public override void Populate(CmsGameObject content)
    {
        Image img = content.GetComponent<Image>();
        if (img != null)
        {
            color = img.color;
        }
    }
            
#if UNITY_EDITOR
    public override void DrawEditor()
    {
        color = EditorGUILayout.ColorField("Color", color);
    }
#endif
}