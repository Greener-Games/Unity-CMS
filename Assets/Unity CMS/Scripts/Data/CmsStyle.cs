#region

using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;

#endregion

namespace GG.UnityCMS
{
    public class CmsStyle
    {
        [ShowInInspector][SerializeField]Dictionary<Type, CmsModuleData> modules = new Dictionary<Type, CmsModuleData>();
        public Dictionary<Type, CmsModuleData> Modules => modules;
        public bool foldout;

        public CmsModuleData GetModule(CmsGameObject cmsObject)
        {
            return GetModule(Helpers.GetDataClass(cmsObject));
        }
        
        public CmsModuleData GetModule(Type type)
        {
            return modules.ContainsKey(type) ? modules[type] : null;
        }
        
        public bool IsModulePresent(CmsGameObject cmsObject)
        {
            return modules.ContainsKey(Helpers.GetDataClass(cmsObject));
        }
        
        public bool IsModulePresent(Type dataClass)
        {
            return modules.ContainsKey(dataClass);
        }
        
        public bool TryAddModule(CmsGameObject cmsGameObject)
        {
            return TryAddModule(Helpers.GetDataClass(cmsGameObject), cmsGameObject);
        }

        public bool TryAddModule(Type styleType, CmsGameObject cmsGameObject = null)
        {
            if (IsModulePresent(styleType))
            {
                return false;
            }
            
            CmsModuleData moduleData = Helpers.CreateModuleObject(styleType);

            if (cmsGameObject != null)
            {
                moduleData.Populate(cmsGameObject);
            }
            
            modules.Add(styleType, moduleData);


            return true;
        }
    }
}