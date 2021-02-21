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
    }
}