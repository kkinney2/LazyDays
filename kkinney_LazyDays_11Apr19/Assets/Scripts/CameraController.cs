using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float MouseSensitivity = 0.5f;
    public float RotAcc = 0.12f;
    public float CamSpeed = 5.0f;
    public float CamSpeedMultiplier = 3f;
    public Vector2 pitchMinMax = new Vector2(-30, 85);
    public bool InvertPitch = false;
    

    Rigidbody rb;
    float horizontalMove;
    float verticalMove;
    float yaw;
    float pitch;
    Vector3 moveDir = Vector3.zero;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody>();

        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
        currentRotation = transform.eulerAngles;

        MouseSensitivity = MouseSensitivity / 5f;
    }
	
	// Update is called once per frame
	void Update () {

        Movement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Rotation();
    }

    void Movement(float horizontalInput, float verticalInput)
    {
        if (horizontalInput > 0)
        {
            moveDir.x = 1;
        }
        else if (horizontalInput < 0)
        {
            moveDir.x = -1;
        }
        else
        {
            moveDir.x = 0;
        } 

        if (verticalInput > 0)
        {
            moveDir.z = 1;
        }
        else if (verticalInput < 0)
        {
            moveDir.z = -1;
        }
        else
        {
            moveDir.z = 0;
        }

        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Space))
        {
            // 'Up' Check
            moveDir.y = 1;
        }
        else if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftControl))
        {
            // 'Down' Check
            moveDir.y = -1;
        }
        else
        {
            moveDir.y = 0;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveDir = moveDir * CamSpeed * CamSpeedMultiplier;
        }
        else
        {
            moveDir = moveDir * CamSpeed;
        }
        
        moveDir = transform.TransformDirection(moveDir);
        rb.transform.position = transform.position + moveDir * Time.deltaTime;

    }

    void Rotation()
    {
        yaw += Input.GetAxis("Mouse X") * MouseSensitivity;

        if (InvertPitch)
            pitch += Input.GetAxis("Mouse Y") * MouseSensitivity;
        else
            pitch += Input.GetAxis("Mouse Y") * -MouseSensitivity;

        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, RotAcc);

        transform.eulerAngles = currentRotation;
    }
}
