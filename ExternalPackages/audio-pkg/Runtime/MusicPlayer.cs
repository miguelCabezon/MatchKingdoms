using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Audio;

namespace JackSParrot.Audio
{
    public class MusicPlayer : IDisposable
    {
        float _volume = 1f;
        public float Volume
        {
            get { return _volume; }
            set
            {
                _volume = Mathf.Clamp(value, 0.0001f, 1f);
                _outputMixerGroup.audioMixer.SetFloat("musicVolume", Mathf.Log10(_volume) * 20f);
            }
        }

        public string PlayingClipId => _playingClip?.ClipId ?? "";

        private AudioClipData             _playingClip      = null;
        private AudioSource               _source           = null;
        private AudioClipsStorer          _clipStorer       = null;
        private AudioService.UpdateRunner _updateRunner     = null;
        private AudioMixerGroup           _outputMixerGroup = null;

        internal MusicPlayer(AudioClipsStorer clipStorer, AudioMixerGroup outputGroup,
            AudioService.UpdateRunner updateRunner)
        {
            _updateRunner = updateRunner;

            _outputMixerGroup = outputGroup;
            _clipStorer = clipStorer;
            _source = CreateSource();
        }

        private AudioSource CreateSource()
        {
            AudioSource retVal = new GameObject("MusicPlayer").AddComponent<AudioSource>();
            UnityEngine.Object.DontDestroyOnLoad(retVal.gameObject);
            retVal.playOnAwake = false;
            retVal.spatialBlend = 0f;
            retVal.outputAudioMixerGroup = _outputMixerGroup;
            return retVal;
        }

        private void Play(ClipId clipId)
        {
            if (!clipId.IsValid())
                return;

            if (_playingClip != null && clipId == _playingClip.ClipId && _source.isPlaying)
            {
                return;
            }

            if (_playingClip != null)
            {
                _source.Stop();
                _playingClip.ReferencedClip.ReleaseAsset();
                _playingClip = null;
            }

            AudioClipData clip = _clipStorer.GetClipById(clipId);
            if (clip == null)
            {
                Debug.LogError("Clip not found: " + clipId.Id);
                return;
            }

            _playingClip = clip;
            clip.ReferencedClip.LoadAssetAsync<AudioClip>().Completed += h => OnClipLoaded(h.Result);
        }

        public void Play(ClipId clipId, float fadeInSeconds)
        {
            if (!clipId.IsValid())
                return;

            Play(clipId);
            _updateRunner.StopAllCoroutines();
            _updateRunner.StartCoroutine(FadeInCoroutine(fadeInSeconds));
        }

        public void Stop(float fadeOutSeconds)
        {
            if (_updateRunner == null)
            {
                return;
            }

            _updateRunner.StopAllCoroutines();
            _updateRunner.StartCoroutine(FadeOutCoroutine(fadeOutSeconds));
        }

        void OnClipLoaded(AudioClip clip)
        {
            if (clip == null)
            {
                Debug.Assert(false);
                return;
            }

            _source.clip = clip;
            _source.loop = _playingClip.Loop;
            _source.pitch = _playingClip.Pitch;
            _source.volume = _playingClip.Volume;
            _source.Play();
        }

        public void CrossFade(ClipId clipId, float duration)
        {
            if (_updateRunner == null || !clipId.IsValid())
            {
                return;
            }

            _updateRunner.StopAllCoroutines();
            if (_playingClip != null)
            {
                _updateRunner.StartCoroutine(CrossFadeCoroutine(clipId, duration));
            }
            else
            {
                Play(clipId);
                _updateRunner.StartCoroutine(FadeInCoroutine(duration));
            }
        }

        IEnumerator CrossFadeCoroutine(ClipId fadeTo, float duration)
        {
            if (_source == null)
            {
                yield break;
            }

            float halfDuration = duration * 0.5f;
            while (!_source.isPlaying)
            {
                yield return null;
            }

            _updateRunner.StartCoroutine(FadeOutCoroutine(halfDuration));
            yield return new WaitForSeconds(halfDuration);
            Play(fadeTo);
            _updateRunner.StartCoroutine(FadeInCoroutine(halfDuration));
        }

        IEnumerator FadeOutCoroutine(float duration)
        {
            if (_source == null)
            {
                yield break;
            }

            while (!_source.isPlaying)
            {
                yield return null;
            }

            float remaining = duration;
            _source.volume = 0f;
            while (remaining > 0f)
            {
                _source.volume = _playingClip.Volume * remaining / duration;
                yield return null;
                remaining -= Time.deltaTime;
            }

            _source.Stop();
            _playingClip.ReferencedClip.ReleaseAsset();
            _playingClip = null;
        }

        IEnumerator FadeInCoroutine(float duration)
        {
            if (_source == null)
            {
                yield break;
            }

            while (!_source.isPlaying)
            {
                yield return null;
            }

            float remaining = duration;
            while (remaining > 0)
            {
                _source.volume = _playingClip.Volume * (1f - (remaining / duration));
                yield return null;
                remaining -= Time.deltaTime;
            }
        }

        public void Dispose()
        {
            if (_playingClip != null)
            {
                if (_playingClip.ReferencedClip.Asset != null)
                {
                    _playingClip.ReferencedClip.ReleaseAsset();
                }
            }

            if (_updateRunner == null || _source == null)
            {
                return;
            }

            _source.Stop();
            _updateRunner.StopAllCoroutines();
            UnityEngine.Object.Destroy(_source.gameObject);
        }
    }
}