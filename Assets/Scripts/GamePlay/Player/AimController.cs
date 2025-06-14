using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AimController : MonoBehaviour
{
    public VariableJoystick aimJoystick;       // Joystick tầm ngắm
    public RectTransform crosshairUI;          // UI tầm ngắm
    public Transform gunPivot;                 // Gốc xoay súng
    public Camera mainCamera;
    public float aimSpeed = 200f;
    public float targetDistance = 9000f;          // Khoảng cách đẩy tầm ngắm ra xa
    public float rotateSpeed = 4f;
    private void Update()
    {
        MoveCrosshair();
        AimGun();
    }
    private void MoveCrosshair()
    {
        Vector2 input = new Vector2(aimJoystick.Horizontal, aimJoystick.Vertical);
        crosshairUI.anchoredPosition += input * aimSpeed * Time.deltaTime;

        // Giới hạn vùng di chuyển nếu cần
        crosshairUI.anchoredPosition = new Vector2(
            Mathf.Clamp(crosshairUI.anchoredPosition.x, -400, 400),
            Mathf.Clamp(crosshairUI.anchoredPosition.y, -200, 200)
        );
    }
    private void AimGun()
    {
        Vector3 screenPosition = crosshairUI.position;
        Vector3 worldPoint = mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, targetDistance));     
        Vector3 aimDirection = worldPoint - gunPivot.position;

        if (aimDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(aimDirection);
            gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        }
    }
}
