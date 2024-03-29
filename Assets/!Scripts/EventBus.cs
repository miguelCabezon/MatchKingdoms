using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Match 3 / Event")]
public class EventBus : ScriptableObject //& Event Bus base class
{
    public event Action Event = delegate { };

    public void NotifyEvent() => Event?.Invoke();
    
}
