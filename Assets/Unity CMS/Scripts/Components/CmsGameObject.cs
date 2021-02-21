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
        //TODO: find a way to not have these as a sub class of their monobehaviour
        [Serializable]
        public abstract class CmsModuleData
        {
            string name = "";

            public string Name
            {
                get
                {
                    if (string.IsNullOrEmpty(name))
                    {
                       name = string.Concat(GetType().Name.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
                    }

                    return name;
                }
            }

            public bool foldout;

            public abstract void Populate(CmsGameObject component);

            //TODO: need to think of a better way to link the editor without having it embedded in the class structure
            public abstract void DrawEditor();

            public override string ToString()
            {
                return Name;
            }
        }
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