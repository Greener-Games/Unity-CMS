#region

using System;
using System.Collections.Generic;
using System.Linq;
using GG;
using GG.DataStructures;
using GG.Extensions;
using TMPro;
using UnityEditor;
using UnityEngine;

#endregion

namespace GG.UnityCMS.Editor
{
    public class UiStyleEditor : EditorWindow
    {
        protected static readonly float SingleLine = EditorGUIUtility.singleLineHeight;

        static UiStyleEditor window;

        static UiStyleEditor Window
        {
            get
            {
                if (window == null)
                {
                    window = GetWindow<UiStyleEditor>(true, "Style Editor", true);
                    window.minSize = new Vector2(1000, 400);
                }

                return window;
            }
            set => window = value;
        }

        static GUIStyle SmallButton => mSmallButton ?? (mSmallButton = new GUIStyle(EditorStyles.miniButton) {fixedWidth = 100});
        static GUIStyle mSmallButton;
        
        static GUIStyle Foldout => _mFoldout ?? (_mFoldout = new GUIStyle(EditorStyles.foldout) {fixedWidth = 100});
        static GUIStyle _mFoldout;

        int selectedIndex;
        List<CmsScriptableObject> styleObjects = new List<CmsScriptableObject>();

        [MenuItem("Unity CMS/Style Sheet", false, 1)]
        static void OpenIcon()
        {
            Open();
        }

        static void Open()
        {
            Window.Show();
            Window.Init();
        }

        void Init()
        {
            styleObjects = Resources.FindObjectsOfTypeAll<CmsScriptableObject>().ToList();
        }

        void OnGUI()
        {
            if (styleObjects.Count == 0)
            {
                return;
            }

            selectedIndex = EditorGUILayout.Popup("Style", selectedIndex, styleObjects.Select(x => x.name).ToArray());
            CmsScriptableObject styleScriptableObject = styleObjects[selectedIndex];
            DrawLine();

            if (styleScriptableObject == null)
            {
                return;
            }
            
            EditorGUI.BeginChangeCheck();
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Space(25);
                    GUILayout.BeginVertical();
                    {
                        DrawModules(styleScriptableObject);
                    }
                    GUILayout.EndVertical();
                    GUILayout.Space(10);
                }
                GUILayout.EndHorizontal();
            }
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(styleScriptableObject);
            }
        }

        void DrawModules(CmsScriptableObject styleScriptableObject)
        {
            SecondaryKeyDictionary<Guid, string, CmsStyle> styleMap = new SecondaryKeyDictionary<Guid, string, CmsStyle>(styleScriptableObject.styles);
            
            foreach (KeyValuePair<Guid, CmsStyle> pair in styleMap.primaryDictionary)
            {
                string entryKey;
                string oldKey = styleMap.GetSecondaryKey(pair.Key);
                EditorGUI.BeginChangeCheck();
                {
                    GUILayout.BeginHorizontal();
                    {
                        pair.Value.foldout = EditorGUILayout.Foldout(pair.Value.foldout, "Key", Foldout);
                        entryKey = EditorGUILayout.TextField(oldKey);
                        
                        if (GUILayout.Button("X", SmallButton))
                        {
                            styleScriptableObject.styles.RemoveUsingSecondary(oldKey);
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                if (EditorGUI.EndChangeCheck())
                {
                    if (!styleScriptableObject.styles.secondaryKeyLink.ContainsKey(entryKey))
                    {
                        styleScriptableObject.styles.secondaryKeyLink.ChangeKey(oldKey, entryKey);
                    }
                }

                if (pair.Value.foldout)
                {
                    Dictionary<Type, CmsModuleData> modules = pair.Value.Modules;
                    List<Type> emptyModules = new List<Type>();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Space(25);
                        GUILayout.BeginVertical();
                        {
                            GUILayout.Space(2.5f);
                            DrawExistingModules(modules, emptyModules);
                            DrawAddModules(pair.Value, emptyModules);
                        }
                        GUILayout.EndVertical();
                    }
                    GUILayout.EndHorizontal();
                }
                
                GUILayout.Space(2.5f);
                DrawLine();
            }

            GUILayout.Space(2.5f);
            if (GUILayout.Button("Add Ui Style"))
            {
                styleScriptableObject.AddStyle($"New Ui Style {styleScriptableObject.styles.Count}");
            }
        }
        
        
        void DrawExistingModules(Dictionary<Type, CmsModuleData> modules, List<Type> emptyModules)
        {
            IEnumerable<Type> exporters = typeof(CmsModuleData)
                                               .Assembly.GetTypes()
                                               .Where(t => t.IsSubclassOf(typeof(CmsModuleData)) && !t.IsAbstract)
                                               .Select(t => Activator.CreateInstance(t).GetType());

            foreach (Type cmsModule in exporters)
            {
                if (!modules.ContainsKey(cmsModule))
                {
                    emptyModules.Add(cmsModule);
                    continue;
                }

                CmsModuleData module = modules[cmsModule];
                GUILayout.BeginHorizontal();
                {
                    module.foldout = EditorGUILayout.Foldout(module.foldout, module.ToString());

                    if (GUILayout.Button("X", SmallButton))
                    {
                        modules.Remove(cmsModule);
                        return;
                    }
                }
                GUILayout.EndHorizontal();

                if (module.foldout)
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Space(25);
                        GUILayout.BeginVertical();
                        {
                            GUILayout.Space(2.5f);
                            module.DrawEditor();
                        }
                        GUILayout.EndVertical();
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Space(2.5f);
                }
            }
        }
        
        void DrawAddModules(CmsStyle style, List<Type> emptyModules)
        {
            List<string> avaiableModules = new List<string>();
            if (emptyModules.Count == 0)
            {
                return;
            }

            avaiableModules.Add("Select Module Type");
            avaiableModules.AddRange(emptyModules.Select(emptyModule => emptyModule.ToString()));
            int selected = EditorGUILayout.Popup(0, avaiableModules.ToArray());

            if (selected > 0)
            {
                string selectedModule = avaiableModules[selected];
                style.TryAddModule(Helpers.GetType(selectedModule));
            }
        }
        
        static void DrawLine(float height = 1)
        {
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(height));
        }
    }
}