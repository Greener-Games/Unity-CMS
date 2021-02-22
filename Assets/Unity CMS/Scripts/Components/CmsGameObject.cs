using System;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace GG.UnityCMS
{
    public abstract class CmsGameObject : MonoBehaviour
    {
        public string humanReadableKey;

        void Awake()
        {
            OnValidate();
        }
        
        public void OnValidate()
        { }
    }
    
    public abstract class CmsGameObject<T> : CmsGameObject where T : class
    {
        public new void OnValidate()
        {
            if (Application.isPlaying || CmsController.Instance.liveUpdate)
            {
                if (CmsScriptableObject.Active.GetModule<T>(this) is T module)
                {
                    Apply(module);
                }
            }
        }

        protected abstract void Apply(T component);
    }
}