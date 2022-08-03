using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine.Audio;

namespace JackSParrot.Audio
{
    public class SfxPlayer : IDisposable
    {
        float _volume = 1f;
        public float Volume
        {
            get => _volume;
            set
            {
                _volume = Mathf.Clamp(value, 0.0001f, 1f);
                _outputMixerGroup.audioMixer.SetFloat("sfxVolume", Mathf.Log10(_volume) * 20f);
            }
        }

        int                               _idGenerator      = 0;
        AudioClipsStorer                  _clipStorer       = null;
        AudioMixerGroup                   _outputMixerGroup = null;
        List<AudioClipHandler>            _handlers         = new List<AudioClipHandler>();
        private AudioService.UpdateRunner _updateRunner     = null;
        Dictionary<AudioClipData, int>    _loadedClips      = new Dictionary<AudioClipData, int>();


        internal SfxPlayer(AudioClipsStorer clipsStorer, AudioMixerGroup outputGroup,
            AudioService.UpdateRunner updateRunner)
        {
            _outputMixerGroup = outputGroup;
            _updateRunner = updateRunner;
            _updateRunner.OnUpdate = Update;
            _clipStorer = clipsStorer;
            for (int i = 0; i < 10; ++i)
            {
                CreateHandler();
            }
        }

        public void Update(float deltaTime)
        {
            foreach (var handler in _handlers)
            {
                if (handler.IsAlive)
                {
                    handler.UpdateHandler(deltaTime);
                    if (!handler.IsAlive)
                    {
                        StopPlaying(handler);
                    }
                }
            }
        }

        public void ReleaseReferenceCache()
        {
            foreach (var kvp in _loadedClips)
            {
                if (kvp.Value < 1)
                {
                    if (kvp.Key.ReferencedClip.Asset != null && kvp.Key.AutoRelease)
                    {
                        kvp.Key.ReferencedClip.ReleaseAsset();
                    }
                }
            }
        }

        AudioClipHandler CreateHandler()
        {
            var newHandler = new GameObject("sfx_handler").AddComponent<AudioClipHandler>();
            newHandler.ResetHandler();
            _handlers.Add(newHandler);
            newHandler.Id = _idGenerator++;
            newHandler.SetOutput(_outputMixerGroup);
            UnityEngine.Object.DontDestroyOnLoad(newHandler.gameObject);
            return newHandler;
        }

        AudioClipHandler GetFreeHandler()
        {
            foreach (var handler in _handlers)
            {
                if (!handler.IsAlive)
                {
                    handler.Id = _idGenerator++;
                    return handler;
                }
            }

            return CreateHandler();
        }

        AudioClipData GetClipToPlay(ClipId clipId)
        {
            foreach (var kvp in _loadedClips)
            {
                if (kvp.Key.ClipId == clipId)
                {
                    _loadedClips[kvp.Key] += 1;
                    return kvp.Key;
                }
            }

            var sfx = _clipStorer.GetClipById(clipId);
            if (sfx == null)
            {
                Debug.LogError("Cip not found: " + clipId.Id);
                return null;
            }

            _loadedClips.Add(sfx, 1);
            return sfx;
        }

        void ReleasePlayingClip(AudioClipData clip)
        {
            if (clip != null && _loadedClips.ContainsKey(clip))
            {
                _loadedClips[clip] = Mathf.Max(0, _loadedClips[clip]);
            }
        }

        public int Play(ClipId clipId)
        {
            if (!clipId.IsValid())
                return -1;

            var clip = GetClipToPlay(clipId);
            if (clip == null)
            {
                return -1;
            }

            var handler = GetFreeHandler();
            handler.Play(clip);
            return handler.Id;
        }

        public int Play(ClipId clipId, Transform toFollow)
        {
            if (!clipId.IsValid())
                return -1;

            var clip = GetClipToPlay(clipId);
            if (clip == null)
            {
                return -1;
            }

            var handler = GetFreeHandler();
            handler.Play(clip, toFollow);
            return handler.Id;
        }

        public int Play(ClipId clipId, Vector3 at)
        {
            if (!clipId.IsValid())
                return -1;

            var clip = GetClipToPlay(clipId);
            if (clip == null)
            {
                return -1;
            }

            var handler = GetFreeHandler();
            handler.Play(clip, at);
            return handler.Id;
        }

        public void StopPlaying(int id)
        {
            foreach (var handler in _handlers)
            {
                if (handler.Id == id)
                {
                    StopPlaying(handler);
                }
            }
        }

        void StopPlaying(AudioClipHandler handler)
        {
            ReleasePlayingClip(handler.Data);
            handler.ResetHandler();
        }

        public void Dispose()
        {
            if (_updateRunner != null)
            {
                _updateRunner.OnUpdate = t => { };
            }

            foreach (var handler in _handlers)
            {
                if (handler != null)
                {
                    UnityEngine.Object.Destroy(handler.gameObject);
                }
            }

            _handlers.Clear();
        }
    }
}