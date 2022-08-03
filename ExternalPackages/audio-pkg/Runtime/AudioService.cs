using System;
using UnityEngine;
using UnityEngine.Audio;

#if JACKSPARROT_UTILS_AUDIO
using JackSParrot.Utils;
#endif

namespace JackSParrot.Audio
{
    public class AudioService : IDisposable
    {
        private float            _volume = 1f;
        private MusicPlayer      _musicPlayer;
        private SfxPlayer        _sfxPlayer;
        private AudioClipsStorer _clips;
        private AudioMixerGroup  _masterGroup;
        private UpdateRunner     _updateRunner;

        public MusicPlayer Music => _musicPlayer;
        public SfxPlayer Sfx => _sfxPlayer;

        public AudioService(AudioClipsStorer storer, float volume = 1f, float sfxVolume = 1f, float musicVolume = 1f)
        {
            _clips = storer;
            _clips.AudioService = this;
            _updateRunner = new GameObject("AudioServiceUpdater", typeof(UpdateRunner)).GetComponent<UpdateRunner>();
            UnityEngine.Object.DontDestroyOnLoad(_updateRunner);
            _updateRunner.OnDestroyed = GameQuit;

            AudioMixer mixer = storer.OutputMixer;
            AudioMixerGroup musicGroup = mixer.FindMatchingGroups("Music")[0];
            AudioMixerGroup sfxGroup = mixer.FindMatchingGroups("SFX")[0];

            _masterGroup = mixer.FindMatchingGroups("Master")[0];
            _sfxPlayer = new SfxPlayer(storer, sfxGroup, _updateRunner);
            _musicPlayer = new MusicPlayer(storer, musicGroup, _updateRunner);
            Volume = volume;
            SfxVolume = sfxVolume;
            MusicVolume = musicVolume;
#if JACKSPARROT_UTILS_AUDIO
            if (!SharedServices.HasService<AudioService>())
            {
                SharedServices.RegisterService(this);
            }
#endif
        }

        public void GameQuit()
        {
#if JACKSPARROT_UTILS_AUDIO
            SharedServices.UnregisterService<AudioService>();
#endif
            Dispose();
        }

        public void Dispose()
        {
            _clips.AudioService = null;
            _sfxPlayer.Dispose();
            _musicPlayer.Dispose();
            if (_updateRunner != null)
            {
                UnityEngine.Object.Destroy(_updateRunner);
            }
        }

        public float Volume
        {
            get => _volume;
            set
            {
                _volume = Mathf.Clamp(value, 0.001f, 1f);
                _masterGroup.audioMixer.SetFloat("masterVolume", Mathf.Log10(_volume) * 20f);
            }
        }

        public float MusicVolume
        {
            get => _musicPlayer.Volume;
            set => _musicPlayer.Volume = value;
        }

        public float SfxVolume
        {
            get => _sfxPlayer.Volume;
            set => _sfxPlayer.Volume = value;
        }

        public void PlayMusic(ClipId clipId, float fadeInSeconds = 0f) => _musicPlayer.Play(clipId, fadeInSeconds);
        public void StopMusic(float fadeOutSeconds = 0f) => _musicPlayer.Stop(fadeOutSeconds);

        public void CrossFadeMusic(ClipId clipId, float duration = 0.3f) => _musicPlayer.CrossFade(clipId, duration);

        public int PlaySfx(ClipId clipId) => _sfxPlayer.Play(clipId);

        public int PlaySfx(ClipId clipId, Transform toFollow) => _sfxPlayer.Play(clipId, toFollow);

        public int PlaySfx(ClipId clipId, Vector3 at) => _sfxPlayer.Play(clipId, at);

        public void StopPlayingSfx(int id) => _sfxPlayer.StopPlaying(id);

        public void LoadClipsForCategory(string categoryId) => _clips.LoadClipsForCategory(categoryId);
        public void UnloadClipsForCategory(string categoryId) => _clips.UnloadClipsForCategory(categoryId);

        internal class UpdateRunner : MonoBehaviour
        {
            public Action<float> OnUpdate    = t => { };
            public Action        OnDestroyed = () => { };
            private void Update() => OnUpdate(Time.deltaTime);

            private void OnDestroy() => OnDestroyed();
        }
    }
}