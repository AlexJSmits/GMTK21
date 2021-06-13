using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public void OnPlayerDeath()
    {
        Debug.Log("you died");
        Destroy(this);
    }
}
