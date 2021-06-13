
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//Inherits from GameEventListener and can be used to do the same thing on a delayed timer
public class GameEventListenerWithDelay : GameEventListener
{

    [SerializeField] float delay = 1f;
    [SerializeField] UnityEvent delayedUnityEvent;

    public override void RaiseEvent()
    {
        unityEvent.Invoke();
        StartCoroutine(RunDelayedEvent());
    }

    private IEnumerator RunDelayedEvent()
    {
        yield return new WaitForSeconds(delay);
        delayedUnityEvent.Invoke();
    }
}
