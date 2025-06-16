
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
   
    [Header("Combat Settings")]
    public Gun GunPlayer;
    private float maxHealth = 100f;
    private float currentHealth;



    private Rigidbody rb;
    private Animator animator;

  
    private PlayerState currentState;
    
    [SerializeField] VariableJoystick cameraJoystick;
    public float rotationSpeed = 10f;
    private float rotationX = 0f;
    private float rotationY = 0f;
    public float minX = -45f;
    public float maxX = 45f;
   
    private enum PlayerState
    {
        MoveForward
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        currentHealth = maxHealth;

        SetState(PlayerState.MoveForward);
    }

    private void Update()
    {
        //HandleCameraRotation();
    }

    private void HandleCameraRotation()
    {
        if (cameraJoystick == null) return;

        Vector3 input = new Vector3(cameraJoystick.Horizontal, cameraJoystick.Vertical, 0);

        if (input.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(input.normalized);
            targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, 0);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
            
            //transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, 0);
        }
    }

   
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player Died!");
        GunPlayer.enabled = false; // Dừng bắn
        Time.timeScale = 0; // Dừng game
    }

    private void SetState(PlayerState newState)
    {
        currentState = newState;
        // animator.SetBool("isMoveForward", false);
        //
        // switch (newState)
        // {
        //     case PlayerState.MoveForward:
        //         animator.SetBool("isMoveForward", true);
        //         animator.speed = 0.8f;
        //         break;
        // }
    }
}
