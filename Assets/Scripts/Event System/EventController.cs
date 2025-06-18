using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "EventController")]
public class EventController : ScriptableObject
{
    List<EventListener> listeners = new List<EventListener>();

    public void AddListener(EventListener listener) => listeners.Add(listener);
    public void RemoveListener(EventListener listener) => listeners.Remove(listener);

    public void ExecuteListeners()
    {
        foreach (var listener in listeners)
            listener.CallEvents();

    }
}


