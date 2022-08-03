using System;
using UnityEngine;
using UnityEngine.Audio;

namespace JackSParrot.Audio
{
    public class AudioClipHandler : MonoBehaviour
    {
        [NonSerialized]
        public int Id = -1;
        [NonSerialized]
        public AudioClipData Data = null;

        public bool IsAlive => _elapsed < _duration || _looping;

        Transform    _toFollow = null;
        Transform    _transform;
        AudioSource  _source            = null;
        float        _elapsed           = 0f;
        float        _duration          = 0f;
        bool         _looping           = false;
        private bool _isFollowingTarget = false;

        void Awake()
        {
            _transform = transform;
            if (_source == null)
            {
                _source = gameObject.AddComponent<AudioSource>();
            }
        }

        public void ResetHandler()
        {
            Data = null;
            _source.Stop();
            _looping = false;
            _elapsed = 0f;
            _duration = 0f;
            _toFollow = null;
            _isFollowingTarget = false;
            _transform.localPosition = Vector3.zero;
            gameObject.SetActive(false);
            Id = -1;
        }

        public void SetOutput(AudioMixerGroup mixerGroup)
        {
            _source.outputAudioMixerGroup = mixerGroup;
        }

        public void UpdateHandler(float deltaTime)
        {
            _elapsed += deltaTime;
            if (!_isFollowingTarget)
            {
                return;
            }

            if (_toFollow != null)
            {
                _transform.position = _toFollow.position;
                return;
            }

            _elapsed = _duration;
            _looping = false;
            _source.Stop();
        }

        public void Play(AudioClipData data)
        {
            Data = data;
            _duration = 9999f;
            _source.spatialBlend = 0f;
            gameObject.name = Data.ClipId;
            _source.volume = Data.Volume;
            _source.pitch = Data.Pitch;
            _source.loop = Data.Loop;
            _looping = Data.Loop;
            
            if (data.ReferencedClip.Asset != null)
            {
                OnLoaded(data.ReferencedClip.Asset as AudioClip);
                return;
            }

            if (data.ReferencedClip.OperationHandle.IsValid())
            {
                if (data.ReferencedClip.OperationHandle.IsDone)
                {
                    OnLoaded(data.ReferencedClip.OperationHandle.Result as AudioClip);
                    return;
                }

                data.ReferencedClip.OperationHandle.Completed += h => OnLoaded(h.Result as AudioClip);

                return;
            }

            data.ReferencedClip.LoadAssetAsync<AudioClip>().Completed += h => OnLoaded(h.Result);
        }

        void OnLoaded(AudioClip clip)
        {
            if (clip == null)
            {
                Debug.Assert(false);
                return;
            }

            gameObject.SetActive(true);
            _source.clip = clip;
            _source.Play();
            _toFollow = null;
            _duration = clip.length;
        }

        public void Play(AudioClipData data, Vector3 position)
        {
            Play(data);
            _source.spatialBlend = 1f;
            _transform.position = position;
        }

        public void Play(AudioClipData data, Transform parent)
        {
            Play(data);
            _source.spatialBlend = 1f;
            _transform.position = parent.position;
            _toFollow = parent;
            _isFollowingTarget = true;
        }
    }
}