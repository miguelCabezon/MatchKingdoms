using UnityEngine;

namespace JackSParrot.Audio
{
    public class AudioMusicPlayerBehaviour : AAudioPlayerBehaviour
    {
        [Header("Music settings")]
        [SerializeField]
        private float _changeMusicCrossfadeSeconds = 0f;
        [SerializeField]
        private float _fadeInSeconds = 0f;
        [SerializeField]
        private float _fadeOutSeconds = 0f;

        private bool  _playing = false;
        private float _delay   = 0f;

        protected override void DoPlay()
        {
            _playing = true;
            if (_audioService?.Music?.PlayingClipId == "")
            {
                _delay = _fadeInSeconds;
                _audioService?.PlayMusic(_clipId, _fadeInSeconds);
                return;
            }

            _delay = _changeMusicCrossfadeSeconds;
            _audioService?.CrossFadeMusic(_clipId, _changeMusicCrossfadeSeconds);
        }

        public override void Stop()
        {
            if (_audioService?.Music.PlayingClipId == _clipId)
            {
                _audioService?.StopMusic(_fadeOutSeconds);
            }

            _playing = false;
        }

        private void Update()
        {
            _delay -= Time.deltaTime;
            if (_delay > 0f)
            {
                return;
            }

            if (_playing && _audioService?.Music?.PlayingClipId != _clipId)
            {
                gameObject.SetActive(false);
                _playing = false;
            }
        }
    }
}