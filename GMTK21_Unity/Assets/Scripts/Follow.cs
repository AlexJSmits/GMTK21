using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Follow : MonoBehaviour
{

    public float sightRadius;

    private GameObject player;
    private NavMeshAgent agent;
    private bool followPlayer;
    private float distanceFromPlayer;
    private RaycastHit hitInfo;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (Physics.SphereCast(transform.position, sightRadius, transform.forward, out hitInfo, sightRadius))
        {
            if (hitInfo.transform.CompareTag("Player"))
            {
                Debug.Log("In Range");
                followPlayer = true;
            }
        }

        if (followPlayer)
        {
            if (distanceFromPlayer > agent.stoppingDistance)
            {
                MoveToPlayer();
            }

            if (distanceFromPlayer <= agent.stoppingDistance)
            {
                RotateToPlayer();
            }
        }
    }

    void MoveToPlayer()
    {
        agent.SetDestination(player.transform.position);
    }

    void RotateToPlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }
}
