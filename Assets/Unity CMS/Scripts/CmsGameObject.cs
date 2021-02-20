using Sirenix.OdinInspector;
using UnityEngine;

namespace GG.UnityCMS
{
    public class CmsGameObject : MonoBehaviour
    {
        public string humanReadableKey;

        void Awake()
        {
            OnValidate();
        }
        
        public void OnValidate()
        {
            if (Application.isPlaying || CmsController.Instance.liveUpdate)
            {
                Apply(CmsController.Instance.styleScriptableObject);
            }
        }

        void Apply(CmsScriptableObject styleObject)
        {
            if (styleObject != null)
            {
                string hiddenGuid = CmsController.GetGuidFromKey(humanReadableKey);

                if (styleObject.uiStyles.primaryDictionary.ContainsKey(hiddenGuid))
                {
                    styleObject.uiStyles.GetValueFromPrimary(hiddenGuid).Apply(this);
                }
                else
                {
                    Debug.LogWarning($"No key matching {humanReadableKey} in {CmsController.Instance.styleScriptableObject}", gameObject);
                }
            }
        }
    }
}