using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.UI;
using static VSX.UniversalVehicleCombat.GunTurret;

public class PlayerController : MonoBehaviour
{
    public VariableJoystick moveJoystick;     // Joystick di chuyển
    public VariableJoystick cameraJoystick;   // Joystick tầm ngắm

    [Header("Player Settings")]
    public Transform cameraHolder;
    public Transform cameraTransform;
    public float rotationSpeed = 10f;
    public float moveSpeed = 0.5f;
    public float smoothTime = 0.1f;
    public float startYRotation = -50f;

    [Header("Aim Settings")]
    public RectTransform crosshairUI;
    public Transform gunPivot;
    public Camera mainCamera;
    public float aimSpeed = 200f;
    public float targetDistance = 9000f;
    public float rotateSpeed = 4f;

    [Header("Combat Settings")]
    public Gun GunPlayer;
    public Slider healthSlider;
    private float maxHealth = 100f;
    private float currentHealth;


    public GameObject losePanel;

    private Rigidbody rb;
    private Animator animator;

  
    private PlayerState currentState;
   
    private enum PlayerState
    {
        MoveForward
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        losePanel.SetActive(false);

        SetState(PlayerState.MoveForward);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Update()
    {
        MoveCrosshair();
        AimGun();
    }

    private void MovePlayer()
    {
        Vector3 direction = new Vector3(-moveJoystick.Vertical, 0, moveJoystick.Horizontal);

        if (direction.magnitude >= 0.1f)
        {
            Vector3 move = direction.normalized * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);
        }
    }

    private void MoveCrosshair()
    {
        Vector2 input = new Vector2(cameraJoystick.Horizontal, cameraJoystick.Vertical);
        crosshairUI.anchoredPosition += input * aimSpeed * Time.deltaTime;

        // Giới hạn vùng di chuyển tầm ngắm
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
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthSlider.value = currentHealth;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player Died!");
        GunPlayer.enabled = false; // Dừng bắn
        losePanel.SetActive(true); // Hiện UI thua
        Time.timeScale = 0; // Dừng game
    }

    private void SetState(PlayerState newState)
    {
        currentState = newState;
        animator.SetBool("isMoveForward", false);

        switch (newState)
        {
            case PlayerState.MoveForward:
                animator.SetBool("isMoveForward", true);
                animator.speed = 0.8f;
                break;
        }
    }
}
