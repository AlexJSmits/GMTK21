using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public GameObject destroyed;

    private AudioSource audioSource;
    private GameObject player;
    
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball") && player.GetComponent<IsometricCharacterController>().throwing)
        { 
            var replacement = Instantiate(destroyed, transform.position, transform.rotation, this.gameObject.transform.parent);
            audioSource = replacement.GetComponent<AudioSource>();
            audioSource.Play();
            Destroy(this.gameObject);
        }
    }
}
