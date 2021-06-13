using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public void OnPlayerDeath()
    {
        //Time.timeScale = 0;
        Debug.Log("you died");
        //Destroy(this.gameObject);
    }
}
