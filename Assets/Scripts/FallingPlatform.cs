using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FallingPlatform : MonoBehaviour
{
    [Tooltip("Delay before the platform starts falling after being stepped on.")]
    public float fallDelay = 0.5f;
    
    private Rigidbody platformRigidbody;
    private bool isFalling = false;

    void Start()
    {
        platformRigidbody = GetComponent<Rigidbody>();
        // Start as kinematic to prevent it from falling due to gravity initially
        platformRigidbody.isKinematic = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with the player
        if (collision.gameObject.CompareTag("Player") && !isFalling)
        {
            // Start the fall delay coroutine
            StartCoroutine(FallAfterDelay());
        }
    }

    IEnumerator FallAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(fallDelay);

        // Activate physics-based falling
        isFalling = true;
        platformRigidbody.isKinematic = false;
        // Optionally, add force or adjust the mass for different falling effects
    }

    void OnCollisionStay(Collision collision)
    {
        // Ensure that the platform supports the player properly by applying an upward force
        // This can help mitigate jitter when the player is on a falling or unstable platform
        if (collision.gameObject.CompareTag("Player") && isFalling)
        {
            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                // Apply a slight upward force to the player to stabilize them on the platform
                playerRigidbody.AddForce(Vector3.up * 2f, ForceMode.Impulse);
            }
        }
    }
}