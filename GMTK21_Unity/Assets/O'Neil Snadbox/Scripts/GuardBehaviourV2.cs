using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SensorToolkit;

public class GuardBehaviourV2 : MonoBehaviour
{
    //public Animator movementAnimator;
    public NavMeshAgent agent;

    public GameObject player;

    public GameObject playerLastPos;

    public GameObject alertSymbol;

    // public RangeSensor sensor;
    public TriggerSensor fov;
    public float chaseSpeed = 4f;

    //Patrol Vars
    public GameObject post;
    public Vector3[] patrolPoints;
    private int patrolPoint = 0;

    //for searching state
    public bool patrolingGuard = false;
    public float speed;
    public float timerDecrease = 1f;
    //public GameObject patrolPoint;

    //search timer vars
    public float searchTime;
    public float startSearchTime;

    public AudioClip alertedClip;
    public AudioClip returningToPostClip;
    public AudioClip idleChatterClip1;
    public AudioClip idleChatterClip2;
    AudioSource audioSource;
    bool alertPlaying = false;
    bool returnPlaying = false;
    bool idle1Playing = false;
    bool idle2Playing = false;
    public AudioSource alertedSound;

    public GameEvent playerDeath;


    public enum GameStates
    {
        patroling,
        chasing,
        searching,
        returningToPost,
    }
    GameStates gameState;
    private void Awake()
    {
        //agent.Warp(transform.position);
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartPatrol();
        gameState = GameStates.patroling;
        searchTime = startSearchTime;
        InvokeRepeating("PlayIdle1Audio", 20, 30);
        InvokeRepeating("PlayIdle2Audio", 5, 30);
    }

    void Update()
    {


        switch (gameState)
        {
            case GameStates.patroling:
                Patrol();
                Debug.Log("We are in state patroling!");
                //alertPlaying = false;
                //returnPlaying = false;
                //idle2Playing = false;

                //if (!idle1Playing)
                //{
                //    audioSource.PlayOneShot(idleChatterClip1);
                //    idle1Playing = true;
                //}

                //StartPatrol();
                break;
            case GameStates.chasing:
                Debug.Log("We are in state chasing!");
                returnPlaying = false;
                idle2Playing = false;
                idle1Playing = false;

                if (!alertPlaying)
                {
                    audioSource.PlayOneShot(alertedClip);
                    alertPlaying = true;
                }
                StopPatrol();
                Chasing();

                break;
            case GameStates.searching:
                //Debug.Log("We are in state searching!");
                StopPatrol();
                IsSearching();
                break;
            case GameStates.returningToPost:
                //Debug.Log("We are in state returningToPost!");
                idle2Playing = false;
                idle1Playing = false;
                alertPlaying = false;

                if (!returnPlaying)
                {
                    audioSource.PlayOneShot(returningToPostClip);
                    returnPlaying = true;
                }
                ReturnToPost();
                break;
            default:
                break;
        }

        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    gameState = GameStates.patroling;
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    gameState = GameStates.chasing;
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    gameState = GameStates.searching;
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    gameState = GameStates.returningToPost;
        //}
    }

    public void PlaySFXOneShot(AudioClip clip)
    {
        audioSource.PlayOneShot(alertedClip);
    }

    public void PlayTransitionAnimation(string animStateName)
    {
        //movementAnimator.Play(animStateName);
    }

    public void Patrol()
    {
        //agent.Resume();
        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[patrolPoint]);
            if (transform.position == patrolPoints[patrolPoint] || Vector3.Distance(transform.position, patrolPoints[patrolPoint]) < 0.2f)
            {
                patrolPoint++;    //use distance if needed(lower precision)
            }
            if (patrolPoint >= patrolPoints.Length)
            {
                patrolPoint = 0;

            }
        }
    }
    public void Chasing()
    {
        var deteced = fov.GetNearest();
        if (deteced != null )
        {
            Chase(deteced);
            alertSymbol.SetActive(true);
        }
    }
   
    void Chase(GameObject target)
    {
        transform.LookAt(target.transform.position);
        if ((transform.position - target.transform.position).magnitude > 1f)
        {
            agent.SetDestination(target.transform.position);
            //movementAnimator.SetFloat("Move", 1f);
        }
        if ((transform.position - target.transform.position).magnitude < 1f)
        {
            agent.SetDestination(transform.position);
            playerDeath?.Invoke();
            //movementAnimator.Play("Attack");
            //movementAnimator.SetFloat("Move", 0f);
        }
    }

    void ReturnToPost()
    {
        if ((transform.position - post.transform.position).magnitude > 2f)
        {
            agent.SetDestination(post.transform.position);
            //movementAnimator.SetFloat("Move", 0.5f);
        }
        else
        {
            gameState = GameStates.patroling;
        }
    }

    void IsSearching()
    {
        if ((transform.position - playerLastPos.transform.position).magnitude > 1f)
        {
            alertedSound.Play();
            //transform.LookAt(playerPos.playerLastPos, Vector3.up);
            //transform.position += transform.forward * speed * Time.deltaTime;
            agent.SetDestination(playerLastPos.transform.position);
        }
        else
        {
            Search();
        }

    }

    void Search()
    {
        if (searchTime >= 0)
        {
            //movementAnimator.Play("Searching");
            searchTime -= (timerDecrease * Time.deltaTime);
        }
        else
        {
            //movementAnimator.Play("Movement");
            gameState = GameStates.returningToPost;
            //guardModle.transform.localRotation = Quaternion.identity;
            //guardModle.transform.localPosition = new Vector3(0, 0, 0);
            searchTime = startSearchTime;
        }
    }

    public void StartPatrol()
    {
        //movementAnimator.SetFloat("Move", 0.5f);
        //guard.GetComponent<NavMeshAgent>().enabled = false;
    }
    public void StopPatrol()
    {
        //guard.GetComponent<PedestrianNavigationController>().enabled = false;
        //guard.GetComponent<WaypointNavigator>().enabled = false;
        //guard.GetComponent<NavMeshAgent>().enabled = true;
    }

    public void ChaseStateTransition()
    {
        gameState = GameStates.chasing;
    }

    public void SearchStateTransition()
    {
        gameState = GameStates.searching;
    }
    void PlayIdle1Audio()
    {
        audioSource.clip = idleChatterClip1;
        audioSource.Play();
    }
    void PlayIdle2Audio()
    {
        audioSource.clip = idleChatterClip2;
        audioSource.Play();
    }
}
