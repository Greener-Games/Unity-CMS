using System;
using GG.Singletons;
using UnityEngine;
using UnityEngine.Serialization;

namespace GG.UnityCMS
{
    public class CmsController : UnitySingletonPersistent<CmsController>
    {
        public CmsScriptableObject styleScriptableObject;
        public bool liveUpdate = true;

        public void OnValidate()
        {
            CmsGameObject[] comps = FindObjectsOfType<CmsGameObject>();
            foreach (CmsGameObject comp in comps)
            {
                Debug.Log(comp.GetType());
                comp.Validate();
            }
        }
    }
}