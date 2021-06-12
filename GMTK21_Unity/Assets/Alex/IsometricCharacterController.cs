using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCharacterController : MonoBehaviour
{
    private CharacterController controller;

    public float speed = 6;
    public float rotationSpeed = 1;


    public Transform cam;
    public Transform camPivot;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    bool kicking;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
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
        }

        if (Input.GetMouseButtonUp(0))
        {
            kicking = false;
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

    }

    void Kick()
    {

    }
}
