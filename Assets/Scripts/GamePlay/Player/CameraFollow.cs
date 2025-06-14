using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Player cần follow
    public VariableJoystick cameraJoystick; // Joystick để xoay camera
    public float rotationSpeed = 10f;
    public float startYRotation = -50f; // Góc Y muốn setup khi start
    private float rotationX = 0f;
    public float minX = -45f;
    public float maxX = 45f;

    private void Start()
    {
        // Setup góc Y ban đầu
        player.rotation = Quaternion.Euler(0f, startYRotation, 0f);

        // Reset camera gốc (cameraHolder)
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void Update()
    {
        HandleCameraRotation();
    }

    private void HandleCameraRotation()
    {
        float horizontal = cameraJoystick.Horizontal;
        float vertical = cameraJoystick.Vertical;

        // Xoay player theo trục Y
        player.Rotate(0f, horizontal * rotationSpeed, 0f);

        // Xoay camera theo trục X
        rotationX -= vertical * rotationSpeed;
        rotationX = Mathf.Clamp(rotationX, minX, maxX);

        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

    }
}
