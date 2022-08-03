using System;
using System.Collections.Generic;

namespace JackSParrot.Utils
{
    public static class SharedServices
    {
        private static Dictionary<Type, IDisposable> _services = new Dictionary<Type, IDisposable>();

        public static void RegisterService<T>(T service, bool overwrite = false) where T : IDisposable
        {
            Type type = typeof(T);
            if (_services.ContainsKey(type))
            {
                if (overwrite)
                {
                    UnregisterService<T>();
                }
                else
                {
                    UnityEngine.Debug.LogError("Tried to add an already existing service to the service locator: " +
                                               type.Name);
                    return;
                }
            }

            _services.Add(type, service);
        }

        public static void RegisterService<T>() where T : IDisposable, new()
        {
            RegisterService(new T());
        }

        public static bool HasService<T>() where T : IDisposable
        {
            return _services.ContainsKey(typeof(T));
        }

        public static T GetService<T>() where T : IDisposable
        {
            IDisposable service;
            if (!_services.TryGetValue(typeof(T), out service))
            {
                UnityEngine.Debug.LogWarning("Tried to get a non registered service from the service locator: " +
                                             typeof(T).Name);
                return default;
            }

            return (T)service;
        }

        public static void UnregisterService<T>()
        {
            Type type = typeof(T);
            if (_services.ContainsKey(type))
            {
                _services[type].Dispose();
                _services.Remove(type);
            }
        }

        public static void UnregisterAll()
        {
            foreach (KeyValuePair<Type, IDisposable> service in _services)
            {
                try
                {
                    service.Value.Dispose();
                }
                catch (System.Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }

            _services.Clear();
        }
    }
}