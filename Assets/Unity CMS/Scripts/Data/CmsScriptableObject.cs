using System;
using System.ComponentModel;
using System.Linq;
using GG;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Component = UnityEngine.Component;

namespace GG.UnityCMS
{
    public class CmsScriptableObject : SerializedScriptableObject
    {
        public static CmsScriptableObject Active => CmsController.Instance.styleScriptableObject;
        
        //Gui, Human readable, value
        public SecondaryKeyDictionary<string, string, CmsStyle> styles = new SecondaryKeyDictionary<string, string, CmsStyle>();

        public bool HasKey(string humanReadable)
        {
            return styles.ContainsSecondaryKey(humanReadable);
        }

        public string GetGuidFromKey(string humanReadableKey)
        {
            try
            {
                return styles.secondaryKeyLink[humanReadableKey];
            }
            catch (Exception e)
            {
                Debug.Log($"unable to get guid of key {humanReadableKey}");
                return "";
            }
        }

        public CmsStyle GetOrAddStyle(string humanReadableKey)
        {
            CmsStyle style = GetStyle(humanReadableKey);

            if (style == null)
            {
                style = AddStyle(humanReadableKey);
            }

            return style;
        }
        
        /// <summary>
        /// Get a style for a human readable name
        /// </summary>
        /// <returns></returns>
        public CmsStyle GetStyle(CmsGameObject cmsGameObject)
        {
            return GetStyle(cmsGameObject.humanReadableKey);
        }
        
        /// <summary>
        /// Get a style for a human readable name
        /// </summary>
        /// <returns></returns>
        public CmsStyle GetStyle(string humanReadableKey)
        {
            string hiddenGuid = GetGuidFromKey(humanReadableKey);
            if (styles.primaryDictionary.ContainsKey(hiddenGuid))
            {
                CmsStyle cmsStyle = styles.GetValueFromPrimary(hiddenGuid);
                return cmsStyle;
            }
            else
            {
                Debug.LogWarning($"No key matching {humanReadableKey} in {CmsController.Instance.styleScriptableObject}");
            }

            return null;
        }
        
        
        /// <summary>
        /// Get the required module for a cms Object
        /// </summary>
        /// <returns></returns>
        public T GetModule<T>(CmsGameObject cmsGameObject) where T : class
        {
            CmsStyle style = GetStyle(cmsGameObject);

            if (style?.GetModule(cmsGameObject) is T module)
            {
                return module;
            }
            else
            {
                Debug.LogWarning($"No module for {Helpers.GetDataClass(cmsGameObject)} inside style {cmsGameObject.humanReadableKey}");
            }

            return null;
        }
        
        /// <summary>
        /// Check if a module exists within a style group
        /// </summary>
        /// <param name="cmsGameObject"></param>
        /// <returns></returns>
        public bool CheckValidModuleExists(CmsGameObject cmsGameObject)
        {
            string guidFromKey = GetGuidFromKey(cmsGameObject.humanReadableKey);
            Type dataClass = Helpers.GetDataClass(cmsGameObject);
            
            return styles.GetValueFromPrimary(guidFromKey).IsModulePresent(dataClass);
        }
        
        /// <summary>
        /// Add a style to the style map
        /// </summary>
        /// <param name="humanReadable"></param>
        /// <returns></returns>
        public CmsStyle AddStyle(string humanReadable)
        {
            CmsStyle style = new CmsStyle();
            styles.Add(Guid.NewGuid().ToString(),style, humanReadable);
            return style;
        }
    }
}
