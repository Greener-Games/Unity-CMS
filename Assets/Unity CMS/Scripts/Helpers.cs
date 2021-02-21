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

        public static CmsGameObject.CmsModuleData CreateModuleObject(Type moduleType)
        {
            string moduleName = moduleType.ToString();
            return CreateModuleObject(moduleName);
        }

        public static CmsGameObject.CmsModuleData CreateModuleObject(string moduleName)
        {
            Type type = Type.GetType(moduleName);
            if (type != null)
            {
                return (CmsGameObject.CmsModuleData) Activator.CreateInstance(type);
            }

            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(moduleName);
                if (type != null)
                {
                    return (CmsGameObject.CmsModuleData) Activator.CreateInstance(type);
                }
            }

            return null;
        }
        
        
        public static Type GetDataClass(CmsGameObject gameObject)
        {
            try
            {
                // Get the Type object corresponding to MyClass.
                Type myType = gameObject.GetType();
                
                // Get an array of nested type objects in MyClass.
                Type[] nestType = myType.GetNestedTypes();
                foreach (Type t in nestType)
                {
                    if (t.IsSubclassOf(typeof(CmsGameObject.CmsModuleData)))
                    {
                        return t;
                    }
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