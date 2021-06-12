using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class IsometricCharacterController : MonoBehaviour
{

    public GameObject ball;
    public GameObject ballPrimer;
    public Transform cam;
    public Transform camPivot;
    [Space]
    public float speed = 6;
    public float rotationSpeed = 1;
    public float turnSmoothTime = 0.1f;
    public float throwforce = 1;

    [Space]
    public bool throwing;
    public bool inRange;
    public bool holding;
    
    private float turnSmoothVelocity;
    private CharacterController controller;
    private Ray ray;
    private RaycastHit hitInfo;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Physics.IgnoreLayerCollision(8, 9, true);
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

    }

    void PlayerMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
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
        if (Physics.Raycast(ray, out hitInfo))
        {
            //rotate player to mouse

            Vector3 mouseDirection = (hitInfo.point - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(mouseDirection.x, 0, mouseDirection.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);


        }

    }

    void Grab()
    {
        if (inRange)
        {

            ball.GetComponent<Rigidbody>().isKinematic = true;
            ball.transform.position = Vector3.Lerp(ball.transform.position, ballPrimer.transform.position, Time.deltaTime * 5);

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
            Invoke("HoldDelay", 1);
        }

    }

    void Drop()
    {
        ball.GetComponent<Rigidbody>().isKinematic = false;
        speed = 6;
        holding = false;
    }

    void Throw()
    {
        throwing = true;
        speed = 6;
        holding = false;
        ball.GetComponent<Rigidbody>().isKinematic = false;
        ball.GetComponent<Rigidbody>().AddForce((hitInfo.point - ball.transform.position).normalized * throwforce, ForceMode.Impulse);
        Invoke("HoldDelay", 1);
    }

    void HoldDelay()
    {
        throwing = false;
    }

}
