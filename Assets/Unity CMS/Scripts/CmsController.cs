using System;
using GG.Singletons;
using UnityEngine;
using UnityEngine.Serialization;

namespace GG.UnityCMS
{
    public class CmsController : UnitySingleton<CmsController>
    {
        public CmsScriptableObject styleScriptableObject;
        public bool liveUpdate = true;

        public void OnValidate()
        {
            CmsGameObject[] comps = GetComponentsInChildren<CmsGameObject>(true);
            foreach (CmsGameObject comp in comps)
            {
                comp.OnValidate();
            }
        }

        public bool HasKey(string humanReadableKey)
        {
            return styleScriptableObject.HasKey(humanReadableKey);
        }

        public static string GetGuidFromKey(string humanReadableKey)
        {
            try
            {
                return Instance.styleScriptableObject.uiStyles.secondaryKeyLink[humanReadableKey];
            }
            catch (Exception e)
            {
                Debug.Log($"unable to get guid of key {humanReadableKey}");
                return "";
            }
        }
    }
}