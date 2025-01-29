using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // visible variables
    [Header("Camera Settings")]
    public float CameraSpeed = 25f;
    public float RotationSpeed = 25f;
    public float BobbingSpeed = 4f;
    public float BobbingLength = 0.3f;

    // hidden variables
    private float initialY = 7.0f;
    private float bobbingTimer = 0f;
    private float rotationY = 0f;

    void Start()
    {
        transform.position = new Vector3(transform.position.x, initialY, transform.position.z);
    }

    void Update()
    {
        Vector3 moveDirection = Vector3.zero;

        // Forward & Backward movement (along local Z-axis)
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += transform.forward; 
        } 
        else if (Input.GetKey(KeyCode.S))
        {
            moveDirection -= transform.forward;
        }

        // Left & Right movement (along local X-axis)
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection -= transform.right;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDirection += transform.right;
        }

        // Apply movement with speed
        transform.position += moveDirection * CameraSpeed * Time.deltaTime;
        // Bobbing animation (up/down)
        bobbingTimer += Time.deltaTime * BobbingSpeed; // Increment timer based on deltaTime and bobbing speed
        float bobbingOffset = Mathf.Sin(bobbingTimer) * BobbingLength; // Use sine wave for smooth oscillation
        Vector3 currentPosition = transform.position;
        currentPosition.y = initialY + bobbingOffset; // Apply the bobbing offset to the Y position

        transform.position = currentPosition;

        // Left/Right Rotation (Y-axis)
        if (Input.GetKey(KeyCode.Q))
        {
            rotationY -= RotationSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rotationY += RotationSpeed * Time.deltaTime;
        }

        // Apply rotation
        transform.rotation = Quaternion.Euler(0, rotationY, 0);
    }
}
