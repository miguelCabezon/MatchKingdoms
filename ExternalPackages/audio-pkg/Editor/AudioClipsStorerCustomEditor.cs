using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace JackSParrot.Audio
{
    [CustomEditor(typeof(AudioClipsStorer))]
    public class AudioClipsStorerCustomEditor : UnityEditor.Editor
    {
        private AudioClipsStorer _storer;
        private List<string>     _duplicatedCategories = new List<string>();
        private List<string>     _duplicatedClips      = new List<string>();

        private ReorderableList _list;
        private List<string>    _expandedClips           = new List<string>();
        private List<string>    _expandedCategories      = new List<string>();
        private List<string>    _expandedCategoriesClips = new List<string>();

        private void OnEnable()
        {
            _storer = (AudioClipsStorer)target;
            _list = new ReorderableList(serializedObject, serializedObject.FindProperty("_categories"), true, true,
                true, true);
            _list.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "Categories");
            _list.drawElementCallback = DrawElementCallback;
            _list.elementHeightCallback = ElementHeightCallback;
            _list.onAddCallback = OnAddCallback;
        }

        private void OnAddCallback(ReorderableList list)
        {
            SerializedProperty categoriesPropertyy = serializedObject.FindProperty("_categories");
            categoriesPropertyy.InsertArrayElementAtIndex(Mathf.Max(0, categoriesPropertyy.arraySize - 1));
            SerializedProperty categoryProperty =
                categoriesPropertyy.GetArrayElementAtIndex(categoriesPropertyy.arraySize - 1);
            categoryProperty.FindPropertyRelative("Id").stringValue =
                $"new Cateogry ({categoriesPropertyy.arraySize - 1})";
            categoryProperty.FindPropertyRelative("Clips").arraySize = 0;
            serializedObject.ApplyModifiedProperties();
        }

        private float ElementHeightCallback(int index)
        {
            if (index >= _storer.Categories.Count)
                return 0;

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float retVal = lineHeight;
            var category = _storer.Categories[index];
            if (!_expandedCategories.Contains(category.Id))
            {
                return retVal;
            }

            retVal += lineHeight;
            retVal += lineHeight;
            if (!_expandedCategoriesClips.Contains(category.Id))
            {
                return retVal;
            }

            retVal += lineHeight;

            foreach (var audioClipData in category.Clips)
            {
                retVal += _expandedClips.Contains(audioClipData.ClipId) ? 7 * lineHeight : lineHeight;
            }

            return retVal;
        }

        private void DrawElementCallback(Rect rect, int index, bool isactive, bool isfocused)
        {
            float lineHeight = EditorGUIUtility.singleLineHeight;

            SerializedProperty category = _list.serializedProperty.GetArrayElementAtIndex(index);
            rect.x += 10f;
            rect.width -= 10f;
            rect.height = lineHeight;
            SerializedProperty categoryIdProperty = category.FindPropertyRelative("Id");
            bool foldout = EditorGUI.Foldout(rect, _expandedCategories.Contains(categoryIdProperty.stringValue),
                categoryIdProperty.stringValue);
            rect.y += lineHeight;
            if (!foldout)
            {
                _expandedCategories.Remove(categoryIdProperty.stringValue);
                return;
            }

            if (!_expandedCategories.Contains(categoryIdProperty.stringValue))
            {
                _expandedCategories.Add(categoryIdProperty.stringValue);
            }

            string newId = EditorGUI.TextField(rect, "Id", categoryIdProperty.stringValue);
            if (newId != categoryIdProperty.stringValue)
            {
                _expandedCategories.Remove(categoryIdProperty.stringValue);
                _expandedCategoriesClips.Remove(categoryIdProperty.stringValue);
                _expandedCategories.Add(newId);
                _expandedCategoriesClips.Add(newId);
                categoryIdProperty.stringValue = newId;
            }

            rect.y += lineHeight;
            bool clipsFoldout = EditorGUI.Foldout(rect, _expandedCategoriesClips.Contains(newId), "Clips");
            if (!clipsFoldout)
            {
                _expandedCategoriesClips.Remove(newId);
                return;
            }

            if (!_expandedCategoriesClips.Contains(newId))
            {
                _expandedCategoriesClips.Add(newId);
            }

            SerializedProperty clips = category.FindPropertyRelative("Clips");
            rect.y += lineHeight;
            rect.x += 10f;
            rect.width -= 10f;
            var old = GUI.color;
            for (int i = 0; i < clips.arraySize; ++i)
            {
                SerializedProperty clip = clips.GetArrayElementAtIndex(i);
                SerializedProperty clipIdProperty = clip.FindPropertyRelative("ClipId");
                bool clipFoldout = EditorGUI.Foldout(new Rect(rect.x, rect.y, rect.width - 40f, rect.height),
                    _expandedClips.Contains(clipIdProperty.stringValue),
                    clipIdProperty.stringValue);
                GUI.color = Color.red;
                if (GUI.Button(new Rect(rect.width - 40f, rect.y, 40f, rect.height), "-"))
                {
                    //add item
                    clips.DeleteArrayElementAtIndex(i--);
                    continue;
                }

                GUI.color = i % 2 == 0 ? Color.black : Color.white;
                GUI.Box(new Rect(rect.x, rect.y, rect.width, lineHeight * (clipFoldout ? 7f : 1f)), "");
                GUI.color = old;
                rect.y += lineHeight;
                if (!clipFoldout)
                {
                    _expandedClips.Remove(clipIdProperty.stringValue);
                    continue;
                }

                EditorGUI.PropertyField(rect, clipIdProperty);
                rect.y += lineHeight;
                EditorGUI.PropertyField(rect, clip.FindPropertyRelative("ReferencedClip"));
                rect.y += lineHeight;
                EditorGUI.PropertyField(rect, clip.FindPropertyRelative("Volume"));
                rect.y += lineHeight;
                EditorGUI.PropertyField(rect, clip.FindPropertyRelative("Pitch"));
                rect.y += lineHeight;
                EditorGUI.PropertyField(rect, clip.FindPropertyRelative("Loop"));
                rect.y += lineHeight;
                EditorGUI.PropertyField(rect, clip.FindPropertyRelative("AutoRelease"));
                rect.y += lineHeight;
                if (!_expandedClips.Contains(clipIdProperty.stringValue))
                {
                    _expandedClips.Add(clipIdProperty.stringValue);
                }
            }

            GUI.color = Color.green;
            if (GUI.Button(rect, "+"))
            {
                clips.InsertArrayElementAtIndex(Mathf.Max(0, clips.arraySize - 1));
                SerializedProperty newClip = clips.GetArrayElementAtIndex(clips.arraySize - 1);
                newClip.FindPropertyRelative("ClipId").stringValue = $"new clip ({clips.arraySize - 1})";
                newClip.FindPropertyRelative("Volume").floatValue = 1f;
                newClip.FindPropertyRelative("Pitch").floatValue = 1f;
                newClip.FindPropertyRelative("Loop").boolValue = false;
                newClip.FindPropertyRelative("AutoRelease").boolValue = true;
            }

            GUI.color = old;
            rect.y += lineHeight;
        }

        public override void OnInspectorGUI()
        {
            if (_duplicatedCategories.Count > 0)
            {
                EditorGUILayout.HelpBox($"There is a duplicated category id: {_duplicatedCategories[0]}",
                    MessageType.Error);
            }

            if (_duplicatedClips.Count > 0)
            {
                EditorGUILayout.HelpBox($"There is a duplicated clip id: {_duplicatedClips[0]}", MessageType.Error);
            }

            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("OutputMixer"));
            _list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();


            var categories = new List<string>();
            var clips = new List<string>();
            _duplicatedCategories.Clear();
            _duplicatedClips.Clear();
            foreach (AudioCategory storerCategory in _storer.Categories)
            {
                if (categories.Contains(storerCategory.Id))
                {
                    _duplicatedCategories.Add(storerCategory.Id);
                }
                else
                {
                    categories.Add(storerCategory.Id);
                }

                foreach (AudioClipData clip in storerCategory.Clips)
                {
                    if (clips.Contains(clip.ClipId))
                    {
                        _duplicatedClips.Add(clip.ClipId);
                    }
                    else
                    {
                        clips.Add(clip.ClipId);
                    }
                }
            }
        }
    }
}