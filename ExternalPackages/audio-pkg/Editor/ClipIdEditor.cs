using JackSParrot.Audio.Editor;
using UnityEditor;
using UnityEngine;

namespace JackSParrot.Audio
{
    [CustomPropertyDrawer(typeof(ClipId))]
    public class ClipIdEditor : PropertyDrawer
    {
        private AudioClipsStorer _clips;
        private string[]         _ids;
        int                      _selected = 0;

        private void Initialize(string currentValue)
        {
            _clips = EditorUtils.GetOrCreateAudioClipsStorer();
            var allClips = _clips.GetAllClips().ToArray();
            _ids = new string[allClips.Length + 1];
            _ids[0] = "NONE";
            for (int i = 0; i < allClips.Length; ++i)
            {
                _ids[i + 1] = allClips[i];
            }

            _selected = 0;
            if (!string.IsNullOrEmpty(currentValue))
            {
                for (int i = 0; i < _ids.Length; ++i)
                {
                    if (_ids[i] == currentValue)
                    {
                        _selected = i;
                        i = 99999;
                    }
                }
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty clipId = property.FindPropertyRelative("Id");
            if (_clips == null)
            {
                Initialize(clipId.stringValue);
            }

            int selected = EditorGUI.Popup(EditorGUI.PrefixLabel(position, label), _selected, _ids);

            if (selected != _selected)
            {
                _selected = selected;
                clipId.stringValue = selected == 0 ? string.Empty : _ids[selected];
            }
        }
    }
}