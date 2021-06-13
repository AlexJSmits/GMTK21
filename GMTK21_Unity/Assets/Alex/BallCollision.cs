using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
    IsometricCharacterController player;

    private void Start()
    {
        player = GetComponentInParent<IsometricCharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Ball"))
        {
            player.inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Ball") && player.holding == false)
        {
            player.inRange = false;
        }
    }
}
