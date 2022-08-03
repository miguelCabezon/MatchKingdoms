using UnityEditor;

namespace JackSParrot.Audio.Editor
{
    [InitializeOnLoad]
    public class EditorMenuItems
    {
        static EditorMenuItems()
        {
            EditorUtils.GetOrCreateAudioClipsStorer();
        }

        [MenuItem("Tools/JackSParrot/Select Audio Clips Storer")]
        public static void SelectAudioClipsStorer()
        {
            Selection.activeObject = EditorUtils.GetOrCreateAudioClipsStorer();
        }
    }
}