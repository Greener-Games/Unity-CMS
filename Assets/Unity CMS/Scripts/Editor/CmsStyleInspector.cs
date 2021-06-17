#region

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

#endregion

namespace GG.UnityCMS.Editor
{
    [CustomEditor(typeof(CmsGameObject), true)]
    [CanEditMultipleObjects]
    public class CmsStyleInspector : OdinEditor
    {
        CmsGameObject cmsGameObject;
        bool addStyle;

        int currentKey = -1;
        List<string> options = new List<string>();

        string newIdentifier;
        
        protected override void OnEnable()
        {
            if (target == null || !CmsController.Exists)
            {
                return;
            }
            
            base.OnEnable();

            cmsGameObject = (CmsGameObject) target;
            
            options = CmsController.Instance.styleScriptableObject.styles.SecondaryKeys;
            if (options.Contains(cmsGameObject.humanReadableKey))
            {
                currentKey = options.IndexOf(cmsGameObject.humanReadableKey);
            }
        }

        public override void OnInspectorGUI()
        {
            if (!CmsController.Exists && CmsController.Instance.styleScriptableObject == null)
            {
                EditorGUILayout.HelpBox("Please make sure a cms controller is present and that a style is selected", MessageType.Error);
                return;
            }

            GUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Key", GUILayout.Width(100));
                int newKey = EditorGUILayout.Popup(currentKey, options.ToArray());
                if (newKey >= 0 && currentKey != newKey)
                {
                    SetKey(newKey);
                }
            }
            GUILayout.EndHorizontal();

            if (currentKey == -1)
            {
                EditorGUILayout.HelpBox($"Style Key {cmsGameObject.humanReadableKey} does not exist on this style sheet", MessageType.Error);
            }
            else if (!CmsScriptableObject.Active.CheckValidModuleExists(cmsGameObject))
            {
                EditorGUILayout.HelpBox("Key does not contain the correct UI element module", MessageType.Error);
            }

            if (GUILayout.Button("Add Style"))
            {
                newIdentifier = cmsGameObject.humanReadableKey;
                addStyle = true;
            }

            if (addStyle)
            {
                DrawStyleAdder();
            }
        }
        
        void DrawStyleAdder()
        {
            newIdentifier = EditorGUILayout.TextField("Key", newIdentifier);
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Create"))
                {
                    bool success = true;
                    if (CmsScriptableObject.Active.HasKey(newIdentifier))
                    {
                        Debug.LogWarning($"{newIdentifier} already exists as a value in the CMS, will attempt to add module into this key");
                    }

                    CmsStyle style = CmsScriptableObject.Active.GetOrAddStyle(newIdentifier);
                    
                    if (style.IsModulePresent(cmsGameObject))
                    {
                        Debug.LogError($"style {Helpers.GetDataClass(cmsGameObject)} already exists inside {newIdentifier} canceling");
                        success = false;
                        return;
                    }

                    if (!style.TryAddModule(cmsGameObject))
                    {
                        Debug.LogError("Failed to add module");
                        success = false;
                    }
                    
                    if (success)
                    {
                        options.Add(newIdentifier);
                        SetKey(options.Count - 1);
                    }
                    addStyle = false;
                }

                if (GUILayout.Button("Cancel"))
                {
                    addStyle = false;
                }
            }
            GUILayout.EndHorizontal();
        }

        void SetKey(int key)
        {
            currentKey = key;
            cmsGameObject.humanReadableKey = options[currentKey];
            cmsGameObject.Validate();
        }
    }
}