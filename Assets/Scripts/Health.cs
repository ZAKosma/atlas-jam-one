using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;
    
    public Slider healthBarSlider; 
    public CanvasGroup damageVignette;
    public AudioClip[] landingSounds; // Assign in the Inspector, order: no damage, small, large, death
    private AudioSource audioSource;
    
    void Start()
    {
        currentHealth = maxHealth;
        
        audioSource = GetComponent<AudioSource>(); // Ensure an AudioSource component is attached
        
        healthBarSlider.maxValue = maxHealth;
        UpdateHealthBar();
    }
    
    public void TakeDamage(int damage, float fallDistance = 0, float maxFallDistance = 0)
    {
        if (damage > 0)
        {
            currentHealth -= damage;
            UpdateHealthBar();
            
            if (currentHealth <= 0)
            {
                if (!isDead) Die();
                PlayLandingSound(3); // Indicating death blow
                return;
            }

            // Determine the severity of the fall for sound effects
            if (fallDistance > 0)
            {
                // Calculate the ratio of the fall distance to the max fall distance for severity
                float severity = fallDistance / maxFallDistance;
                if (severity < 0.5f) PlayLandingSound(1); // Small damage
                else PlayLandingSound(2); // Large damage
            }
            else
            {
                PlayLandingSound(0); // No visible damage but triggered due to fall
            }
        }
        else
        {
            // No damage was taken, possibly a very short fall
            PlayLandingSound(0); // No damage sound
        }

        StartCoroutine(ShowDamageVignette());
    }

    public void Die()
    {
        isDead = true;
        Debug.Log("Player died");
    }
    
    private void UpdateHealthBar()
    {
        healthBarSlider.value = currentHealth;
    }

    IEnumerator ShowDamageVignette()
    {
        damageVignette.alpha = 0.5f; // Adjust the alpha value to your preference
        yield return new WaitForSeconds(0.5f); // Adjust the duration to your preference
        damageVignette.alpha = 0;
    }
    
    private void PlayLandingSound(int severity)
    {
        // Adjust severity to map correctly if needed
        audioSource.PlayOneShot(landingSounds[severity]); // Severity determines the sound clip
    }
}
