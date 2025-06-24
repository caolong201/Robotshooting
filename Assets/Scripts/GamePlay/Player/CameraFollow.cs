using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public DynamicJoystick cameraJoystick; // Joystick
    public float rotationSpeed = 10f;

    private float xRotation = 0f;
    Vector2 input = Vector2.zero;
    private Vector3 currentVelocity;

    void Update()
    {
        if(GameManager.Instance.CurrentGameStatus != EGameStatus.Live) return;
        
        input = new Vector3(cameraJoystick.Horizontal, cameraJoystick.Vertical).normalized;
        transform.Rotate(
            -input.y * rotationSpeed * Time.deltaTime, // X-axis
            input.x * rotationSpeed * Time.deltaTime, // Y-axis
            0, // Z-axis locked
            Space.Self // Rotate in local space
        );
        
        xRotation = transform.eulerAngles.x;
        if (xRotation > 180f) xRotation -= 360f;
        xRotation = Mathf.Clamp(xRotation, -30f, 30f);
        transform.eulerAngles = new Vector3(xRotation, transform.eulerAngles.y, 0);
    }
}