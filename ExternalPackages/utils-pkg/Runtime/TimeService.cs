using System;
using UnityEngine;

namespace JackSParrot.Utils
{
    public interface ITimeService : IDisposable
    {
        void UpdateRealTime();
        DateTime Now { get; }
        ulong TimestampMillis { get; }
        long TimestampSeconds { get; }
        long DaysFromEpoch { get; }
        string FormatDateEurope(int timestampSeconds);
        string FormatDateEurope(DateTime dateTime);
        string FormatTime(int timeSeconds, string daysToken = "d", string hoursToken = "h", string minutesToken = "m", string secondsToken = "s");
        string FormatTimeShort(int timeSeconds, string separationToken = ":");
    }

    public class UnityTimeService : ITimeService
    {
        DateTime _epoch = new DateTime(1970, 1, 1);
        float _lastSeconds = 0f;
        DateTime _lastDate;

        public UnityTimeService()
        {
            UpdateRealTime();
			Application.focusChanged += focused =>
			{
				if (focused)
					UpdateRealTime();
			};
        }

        public void UpdateRealTime()
        {
            _lastSeconds = UnityEngine.Time.time;
            _lastDate = DateTime.Now;
        }

        public DateTime Now
        {
            get
            {
                float now = UnityEngine.Time.time;
                float diff = now - _lastSeconds;
                if(diff > 0f)
                {
                    _lastDate = _lastDate.AddSeconds(diff);
                    _lastSeconds = now;
                }
                return _lastDate;
            }
        }

        public ulong TimestampMillis => (ulong)Now.Subtract(_epoch).TotalMilliseconds;

        public long TimestampSeconds => (long)Now.Subtract(_epoch).TotalSeconds;

        public long DaysFromEpoch => (long)Now.Subtract(_epoch).TotalDays;
        
        
        public string FormatTimeShort(int timeSeconds, string separationToken = ":")
        {
            int time = timeSeconds;
            int hours = time / 3600;
            time -= hours * 3600;
            int minutes = time / 60;
            int seconds = time - minutes * 60;
            if (hours > 0)
            {
                return $"{hours.ToString()}{separationToken}{minutes.ToString()}{separationToken}{seconds.ToString()}";
            }
            return $"{minutes.ToString()}{separationToken}{seconds.ToString()}";
        }
        
        public string FormatTime(int timeSeconds, string daysToken = "d", string hoursToken = "h", string minutesToken = "m", string secondsToken = "s")
        {
            int time = timeSeconds;
            int days = timeSeconds / (3600 * 24);
            time -= days * (3600 * 24);
            int hours = time / 3600;
            time -= hours * 3600;
            int minutes = time / 60;
            int seconds = time - minutes * 60;
            if (days > 0)
            {
                return $"{days.ToString()}{daysToken} {hours.ToString()}{hoursToken}";
            }
            if (hours > 0)
            {
                return $"{hours.ToString()}{hoursToken} {minutes.ToString()}{minutesToken}";
            }
            return $"{minutes.ToString()}{minutesToken} {seconds.ToString()}{secondsToken}";
        }

        public string FormatDateEurope(int timestampSeconds)
        {
            var currentDate = _epoch.AddSeconds(timestampSeconds);
            return new System.Text.StringBuilder().Append(currentDate.Day).Append("/").Append(currentDate.Month).Append("/").Append(currentDate.Year).ToString();
        }

        public string FormatDateEurope(DateTime dateTime)
        {
            return new System.Text.StringBuilder().Append(dateTime.Day).Append("/").Append(dateTime.Month).Append("/").Append(dateTime.Year).ToString();
        }

        public void Dispose()
        {

        }
    }
}
