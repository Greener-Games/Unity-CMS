using System.ComponentModel;
using GG;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Component = UnityEngine.Component;

namespace GG.UnityCMS
{
    public enum StyleType
    {
        [Description("Image")]IMAGE,
        [Description("Font")]FONT,
        [Description("Error")]ERROR
    }

    public class CmsScriptableObject : SerializedScriptableObject
    {
        //Gui, Human readable, value
        public SecondaryKeyDictionary<string, string, CmsData> uiStyles = new SecondaryKeyDictionary<string, string, CmsData>();

        public bool HasKey(string humanReadable)
        {
            return uiStyles.ContainsSecondaryKey(humanReadable);
        }
        /// <summary>
        /// Returns the type of Ui style that the style object would require
        /// </summary>
        /// <param name="cmsObject"></param>
        /// <returns></returns>
        public static StyleType GetModuleTypeForObject(Component cmsObject)
        {
            if (cmsObject.GetComponent<Image>() != null)
            {
                return StyleType.IMAGE;
            }
            else if(cmsObject.GetComponent<TextMeshProUGUI>() != null)
            {
                return StyleType.FONT;
            }

            return StyleType.ERROR;
        }
        
        public static bool CheckValidModule(CmsGameObject cmsGameObject)
        {
            string guidFromKey = CmsController.GetGuidFromKey(cmsGameObject.humanReadableKey);
            StyleType styleType = GetModuleTypeForObject(cmsGameObject);
            switch (styleType)
            {
                case StyleType.IMAGE:
                    return cmsGameObject.GetComponent<Image>() != null && CmsController.Instance.styleScriptableObject.uiStyles.GetValueFromPrimary(guidFromKey).modules.ContainsKey(styleType);
                case StyleType.FONT:
                    return cmsGameObject.GetComponent<TextMeshProUGUI>() != null && CmsController.Instance.styleScriptableObject.uiStyles.GetValueFromPrimary(guidFromKey).modules.ContainsKey(styleType);;
                case StyleType.ERROR:
                default:
                    return false;
            }

            return false;
        }
        
        public CmsData AddStyle(string guid, string humanReadable, StyleType styleType, GameObject content = null)
        {
            CmsData data = AddStyle(guid, humanReadable);
            data.AddStyle(styleType, content);
            return data;
        }
        
        public CmsData AddStyle(string guid, string humanReadable)
        {
            CmsData data = new CmsData();
            uiStyles.Add(guid,data, humanReadable);

            return data;
        }
    }
}
