using UnityEngine;

namespace JackSParrot.Utils
{
    public class UnityLogger : ICustomLogger
    {
        LogLevel _level = LogLevel.Debug;

        public UnityLogger(LogLevel level)
        {
            SetLogLevel(level);
        }

        public void SetLogLevel(LogLevel level)
        {
            _level = level;
        }

        public void LogDebug(string message)
        {
            if(_level == LogLevel.Debug)
            {
                Debug.Log(message);
            }
        }

        public void LogError(string message)
        {
             Debug.LogError(message);
        }

        public void LogWarning(string message)
        {
            if (_level == LogLevel.Warning || _level == LogLevel.Error)
            {
                Debug.LogWarning(message);
            }
        }

        public void Dispose()
        {

        }
    }
}