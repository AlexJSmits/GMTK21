
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventTriggerExample : MonoBehaviour
{

    [SerializeField] GameEvent onEventTriggered; //This needs to be a Game Event SO (scriptable object) that is created and dragged into the inspector

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            onEventTriggered?.Invoke(); //This sends a message (saying to trigger the event) to any GameEventListener that is listening for the same event
        }
    }
}