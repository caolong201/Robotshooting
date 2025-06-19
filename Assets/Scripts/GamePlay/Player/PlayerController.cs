using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    private bool isDead = false;
    private void Start()
    {
        currentHealth = maxHealth;
        isDead = false;
        UIManager.Instance.SetHealthSliderMax(maxHealth);
        UIManager.Instance.UpdateHealthSlider(currentHealth);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        UIManager.Instance.UpdateHealthSlider(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

        UIManager.Instance.OnTakeDamage();

        if (currentHealth / maxHealth < 0.2f)
        {
            UIManager.Instance.OnHPLow();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Player Died!");
        UIManager.Instance.ShowEndGame(false);
    }
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;

        UIManager.Instance.SetHealthSliderMax(maxHealth);
        UIManager.Instance.UpdateHealthSlider(currentHealth);
    }

}


