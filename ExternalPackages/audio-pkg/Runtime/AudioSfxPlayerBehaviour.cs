using UnityEngine;

namespace JackSParrot.Audio
{
    public class AudioSfxPlayerBehaviour : AAudioPlayerBehaviour
    {
        [Header("SFX settings")]
        [SerializeField]
        private bool PlayAtThisPosition = false;
        [SerializeField]
        private bool PlayFollowingThis = false;

        protected int _playingClip = -1;

        protected override void DoPlay()
        {
            if (PlayFollowingThis)
            {
                _playingClip = _audioService.PlaySfx(_clipId, transform);
                return;
            }

            if (PlayAtThisPosition)
            {
                _playingClip = _audioService.PlaySfx(_clipId, transform.position);
                return;
            }

            _playingClip = _audioService.PlaySfx(_clipId);
        }

        public override void Stop()
        {
            if (_playingClip >= 0)
            {
                _audioService.StopPlayingSfx(_playingClip);
                _playingClip = -1;
            }
        }
    }
}