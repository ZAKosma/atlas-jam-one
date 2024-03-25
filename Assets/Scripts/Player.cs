using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour
{
    private CharacterController characterController;
    private Health health;

    private bool isFalling = false;
    private float fallStartHeight = 0.0f;
    private float fallDistance = 0.0f;

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
                fallDistance = fallStartHeight - transform.position.y;
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
                // Update the start height to the highest point reached
                fallStartHeight = Mathf.Max(fallStartHeight, transform.position.y);
            }
        }
    }

    private void ApplyFallDamage(float distance)
    {
        if (distance > safeFallDistance)
        {
            float damageRatio = Mathf.InverseLerp(safeFallDistance, maxFallDistance, distance);
            int damageToApply = Mathf.RoundToInt(Mathf.Lerp(minFallDamage, maxFallDamage, damageRatio));
            health.TakeDamage(damageToApply, fallDistance, maxFallDistance);
            Debug.Log($"Player took {damageToApply} damage from a {distance}m fall.");
        }
        else
        {
            Debug.Log("Safe fall. No damage applied.");
        }
    }
}
