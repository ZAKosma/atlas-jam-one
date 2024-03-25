using System;
using System.Collections.Generic;
using UnityEngine;

[Flags] // Enables the use of enum as bit flags for multiple movement types
public enum MovementType
{
    None = 0,
    Horizontal = 1 << 0,
    Vertical = 1 << 1,
    // Rotational = 1 << 2,
    // Scaling = 1 << 3,
}

[RequireComponent(typeof(Rigidbody))]
public class MovingPlatform : MonoBehaviour
{
    public MovementType movementTypes = MovementType.None;
    
    private delegate void MovementMethod();
    private List<MovementMethod> activeMovements = new List<MovementMethod>();

    public Vector3 movementVector = new Vector3(5f, 0, 0);
    // public Vector3 rotationVector = new Vector3(0, 30, 0); // Defines rotation speed and direction
    // public Vector3 targetScale = new Vector3(1f, 1f, 1f); // Target scale of the platform
    public float speed = 1.0f; // Used for rotational and scaling speed
    public float cycleTime = 3.0f; // Duration of one movement cycle
    public float cycleDelay = 0.5f; // Delay between cycles

    private Vector3 startPosition;
    private Vector3 initialScale;
    private Vector3 targetPosition;
    private float timer = 0.0f;
    private bool isDelaying = false;
    
    private HashSet<Rigidbody> affectedRigidbodies = new HashSet<Rigidbody>();
    private CharacterController affectedController;

    private Rigidbody platformRb;

    void Start()
    {
        platformRb = GetComponent<Rigidbody>();
        platformRb.isKinematic = true;

        startPosition = transform.position;
        initialScale = transform.localScale;
        SubscribeToActiveMovements();
        PrepareMovement();
    }

    
    void Update()
    {
        if (!isDelaying)
        {
            timer += Time.deltaTime;
            if (timer > cycleTime)
            {
                StartDelay();
            }
            CheckCharacterControllerPresence();
        }
    }

    void FixedUpdate()
    {
        if (!isDelaying)
        {
            // Store the previous position to calculate the actual movement done this frame.
            Vector3 previousPosition = transform.position;
        
            PerformActiveMovements(); // This adjusts the transform.position based on the cycle.

            Vector3 actualMovementThisFrame = transform.position - previousPosition;

            // Apply the same movement to Rigidbodies to ensure they stay on the platform.
            foreach (var rb in affectedRigidbodies)
            {
                // Use rb.position + actualMovementThisFrame for frame-accurate movement.
                rb.MovePosition(rb.position + actualMovementThisFrame);
            }

            if (affectedController != null)
            {
                affectedController.Move(actualMovementThisFrame);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody rb))
        {
            affectedRigidbodies.Add(rb);
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody rb))
        {
            affectedRigidbodies.Remove(rb);
        }
    }
    
    
    void CheckCharacterControllerPresence()
    {
        if (affectedController != null)
        {
            Debug.Log("Character controller is present");
            
            // Assume the ray starts at the character's feet and extends downwards slightly beyond the expected ground contact point.
            Vector3 rayStart = affectedController.transform.position + (Vector3.up * 0.1f); // Start slightly above the feet to ensure the ray starts within the character
            float rayLength = (affectedController.height / 2) + 0.5f; // Adjust the length based on expected clearance between character and platform
            bool isGrounded = Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, rayLength);

            // Check if the ray hit this moving platform
            if (!isGrounded || hit.transform != this.transform)
            {
                // The character is considered no longer on this platform
                ClearCurrentCharacterController(affectedController);
            }
        }
    }

    public void SetCurrentCharacterController(CharacterController controller)
    {
        affectedController = controller;
    }
    
    public void ClearCurrentCharacterController(CharacterController controller)
    {
        if (affectedController == controller)
        {
            affectedController = null;
        }
    }

    
    private void SubscribeToActiveMovements()
    {
        activeMovements.Clear();

        if (movementTypes.HasFlag(MovementType.Horizontal) || movementTypes.HasFlag(MovementType.Vertical))
        {
            activeMovements.Add(PerformLinearMovement);
        }
        // if (movementTypes.HasFlag(MovementType.Rotational))
        // {
        //     activeMovements.Add(PerformRotationalMovement);
        // }
        // if (movementTypes.HasFlag(MovementType.Scaling))
        // {
        //     activeMovements.Add(PerformScalingMovement);
        // }
    }

    private void PerformActiveMovements()
    {
        foreach (var movement in activeMovements)
        {
            movement();
        }
    }

    private void UpdateTimerAndPrepareMovementIfNecessary()
    {
        timer += Time.deltaTime;
        if (timer > cycleTime)
        {
            StartDelay();
        }
    }

    private void PerformLinearMovement()
    {
        // Calculate a factor that smoothly goes from 0 to 1 and back to 0 over the cycleTime
        float t = Mathf.Abs(Mathf.Cos(Mathf.PI * timer / cycleTime));
        transform.position = Vector3.Lerp(startPosition, targetPosition, t);
    }

    // private void PerformScalingMovement()
    // {
    //     // Similar to linear movement, smoothly scale up and then back down
    //     float t = Mathf.Abs(Mathf.Cos(Mathf.PI * timer / cycleTime));
    //     transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
    // }
    //
    // private void PerformRotationalMovement()
    // {
    //     transform.Rotate(rotationVector * Time.deltaTime * speed);
    // }

    private void PrepareMovement()
    {
        if (movementTypes.HasFlag(MovementType.Horizontal) || movementTypes.HasFlag(MovementType.Vertical))
        {
            targetPosition = startPosition + movementVector;
        }
    }

    private void StartDelay()
    {
        if (cycleDelay > 0)
        {
            isDelaying = true;
            Invoke(nameof(EndDelay), cycleDelay);
        }
        else
        {
            timer -= cycleTime;
            PrepareMovement();
        }
    }

    private void EndDelay()
    {
        timer -= cycleTime;
        PrepareMovement();
        isDelaying = false;
    }
}
