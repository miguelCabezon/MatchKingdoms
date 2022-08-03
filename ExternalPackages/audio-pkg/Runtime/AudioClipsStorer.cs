using System;
using System.Collections.Generic;
using JackSParrot.Utils;
using UnityEngine;
using UnityEngine.Audio;

namespace JackSParrot.Audio
{
    [CreateAssetMenu(fileName = "ClipStorer", menuName = "Audio/ClipStorer", order = 1)]
    public class AudioClipsStorer : ScriptableObject
    {
        [NonSerialized]
        private AudioService _audioService;

        public AudioService AudioService
        {
            get => _audioService ??= new AudioService(this);
            set => _audioService = value;
        }

        public AudioMixer OutputMixer = null;

        [SerializeField]
        List<AudioCategory> _categories = new List<AudioCategory>();

        public IReadOnlyList<AudioCategory> Categories => _categories;

        public List<string> GetAllClips()
        {
            var retVal = new List<string>();
            foreach (var category in _categories)
            {
                foreach (var clip in category.Clips)
                {
                    retVal.Add(clip.ClipId);
                }
            }

            return retVal;
        }

        public AudioClipData GetClipById(ClipId clipId)
        {
            if (!clipId.IsValid())
                return null;

            foreach (var category in _categories)
            {
                foreach (var clip in category.Clips)
                {
                    if (clip.ClipId == clipId)
                    {
                        return clip;
                    }
                }
            }

            return null;
        }

        public void LoadClipsForCategory(string categoryId)
        {
            var category = GetCategoryById(categoryId);
            if (category == null)
            {
                Debug.Assert(false);
                return;
            }

            foreach (var clip in category.Clips)
            {
                if (clip.ReferencedClip.IsValid() && clip.ReferencedClip.Asset == null && !clip.ReferencedClip.IsDone)
                {
                    clip.ReferencedClip.LoadAssetAsync<AudioClip>();
                }
            }
        }

        public void UnloadClipsForCategory(string categoryId)
        {
            var category = GetCategoryById(categoryId);
            if (category == null)
            {
                Debug.Assert(false);
                return;
            }

            foreach (var clip in category.Clips)
            {
                if (clip.ReferencedClip.IsValid() && clip.ReferencedClip.IsDone)
                {
                    clip.ReferencedClip.ReleaseAsset();
                }
            }
        }

        public AudioCategory GetCategoryById(string categoryId)
        {
            foreach (var category in _categories)
            {
                if (categoryId.Equals(category.Id))
                {
                    return category;
                }
            }

            return null;
        }

        public void Dispose()
        {
            _audioService = null;
        }
    }
}