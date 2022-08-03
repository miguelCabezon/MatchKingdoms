using System.Collections.Generic;

[System.Serializable]
public class AudioCategory
{
    public string              Id;
    public List<AudioClipData> Clips = new List<AudioClipData>();
}