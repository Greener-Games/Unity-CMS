using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GG.UnityCMS
{
    public class CmsData
    { 
        public readonly Dictionary<StyleType, CmsModule> modules = new Dictionary<StyleType, CmsModule>();
        public bool foldout;
        
        /// <summary>
        /// Adds a style and applies the content into that style
        /// </summary>
        /// <param name="styleType"></param>
        /// <param name="content"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void AddStyle(StyleType styleType, GameObject content = null)
        {
            switch (styleType)
            {
                case StyleType.IMAGE:
                {
                    modules.Add(styleType, new ImageCmsModule());
                    if (content != null)
                    {
                        modules[styleType] = new ImageCmsModule().Set(content.GetComponent<Image>());
                    }
                    break;
                }
                case StyleType.FONT:
                {
                    modules.Add(styleType, new TextMeshCmsModule());
                    if (content != null)
                    {
                        modules[styleType] = new TextMeshCmsModule().Set(content.GetComponent<TextMeshProUGUI>());
                    }

                    break;
                }
                case StyleType.ERROR:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        /// <summary>
        /// Apply the style to the object
        /// </summary>
        /// <param name="cmsGameObject"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Apply(CmsGameObject cmsGameObject)
        {
            StyleType styleType = CmsScriptableObject.GetModuleTypeForObject(cmsGameObject);
            if (modules.ContainsKey(styleType))
            {
                switch (styleType)
                {
                    case StyleType.IMAGE:
                    {
                        (modules[styleType] as ImageCmsModule)?.Apply(cmsGameObject.GetComponent<Image>());
                        break;
                    }
                    case StyleType.FONT:
                    {
                        (modules[styleType] as TextMeshCmsModule)?.Apply(cmsGameObject.GetComponent<TextMeshProUGUI>());
                        break;
                    }
                    case StyleType.ERROR:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}