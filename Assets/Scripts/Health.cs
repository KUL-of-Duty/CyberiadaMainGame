using UnityEngine;

public class Health : MonoBehaviour
{
    //podstawowe zmienne zdrowia
    public int maxHealth = 100;
    private int currentHealth;
    
   //nawiązanie do HpBar
    public HpBar healthBar;
    
    //ustawienia obrażeń
    public string damageTag = "Enemy"; 
    public int damageAmount = 20;

    void Start()
    {
        currentHealth = maxHealth;
        
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    // Metoda wywołująca spadek HP i aktualizację UI
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        // Aktualizacja Slidera (Paska HP)
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth); //wartość Slidera
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    // kolizja z obiektem zadającym obrażenia
    private void OnCollisionEnter(Collision collision)
    {
        // Sprawdza, czy dotknięty obiekt ma zdefiniowany tag
        if (collision.gameObject.CompareTag(damageTag))
        {
            // Natychmiastowe zabranie obrażeń i spadek wartości hp
            TakeDamage(damageAmount);
             
        }
    }

    void Die()
    {
        Debug.Log("nie chciałbym cię martwić ale już jesteś martwy");
    }
}