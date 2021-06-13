using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BallController : MonoBehaviour
{
    public float speed = 6;
    public float chainSnapForce = 1;

    private GameObject player;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreLayerCollision(8, 9, true);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance > agent.stoppingDistance && agent.enabled == true)
        {
            agent.SetDestination(player.transform.position);
        }
    }
}
