using UnityEngine;

public class PlayerShoots : MonoBehaviour
{
    public float range = 100f; // Range of the gun

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, range))
            {
                BlockHealth blockHealth = hit.transform.GetComponent<BlockHealth>();
                if (blockHealth != null)
                {
                    // Calculate the hit point relative to the block
                    Vector3 relativeHitPoint = hit.point - blockHealth.transform.position;

                    // Assuming each child square is a unit cube and the block's size is 4x4
                    int blockSize = 4; // Adjust based on your block's actual size
                    float squareSize = 1.0f; // Adjust based on your child square's actual size

                    // Calculate the child index based on the hit point
                    int x = Mathf.FloorToInt(relativeHitPoint.x / squareSize);
                    int y = Mathf.FloorToInt(relativeHitPoint.y / squareSize);
                    int z = Mathf.FloorToInt(relativeHitPoint.z / squareSize);

                    // Ensure the calculated index is within the bounds of the block
                    if (x >= 0 && x < blockSize && y >= 0 && y < blockSize && z >= 0 && z < blockSize)
                    {
                        int childIndex = x + y * blockSize + z * blockSize * blockSize;
                        // Now you have the child index, you can proceed to delete the square
                    }
                }
            }
        }
    }
}