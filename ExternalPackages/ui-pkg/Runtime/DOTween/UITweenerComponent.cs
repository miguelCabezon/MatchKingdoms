using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JackSParrot.UI.DOTween
{
    public class UITweenerComponent : MonoBehaviour
    {
        [SerializeField]
        List<UITweenData> _data = null;
        [SerializeField]
        bool _showOnEnable = true;
        [SerializeField]
        UnityEvent _onComplete = new UnityEvent();
        UITweener _tweener = new UITweener();

        private int _current = 0;

        void OnEnable()
        {
            if (_showOnEnable)
            {
                Play();
            }
        }

        void OnDisable()
        {
            Stop();
        }

        public void Stop()
        {
            _current = 0;
            _tweener.Stop();
        }

        public void Play()
        {
            Play(null);
        }

        public void Play(Action callback)
        {
            if (_data.Count < 1)
                return;
            _current = 0;
            _tweener.Play(_data[_current], () => { PlayNext(callback); });
        }

        private void PlayNext(Action callback)
        {
            _current++;
            if (_current == _data.Count)
            {
                _onComplete?.Invoke();
                callback?.Invoke();
            }
            else
            {
                _tweener.Play(_data[_current], () => { PlayNext(callback); });
            }
        }
    }
}