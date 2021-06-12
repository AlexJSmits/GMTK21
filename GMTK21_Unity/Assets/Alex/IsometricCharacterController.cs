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
    public float lineDistance = 3;
    public float turnSmoothTime = 0.1f;
    public float kickforce = 1;
    public float kickChargeTime = 1;


    private float currentKickCharge;
    private float turnSmoothVelocity;
    private bool kicking;
    private CharacterController controller;
    private LineRenderer line;
    private Ray ray;
    private RaycastHit hitInfo;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        line = GetComponent<LineRenderer>();
        Physics.IgnoreLayerCollision(8, 9, true);
    }

    // Update is called once per frame
    void Update()
    {
        camPivot.transform.position = transform.position;

        PlayerMovement();

        if (kicking)
        {
            Kick();
        }

        if (Input.GetMouseButtonDown(0))
        {
            kicking = true;
            line.enabled = true;
            currentKickCharge = kickChargeTime;
        }

    }

    void PlayerMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f && !kicking)
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

    }

    void Kick()
    {
        currentKickCharge -= Time.deltaTime;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        ball.GetComponent<Rigidbody>().isKinematic = true;
        ball.transform.position = Vector3.Lerp(ball.transform.position, ballPrimer.transform.position, Time.deltaTime * 5);
        
        if (Physics.Raycast(ray, out hitInfo))
        {
            //rotate player to mouse

            Vector3 direction = (hitInfo.point - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            //draw line to mouse (currently busted)
            Vector3 directionOfHitPoint = (hitInfo.point - transform.position).normalized * lineDistance;
            line.SetPositions(new[] { transform.position, directionOfHitPoint });

            
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (currentKickCharge <= 0)
            {
                ball.GetComponent<Rigidbody>().isKinematic = false;
                ball.GetComponent<Rigidbody>().AddForce((hitInfo.point - ball.transform.position).normalized * kickforce, ForceMode.Impulse);
                line.enabled = false;
                kicking = false;

            }
            else
            {
                ball.GetComponent<Rigidbody>().isKinematic = false;
                line.enabled = false;
                kicking = false;
            }
        }

    }
}
