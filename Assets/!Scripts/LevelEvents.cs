using System;
using System.Diagnostics.Tracing;
using UnityEngine;

public static class LevelEvents //& EVENTS SCRIPTS (NOT KNOW HOW THIS WORKS EXACTLY RIGHT NOW)
{
    public static event Action<int> OnScoreUpdated = delegate (int score) { };
    public static event Action OnLevelCompleted = delegate { };
    public static event Action OnLevelFailed = delegate { };
    public static event Action OnMatchComplete = delegate { };
    public static event Action OnMovePerformed = delegate { };

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
    public static void NotifyOnMatchComplete()
    {
        OnMatchComplete?.Invoke();
    }
    public static void NotifyOnMovePerformed()
    {
        OnMovePerformed?.Invoke();
    } 
}