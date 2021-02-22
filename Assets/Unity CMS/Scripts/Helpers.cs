#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

#endregion

namespace GG.UnityCMS
{
    public static class Helpers
    {
        public static Type GetType(string typeName)
        {
            Type type = Type.GetType(typeName);
            if (type != null)
            {
                return type;
            }

            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                {
                    return type;
                }
            }

            return null;
        }

        public static CmsModuleData CreateModuleObject(Type moduleType)
        {
            string moduleName = moduleType.ToString();
            return CreateModuleObject(moduleName);
        }

        public static CmsModuleData CreateModuleObject(string moduleName)
        {
            Type type = Type.GetType(moduleName);
            if (type != null)
            {
                return (CmsModuleData) Activator.CreateInstance(type);
            }

            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(moduleName);
                if (type != null)
                {
                    return (CmsModuleData) Activator.CreateInstance(type);
                }
            }

            return null;
        }
        
        
        public static Type GetDataClass(CmsGameObject gameObject)
        {
            try
            {
                object[] attributes = gameObject.GetType().GetCustomAttributes(typeof(DataAttribute), true);
                if (attributes.Length > 0)
                {
                    Type t = (attributes[0] as DataAttribute)?.linkedType;
                    return t;
                }
            }
            catch(Exception e)
            {
                Debug.Log("Error"+e.Message);
            }

            return null;
        }
    }
}