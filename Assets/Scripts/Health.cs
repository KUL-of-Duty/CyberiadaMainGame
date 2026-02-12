using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    
    public HpBar healthBar; 
    public string damageTag = "Enemy"; 
    public int damageAmount = 20;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null) healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        // Odtwarzamy dźwięk przy otrzymaniu obrażeń
        SoundManager.PlaySound(SoundType.TAKINGDAMAGE);

        if (healthBar != null) healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0) Die();
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(damageTag)) 
        {
            TakeDamage(damageAmount);
        }
    }

    void Die() 
    {
        Debug.Log("Nie chciałbym cię martwić, ale już jesteś martwy");
        // Tu można dodać SoundManager.PlaySound(jakiś_dźwięk_śmierci);
    }
}