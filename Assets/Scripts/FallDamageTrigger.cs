using UnityEngine;

public class FallDamageTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player playerScript = other.GetComponent<Player>();
            if (playerScript != null && playerScript.isFalling)
            {
                // Directly use fall height to apply fall damage
                float fallHeight = playerScript.fallStartHeight - other.transform.position.y;
                playerScript.ApplyFallDamage(fallHeight);
                playerScript.isFalling = false; // Reset the falling state
            }
        }
    }
}
