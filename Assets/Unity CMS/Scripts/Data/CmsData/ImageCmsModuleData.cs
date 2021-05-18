using GG.UnityCMS;
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
}