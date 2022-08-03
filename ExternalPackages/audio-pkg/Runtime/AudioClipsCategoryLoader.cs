using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using JackSParrot.Audio.Editor;
using UnityEditor;

#endif

namespace JackSParrot.Audio
{
    public class AudioClipsCategoryLoader : MonoBehaviour
    {
        [SerializeField]
        private AudioClipsStorer _clipsStorer;

        [SerializeField]
        private List<string> _categoriesToLoad = new List<string>();

        [SerializeField]
        private bool _loadOnEnable = true;

        [SerializeField]
        private bool _unloadOnDisable = true;

        public IReadOnlyList<string> CategoriesToLoad => _categoriesToLoad;

        private bool _loaded = false;

        public void Load()
        {
            if (_loaded)
            {
                return;
            }

            foreach (string category in _categoriesToLoad)
            {
                _clipsStorer.LoadClipsForCategory(category);
            }

            _loaded = true;
        }

        public void Unload()
        {
            if (!_loaded)
            {
                return;
            }

            foreach (string category in _categoriesToLoad)
            {
                _clipsStorer.UnloadClipsForCategory(category);
            }

            _loaded = false;
        }

        private void OnEnable()
        {
            if (_loadOnEnable)
            {
                Load();
            }
        }

        private void OnDisable()
        {
            if (_unloadOnDisable)
            {
                Unload();
            }
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            _clipsStorer = EditorUtils.GetOrCreateAudioClipsStorer();
            EditorUtility.SetDirty(gameObject);
#endif
        }
    }
}