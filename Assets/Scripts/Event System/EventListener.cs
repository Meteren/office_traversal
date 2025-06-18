using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class EventListener : MonoBehaviour
{
    public delegate void Events();
    public event Events events;
    public EventController eventController;

    public void AddEvent(Events singleEvent) => events += singleEvent; 
    public void RemoveEvent(Events singleEvent) => events -= singleEvent;

    public void CallEvents() => events?.Invoke();
    private void OnEnable() => eventController.AddListener(this);

    private void OnDisable() => eventController.RemoveListener(this);

    

}
