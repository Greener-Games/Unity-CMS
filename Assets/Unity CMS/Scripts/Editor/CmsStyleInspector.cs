#region

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

#endregion

namespace GG.UnityCMS.Editor
{
    [CustomEditor(typeof(CmsGameObject))]
    [CanEditMultipleObjects]
    public class CmsStyleInspector : OdinEditor
    {
        string key;
        StyleType style;

        CmsGameObject cmsGameObject;
        bool addStyle;

        int currentKey = -1;
        List<string> options = new List<string>();

        string newIdentifier;
        
        protected override void OnEnable()
        {
            base.OnEnable();

            cmsGameObject = (CmsGameObject) target;
            key = cmsGameObject.humanReadableKey;

            options = CmsController.Instance.styleScriptableObject.uiStyles.SecondaryKeys;
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
                    cmsGameObject.humanReadableKey = options[newKey];
                    cmsGameObject.OnValidate();
                }
            }
            GUILayout.EndHorizontal();

            if (currentKey == -1)
            {
                EditorGUILayout.HelpBox($"Style Key {cmsGameObject.humanReadableKey} does not exist on this style sheet", MessageType.Error);
            }
            else if (!CmsScriptableObject.CheckValidModule(cmsGameObject))
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
            style = (StyleType) EditorGUILayout.EnumPopup("Type", style);

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Create"))
                {
                    if (CmsController.Instance.HasKey(newIdentifier))
                    {
                        Debug.LogError($"{newIdentifier} already exists as a value in the CMS");
                        return;
                    }

                    CmsController.Instance.styleScriptableObject.AddStyle(Guid.NewGuid().ToString(), newIdentifier,style, cmsGameObject.gameObject);
                    cmsGameObject.humanReadableKey = newIdentifier;
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