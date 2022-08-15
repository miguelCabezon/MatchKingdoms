using System;
using System.Diagnostics.Tracing;
using UnityEngine;

public static class LevelEvents
{
    public static event Action<int> OnScoreUpdated = delegate (int score) { };
    public static event Action OnLevelCompleted = delegate { };
    public static event Action OnLevelFailed = delegate { };

    public static void NotifyOnUpdateScore(int scoreDelta)
    {
        OnScoreUpdated?.Invoke(scoreDelta);
    }

    public static void NotifyOnLevelCompleted()
    {
        OnLevelCompleted?.Invoke();
    }
    public static void NotifyOnLevelFailed()
    {
        OnLevelFailed?.Invoke();
    }
}