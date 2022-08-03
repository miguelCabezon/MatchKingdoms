#if UNITY_EDITOR
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

[assembly: InternalsVisibleTo("JackSParrotAudio.Editor")]
namespace JackSParrot.Audio.Editor
{
    public static class EditorUtils
    {
        internal static AudioClipsStorer GetOrCreateAudioClipsStorer()
        {
            AudioClipsStorer retVal = null;
            var clips = AssetDatabase.FindAssets("t: AudioClipsStorer");
            if (clips.Length < 1)
            {
                if (!System.IO.Directory.Exists(Application.dataPath + "/JackSParrot"))
                {
                    System.IO.Directory.CreateDirectory(Application.dataPath + "/JackSParrot");
                }

                if (!System.IO.Directory.Exists(Application.dataPath + "/JackSParrot/Audio"))
                {
                    System.IO.Directory.CreateDirectory(Application.dataPath + "/JackSParrot/Audio");
                }

                if (!System.IO.Directory.Exists(Application.dataPath + "/JackSParrot/Audio/Resources"))
                {
                    System.IO.Directory.CreateDirectory(Application.dataPath + "/JackSParrot/Audio/Resources");
                }

                AudioMixer mixer = Resources.Load<AudioMixer>("JackSParrotMixer");

                AudioClipsStorer item = ScriptableObject.CreateInstance<AudioClipsStorer>();
                item.OutputMixer = mixer;
                AssetDatabase.CreateAsset(item, "Assets/JackSParrot/Audio/Resources/AudioClipStorer.asset");

                return item;
            }

            if (clips.Length > 1)
            {
                Debug.LogError("There is a duplicate AudioClipStorer");
            }

            AudioClipsStorer storer =
                AssetDatabase.LoadAssetAtPath<AudioClipsStorer>(AssetDatabase.GUIDToAssetPath(clips[0]));
            return storer;
        }
    }
}
#endif