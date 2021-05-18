using System;
using System.Linq;
using System.Reflection;
using GG.UnityCMS;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
    
    public override string ToString()
    {
        return Name;
    }
    
#if UNITY_EDITOR
    public virtual void DrawEditor()
    {
        Type type = GetType();
     
        FieldInfo[] fields = type.GetFields();
        foreach(FieldInfo field in fields)
        {
            //dont draw the foldout variable
            if (field.Name == nameof(foldout))
            {
                continue;
            }
            
            object temp = field.GetValue(this);
            switch (temp)
            {
                case string s:
                    field.SetValue(this, EditorGUILayout.TextField(field.Name + ": ", s));
                    break;
                case float f:
                    field.SetValue(this, EditorGUILayout.FloatField(field.Name + ": ", f));
                    break;
                case int i:
                    field.SetValue(this, EditorGUILayout.IntField(field.Name + ": ", i));
                    break;
                case bool b:
                    field.SetValue(this, EditorGUILayout.Toggle(field.Name + ": ", b));
                    break;
                case Color color:
                    field.SetValue(this, EditorGUILayout.ColorField(field.Name + ": ", color));
                    break;
                default:
                    field.SetValue(this, EditorGUILayout.ObjectField(field.Name, temp as Object, field.FieldType));
                    break;
            }
        }
    }
#endif
}