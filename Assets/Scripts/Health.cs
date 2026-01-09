using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    
    public HpBar healthBar; // Przeciągnij tutaj obiekt z HpBar.cs
    
    public string damageTag = "Enemy"; // Obiekty z tym tagiem zadają obrażenia
    public int damageAmount = 20;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null) healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (healthBar != null) healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0) Die();
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        // Jeśli dotknie nas coś z tagiem "Enemy", otrzymujemy obrażenia
        if (collision.gameObject.CompareTag(damageTag)) TakeDamage(damageAmount);
    }

    void Die() => Debug.Log("nie chciałbym cię martwić ale już jesteś martwy");
}