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
            if (target == null)
            {
                return;
            }
            
            base.OnEnable();

            cmsGameObject = (CmsGameObject) target;
            newIdentifier = cmsGameObject.humanReadableKey;
            
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
                    currentKey = newKey;
                    newIdentifier = cmsGameObject.humanReadableKey;

                    cmsGameObject.humanReadableKey = options[newKey];
                    cmsGameObject.OnValidate();
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
                    if (CmsScriptableObject.Active.HasKey(newIdentifier))
                    {
                        Debug.LogWarning($"{newIdentifier} already exists as a value in the CMS, will attempt to add module into this key");
                    }

                    CmsStyle style = CmsScriptableObject.Active.GetOrAddStyle(newIdentifier);
                    
                    if (style.IsModulePresent(cmsGameObject))
                    {
                        Debug.LogError($"style {Helpers.GetDataClass(cmsGameObject)} already exists inside {newIdentifier} canceling");
                        return;
                    }

                    if (!style.TryAddModule(cmsGameObject))
                    {
                        Debug.LogError("Failed to add module");
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
    }
}