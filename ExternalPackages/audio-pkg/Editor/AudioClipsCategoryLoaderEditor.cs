using System.Collections.Generic;
using JackSParrot.Audio.Editor;
using UnityEditor;
using UnityEngine;

namespace JackSParrot.Audio
{
    [CustomEditor(typeof(AudioClipsCategoryLoader))]
    public class AudioClipsCategoryLoaderEditor : UnityEditor.Editor
    {
        private List<string>             _categoryIds = new List<string>();
        private AudioClipsCategoryLoader _castedTarget;
        private int                      _chosen  = 0;
        private bool                     _foldOut = true;

        private void OnEnable()
        {
            _castedTarget = (AudioClipsCategoryLoader) target;
            AudioClipsStorer audioClipsStorer = EditorUtils.GetOrCreateAudioClipsStorer();
            foreach (AudioCategory category in audioClipsStorer.Categories)
            {
                _categoryIds.Add(category.Id);
            }

            foreach (string category in _castedTarget.CategoriesToLoad)
            {
                _categoryIds.Remove(category);
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_clipsStorer"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_loadOnEnable"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_unloadOnDisable"));
            _foldOut = EditorGUILayout.BeginFoldoutHeaderGroup(_foldOut, "Categories");
            SerializedProperty categories = serializedObject.FindProperty("_categoriesToLoad");
            Color originalColor = GUI.color;
            if (_foldOut)
            {
                EditorGUI.indentLevel++;
                if (categories.arraySize < 1)
                {
                    EditorGUILayout.LabelField("EMPTY");
                }

                for (int i = 0; i < categories.arraySize; ++i)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(categories.GetArrayElementAtIndex(i).stringValue);
                    EditorGUILayout.Space();
                    GUI.color = Color.red;
                    if (GUILayout.Button("-"))
                    {
                        _categoryIds.Add(categories.GetArrayElementAtIndex(i).stringValue);
                        categories.DeleteArrayElementAtIndex(i--);
                    }

                    GUI.color = originalColor;
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            if (_categoryIds.Count > 0)
            {
                EditorGUILayout.LabelField("Add New Category", EditorStyles.boldLabel);
                EditorGUILayout.BeginHorizontal();
                _chosen = EditorGUILayout.Popup(_chosen, _categoryIds.ToArray());
                GUI.color = Color.green;
                if (GUILayout.Button("+"))
                {
                    categories.InsertArrayElementAtIndex(Mathf.Max(0, categories.arraySize - 1));
                    categories.GetArrayElementAtIndex(categories.arraySize - 1).stringValue = _categoryIds[_chosen];
                    _categoryIds.Remove(_categoryIds[_chosen]);
                }

                GUI.color = originalColor;
                EditorGUILayout.EndHorizontal();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}