using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Animator movementAnimator;
    public NavMeshAgent agent;
    public GameEvent playerDeath;
    public GameObject player;

    void Update()
    {
        //transform.LookAt(target.transform.position);
        if ((transform.position - player.transform.position).magnitude > 1.5f)
        {
            agent.SetDestination(player.transform.position);
            movementAnimator.SetFloat("Move", 1f);
        }
        else //if ((transform.position - player.transform.position).magnitude < 1.5f)
        {
            agent.SetDestination(transform.position);
            //playerDeath.Invoke();

            movementAnimator.SetFloat("Move", 0f);
            movementAnimator.Play("Attack");

        }
    }
}
