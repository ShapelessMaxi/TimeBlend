using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public float CameraSpeed = 50f;
    public float RotationSpeed = 50f; // Rotation speed multiplier

    private float rotationY = 0f;

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
