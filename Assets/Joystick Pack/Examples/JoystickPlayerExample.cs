using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
    public float speed;
    public VariableJoystick variableJoystick;
    public Rigidbody rb;

    public void FixedUpdate()
    {
        if(variableJoystick.Horizontal == 0&& variableJoystick.Vertical == 0) return;
        float desiredAngle = Mathf.Atan2(variableJoystick.Vertical, -variableJoystick.Horizontal) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, desiredAngle, 0);
        Debug.Log(variableJoystick.Horizontal + " # "+ variableJoystick.Vertical);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * speed);
    }
}