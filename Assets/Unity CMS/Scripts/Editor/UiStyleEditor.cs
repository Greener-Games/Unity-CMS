#region

using System;
using System.Collections.Generic;
using System.Linq;
using GG;
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

        static UiStyleEditor _window;

        static UiStyleEditor Window
        {
            get
            {
                if (_window == null)
                {
                    _window = GetWindow<UiStyleEditor>(true, "Style Editor", true);
                    _window.minSize = new Vector2(1000, 400);
                }

                return _window;
            }
            set => _window = value;
        }

        static GUIStyle SmallButton => _mSmallButton ?? (_mSmallButton = new GUIStyle(EditorStyles.miniButton) {fixedWidth = 100});
        static GUIStyle _mSmallButton;
        
        static GUIStyle Foldout => _mFoldout ?? (_mFoldout = new GUIStyle(EditorStyles.foldout) {fixedWidth = 100});
        static GUIStyle _mFoldout;

        int selectedIndex;
        List<CmsScriptableObject> styleObjects = new List<CmsScriptableObject>();

        [MenuItem("Dev Test/Style Sheet", false, 1)]
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
            GG.SecondaryKeyDictionary<string, string, CmsData> temp = new GG.SecondaryKeyDictionary<string, string, CmsData>(styleScriptableObject.uiStyles);
            foreach (KeyValuePair<string, CmsData> keyValuePair in temp.primaryDictionary)
            {
                string entryKey;
                string oldKey = temp.GetSecondaryKey(keyValuePair.Key);
                EditorGUI.BeginChangeCheck();
                {
                    GUILayout.BeginHorizontal();
                    {
                        keyValuePair.Value.foldout = EditorGUILayout.Foldout(keyValuePair.Value.foldout, "Key", Foldout);
                        entryKey = EditorGUILayout.TextField(oldKey);
                        
                        if (GUILayout.Button("X", SmallButton))
                        {
                            styleScriptableObject.uiStyles.RemoveUsingSecondary(oldKey);
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                if (EditorGUI.EndChangeCheck())
                {
                    if (!styleScriptableObject.uiStyles.secondaryKeyLink.ContainsKey(entryKey))
                    {
                        styleScriptableObject.uiStyles.secondaryKeyLink.ChangeKey(oldKey, entryKey);
                    }
                }

                if (keyValuePair.Value.foldout)
                {
                    Dictionary<StyleType, CmsModule> modules = keyValuePair.Value.modules;
                    List<StyleType> emptyModules = new List<StyleType>();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Space(25);
                        GUILayout.BeginVertical();
                        {
                            GUILayout.Space(2.5f);
                            DrawExistingModules(modules, emptyModules);
                            DrawAddModules(keyValuePair.Value, emptyModules);
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
                string key = Guid.NewGuid().ToString();
                styleScriptableObject.AddStyle(key,key);
            }
        }
        
        
        void DrawExistingModules(Dictionary<StyleType, CmsModule> modules, List<StyleType> emptyModules)
        {
            foreach (StyleType styleType in (StyleType[]) Enum.GetValues(typeof(StyleType)))
            {
                if (!modules.ContainsKey(styleType))
                {
                    emptyModules.Add(styleType);
                    continue;
                }
                
                GUILayout.BeginHorizontal();
                {
                    modules[styleType].foldout = EditorGUILayout.Foldout(modules[styleType].foldout, styleType.GetDescription());

                    if (GUILayout.Button("X", SmallButton))
                    {
                        modules.Remove(styleType);
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Space(25);
                    GUILayout.BeginVertical();
                    {
                        GUILayout.Space(2.5f);
                        if (modules.ContainsKey(styleType) && modules[styleType].foldout)
                        {
                            switch (styleType)
                            {
                                case StyleType.IMAGE:
                                {
                                    if (modules[styleType] is ImageCmsModule mod)
                                    {
                                        mod.color = EditorGUILayout.ColorField("Color", mod.color);
                                    }
                                    break;
                                }
                                case StyleType.FONT:
                                {
                                    if (modules[styleType] is TextMeshCmsModule mod)
                                    {
                                        mod.color = EditorGUILayout.ColorField("Color", mod.color);
                                        mod.font = EditorGUILayout.ObjectField("Font", mod.font, typeof(TMP_FontAsset), false) as TMP_FontAsset;
                                        mod.fontSize = EditorGUILayout.FloatField("Font Size", mod.fontSize);
                                    }
                                    break;
                                }
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(2.5f);
            }
        }
        
        void DrawAddModules(CmsData style, List<StyleType> emptyModules)
        {
            List<string> avaiableModules = new List<string>();
            emptyModules.Remove(StyleType.ERROR); //we never want to show error
            if (emptyModules.Count == 0)
            {
                return;
            }

            avaiableModules.Add("Select Module Type");
            avaiableModules.AddRange(emptyModules.Select(emptyModule => emptyModule.GetDescription()));
            int selected = EditorGUILayout.Popup(0, avaiableModules.ToArray());

            if (selected > 0)
            {
                string selectedModule = avaiableModules[selected];
                StyleType styleType = EnumExtensions.GetEnumValueFromDescription<StyleType>(selectedModule);
                style.AddStyle(styleType);
            }
        }

        static void DrawLine(float height = 1)
        {
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(height));
        }
    }
}