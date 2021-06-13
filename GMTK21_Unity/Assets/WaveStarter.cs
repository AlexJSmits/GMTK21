using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveStarter : MonoBehaviour
{
    public GameObject waveSpawner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            waveSpawner.SetActive(true);

        }
    }




}
