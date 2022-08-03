using System;

namespace JackSParrot.Utils
{
    public interface IUpdatable
    {
        void UpdateDelta(float deltaTime);
    }

    public interface IUpdateScheduler : IDisposable
    {
        void ScheduleUpdate(IUpdatable updatable);
        void UnscheduleUpdate(IUpdatable updatable);
    }
}

