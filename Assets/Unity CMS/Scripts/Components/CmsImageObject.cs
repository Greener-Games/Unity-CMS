#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace GG.UnityCMS
{
    [DataAttribute(typeof(ImageCmsModuleData))]
    public class CmsImageObject : CmsGameObject<ImageCmsModuleData>
    {
        protected override void Apply(ImageCmsModuleData content)
        {
            Image img = GetComponent<Image>();
            if (img != null)
            {
                img.color = content.color;
            }
        }


    }
}