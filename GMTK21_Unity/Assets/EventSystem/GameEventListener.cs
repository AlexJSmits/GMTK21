
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] protected GameEvent gameEvent;
    [SerializeField] protected UnityEvent unityEvent;

    void Awake()
    {
        gameEvent.Register(this); //Adds the event to the HashSet list so that it can be raised later
    }
    void OnDestroy() //if the object is destroyed, remove the event from HashSet list so that it can't be triggered again
    {
        gameEvent.Deregister(this);
    }
    public virtual void RaiseEvent() //triggers the event
    {
        unityEvent.Invoke();
    }
}
