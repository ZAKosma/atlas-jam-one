using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour
{
    private CharacterController characterController;
    private Health health;

    public bool isFalling = false;
    public float fallStartHeight = 0.0f;

    // Thresholds and damage values can be adjusted in the Inspector for balancing
    public float safeFallDistance = 3.0f; // Distance in meters the player can fall without taking damage
    public float maxFallDistance = 10.0f; // Maximum distance for calculating fall damage
    public int minFallDamage = 10; // Damage taken for falling just over the safe fall distance
    public int maxFallDamage = 100; // Damage taken for falling the maximum distance or more

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        health = GetComponent<Health>();
    }

    void Update()
    {
        CheckFallStatus();
    }

    private void CheckFallStatus()
    {
        if (characterController.isGrounded)
        {
            if (isFalling)
            {
                // Player has landed, calculate fall distance and apply damage if necessary
                isFalling = false;
                float fallDistance = fallStartHeight - transform.position.y;
                ApplyFallDamage(fallDistance);
            }
        }
        else
        {
            if (!isFalling)
            {
                // Player has started falling
                isFalling = true;
                fallStartHeight = transform.position.y;
            }
            else
            {
                // Continuously update the start height to the highest point reached during fall
                fallStartHeight = Mathf.Max(fallStartHeight, transform.position.y);
            }
        }
    }

    public void ApplyFallDamage(float distance)
    {
        if (distance > safeFallDistance)
        {
            float damageRatio = Mathf.InverseLerp(safeFallDistance, maxFallDistance, distance);
            int damageToApply = Mathf.RoundToInt(Mathf.Lerp(minFallDamage, maxFallDamage, damageRatio));
            health.TakeDamage(damageToApply);
            Debug.Log($"Player took {damageToApply} damage from a {distance}m fall.");
        }
        else
        {
            Debug.Log("Safe fall. No damage applied.");
        }
    }
}
