using UnityEngine;

public class SpiralMotion : MonoBehaviour
{
    public Transform focusPoint; // Assign this to the FocusPoint GameObject
    public float initialRadius = 5f;
    public float speed = 1f;
    public float verticalSpeed = 0.1f; // Speed of vertical movement, either up or down
    public float radiusChangePerSecond = 0f; // Allows radius to increase/decrease over time
    public float bottomHeight = -10f; // The bottom height to which the object descends
    private float angle = 0f;
    private float currentRadius;
    private float startY; // Starting Y position
    private bool descending = true; // Direction of movement, true if descending

    void Start()
    {
        startY = transform.position.y;
        currentRadius = initialRadius;
        if (focusPoint != null)
        {
            focusPoint.position = transform.position;
        }
    }

    void Update()
    {
        angle += speed * Time.deltaTime;
        currentRadius += radiusChangePerSecond * Time.deltaTime;

        float x = Mathf.Cos(angle) * currentRadius;
        float z = Mathf.Sin(angle) * currentRadius;
        // Determine vertical movement based on direction
        float y = descending ? transform.position.y - verticalSpeed * Time.deltaTime : transform.position.y + verticalSpeed * Time.deltaTime;

        // Check for direction change
        if (descending && transform.position.y <= bottomHeight)
        {
            descending = false; // Start ascending
        }
        else if (!descending && transform.position.y >= startY)
        {
            descending = true; // Resume descending
        }

        transform.position = new Vector3(x, y, z);
        
        // Optionally adjust the focus point to maintain Y position
        if (focusPoint != null)
        {
            focusPoint.position = new Vector3(focusPoint.position.x, transform.position.y, focusPoint.position.z);
        }
    }
}