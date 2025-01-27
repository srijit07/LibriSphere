using UnityEngine;

public class SmoothRandomRotation : MonoBehaviour
{
    public float rotationSpeed = 10f;          // Speed of rotation
    public float directionChangeInterval = 2f; // Time interval to change direction

    private Vector3 randomDirection;           // Current target rotation direction
    private Vector3 currentDirection;          // Smoothly transitioning direction

    private void Start()
    {
        // Initialize directions
        currentDirection = transform.forward;
        SetRandomDirection();
        InvokeRepeating(nameof(SetRandomDirection), directionChangeInterval, directionChangeInterval);
    }

    private void Update()
    {
        // Smoothly transition towards the random direction
        currentDirection = Vector3.Lerp(currentDirection, randomDirection, Time.deltaTime);

        // Apply rotation based on the current interpolated direction
        transform.Rotate(rotationSpeed * Time.deltaTime * currentDirection);
    }

    private void SetRandomDirection()
    {
        // Generate a random direction vector
        randomDirection = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;
    }
}
