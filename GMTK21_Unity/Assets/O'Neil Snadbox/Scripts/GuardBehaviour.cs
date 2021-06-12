using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardBehaviour : MonoBehaviour
{
    public enum AIStates
    {
        patroling,
        Chasing,
        Searching,
        Returning,

    }
    AIStates aiStates;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (aiStates)
        {
            case AIStates.patroling:

                break;
            case AIStates.Chasing:

                break;
            case AIStates.Searching:

                break;
            case AIStates.Returning:

                break;
        }
    }
}
