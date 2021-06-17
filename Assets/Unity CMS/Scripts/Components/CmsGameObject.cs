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
            Validate();
        }

        void OnValidate()
        {
            Validate();
        }

        public virtual void Validate()
        { }
    }
    
    public abstract class CmsGameObject<T> : CmsGameObject where T : class
    {
        public override void Validate()
        {
            if (CmsController.Exists)
            {
                if (Application.isPlaying || CmsController.Instance.liveUpdate)
                {
                    if (CmsScriptableObject.Active.GetModule<T>(this) is T module)
                    {
                        Apply(module);
                    }
                }
            }
        }

        protected abstract void Apply(T component);
    }
}