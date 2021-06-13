using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IsometricCharacterController : MonoBehaviour
{

    public GameObject ball;
    public GameObject ballPrimer;
    public Transform cam;
    public Transform camPivot;
    public Transform animationRigTarget;
    [Space]
    public float speed = 5;
    public float rotationSpeed = 1;
    public float turnSmoothTime = 0.1f;
    public float throwforce = 1;

    [Space]
    public bool throwing;
    public bool inRange;
    public bool holding;

    [Space]
    public AudioClip[] ballNChain;

    [Space]
    public AudioSource playerNoise;
    public AudioSource ballNoise;

    private float turnSmoothVelocity;
    private CharacterController controller;
    private Ray ray;
    private RaycastHit hitInfo;
    private Animator animator;
    private Vector3 direction;



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Physics.IgnoreLayerCollision(8, 9, true);
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        camPivot.transform.position = transform.position;

        PlayerMovement();

        if (holding)
        {
            Grab();
        }

        if (Input.GetMouseButton(1) && !holding && !throwing)
        {
            holding = true;
            speed = 2;
        }

        //Holding Animation
        if (holding && inRange)
        {
            animator.SetFloat("HoldingSpeed", Mathf.MoveTowards(animator.GetFloat("HoldingSpeed"), 1, Time.deltaTime * 7));
        }
        else
        {
            animator.SetFloat("HoldingSpeed", Mathf.MoveTowards(animator.GetFloat("HoldingSpeed"), -1, Time.deltaTime * 7));
        }

        //Walking Animation
        if (direction.magnitude > 0.1f)
        {
            animator.SetFloat("WalkSpeed", Mathf.MoveTowards(animator.GetFloat("WalkSpeed"), 1, Time.deltaTime * 7));
        }
        else
        {
            animator.SetFloat("WalkSpeed", Mathf.MoveTowards(animator.GetFloat("WalkSpeed"), -1, Time.deltaTime * 7));
        }
        
    }

    void PlayerMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

            if (!playerNoise.isPlaying)
            {
                playerNoise.Play();
            }

            if (!ballNoise.isPlaying)
            {
                ballNoise.clip = ballNChain[0];
                ballNoise.Play();
            }
        }


        if (Input.GetKey(KeyCode.E))
        {
            camPivot.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            camPivot.transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
        }

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore))
        {
            animationRigTarget.position = hitInfo.point + new Vector3(0, 1, 0);
        }

    }

    void Grab()
    {
        if (inRange)
        {

            ball.GetComponent<Rigidbody>().isKinematic = true;
            ball.transform.position = Vector3.Lerp(ball.transform.position, ballPrimer.transform.position, Time.deltaTime * 10);

            ballNoise.clip = ballNChain[1];
            ballNoise.Play();

            if (Input.GetMouseButtonUp(1))
            {
                Drop();
            }

            if (Input.GetMouseButtonDown(0))
            {
                Throw();
            }
        }
        else
        {
            speed = 5;
            holding = false;
        }

    }

    void Drop()
    {
        ballNoise.clip = ballNChain[1];
        ballNoise.Play();

        ball.GetComponent<Rigidbody>().isKinematic = false;
        speed = 5;
        holding = false;
    }

    void Throw()
    {
        ballNoise.clip = ballNChain[2];
        ballNoise.Play();

        throwing = true;
        speed = 5;
        holding = false;
        ball.GetComponent<Rigidbody>().isKinematic = false;
        ball.GetComponent<Rigidbody>().AddForce(ballPrimer.transform.forward * throwforce, ForceMode.Impulse);
        Invoke("HoldDelay", 1);
    }

    void HoldDelay()
    {
        throwing = false;
    }

    public void PlayerDeath()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
