using UnityEngine;

public class FollowPlayerY : MonoBehaviour
{
    private Transform playerTransform;
    private float highestY;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        highestY = playerTransform.position.y;
    }

    void Update()
    {
        if (playerTransform.position.y > highestY)
        {
            highestY = playerTransform.position.y;
            transform.position = new Vector3(transform.position.x, highestY, transform.position.z);
        }
    }
}