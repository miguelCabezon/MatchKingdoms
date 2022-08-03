using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class AudioClipData
{
    public string ClipId;

    public AssetReferenceT<AudioClip> ReferencedClip;
    [Range(0f, 1f)]
    public float Volume = 1f;
    [Range(.3f, 3f)]
    public float Pitch = 1f;
    public bool Loop        = false;
    public bool AutoRelease = false;
}