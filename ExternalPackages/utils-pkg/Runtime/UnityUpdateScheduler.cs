using System;
using System.Collections.Generic;
using UnityEngine;

namespace JackSParrot.Utils
{
    public class UnityUpdateScheduler : IUpdateScheduler
    {
        class Updater : MonoBehaviour
        {
            public event Action<float> OnUpdate;
            void Update()
            {
                OnUpdate?.Invoke(Time.deltaTime);
            }
        }

        List<IUpdatable> _registeredUpdatables = new List<IUpdatable>();
        List<IUpdatable> _updatablesToAdd = new List<IUpdatable>();
        List<IUpdatable> _updatablesToRemove = new List<IUpdatable>();
        Updater _updater = null;

        public void ScheduleUpdate(IUpdatable updatable)
        {
            CheckUpdater();
            if (!_updatablesToAdd.Contains(updatable) && !_registeredUpdatables.Contains(updatable))
            {
                _updatablesToAdd.Add(updatable);
            }
        }

        public void UnscheduleUpdate(IUpdatable updatable)
        {
            if (!_updatablesToRemove.Contains(updatable))
            {
                _updatablesToRemove.Add(updatable);
            }
        }

        public void Dispose()
        {
            if (_updater != null)
            {
                _updater.OnUpdate -= Update;
                UnityEngine.Object.Destroy(_updater.gameObject);
                _updater = null;
            }
            _registeredUpdatables.Clear();
            _updatablesToAdd.Clear();
            _updatablesToRemove.Clear();
        }

        void Update(float dt)
        {
            for (int i = 0; i < _updatablesToRemove.Count; ++i)
            {
                IUpdatable current = _updatablesToRemove[i];
                if (_registeredUpdatables.Contains(current))
                {
                    _registeredUpdatables.Remove(current);
                }
            }
            _updatablesToRemove.Clear();
            for (int i = 0; i < _updatablesToAdd.Count; ++i)
            {
                _registeredUpdatables.Add(_updatablesToAdd[i]);
            }
            _updatablesToAdd.Clear();
            for (int i = 0; i < _registeredUpdatables.Count; ++i)
            {
                _registeredUpdatables[i].UpdateDelta(dt);
            }
        }

        void CheckUpdater()
        {
            if (_updater == null)
            {
                _updater = new GameObject("UpdateRunner").AddComponent<Updater>();
                UnityEngine.Object.DontDestroyOnLoad(_updater.gameObject);
                _updater.OnUpdate += Update;
            }
        }
    }
}
