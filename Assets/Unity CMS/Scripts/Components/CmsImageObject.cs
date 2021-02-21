#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace GG.UnityCMS
{
    public class CmsImageObject : CmsGameObject<CmsImageObject.ImageCmsModuleData>
    {
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