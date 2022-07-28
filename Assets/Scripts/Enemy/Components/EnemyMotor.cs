using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class EnemyMotor : MonoBehaviour
{
    [Header("References")]
    public Rigidbody2D rb;
    public BoxCollider2D col;

    [Header("Movement Settings")]
    public float maxSpeedX;
    public float maxSpeedY;
    public float acceleration;
    public float decceleration;
    public float accelerationAirMultiplier;
    public float deccelerationAirMultiplier;
    public float stopDistance;

    [Header("Physics Settings")]
    public float gravity;
    public float gravityMultiplier;
    public float driftSpeed;
    public LayerMask groundMask;
    public float groundedDistance;

    public Vector2 Destination => destination;
    public bool IsGrounded => isGrounded;
    public bool IsMoving => shouldMove;

    private bool isGrounded;
    private bool shouldMove;
    private Vector3 destination;

    private void Update()
    {
        isGrounded = GetFloorContact(Vector2.down);
        if (HasArrivedAt(destination))
        {
            shouldMove = false;
            destination = transform.position;
        }
    }

    private bool HasArrivedAt(Vector3 pos)
    {
        return (pos - transform.position).magnitude < stopDistance;
    }

    /// <summary>
    /// Sets destination as the nearest accessible tile to pos
    /// </summary>
    /// <param name="pos"></param>
    public void SetDestinationNearest(Vector3 pos)
    {
        float floorOffset = col.bounds.extents.y - 0.5f;

        int maxSteps = (int)pos.x - (int)transform.position.x;
        Vector3 newDest = new Vector3((int)transform.position.x +0.5f, transform.position.y, 0f);
        Vector3 offset = new Vector3(pos.x < transform.position.x ? -1 : 1, 0f, 0f);

        for (int i = 0; i <= Mathf.Abs(maxSteps); i++)
        {
            if (TileHelper.INSTANCE.IsWalkable(newDest + offset + new Vector3(0f, -floorOffset, 0f)))
                newDest += offset;
            else
                break;
        }
        SetDestination(newDest);
    }

    public void SetDestination(Vector3 pos)
    {
        if(!HasArrivedAt(pos))
        {
            destination = pos;
            shouldMove = true;
        }
    }

    private void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;
        float aMultiplier = 1f;
        float dMultiplier = 1f;

        if(!isGrounded) // Gravity / Air controls
        {
            aMultiplier = accelerationAirMultiplier;
            dMultiplier = deccelerationAirMultiplier;

            float g = LayerManager.INSTANCE.ActiveLayer == Layers.GRAVITY ? -gravity : gravity;
            if (rb.velocity.y < 0f)
                g *= gravityMultiplier;
            velocity.y -= g * Time.fixedDeltaTime; // Regular gravity

        }

        // ------------------------------------- HORIZONTAL MOVEMENT -------------------------------------

        float xInput;
        if (shouldMove)
            xInput = transform.position.x < destination.x ? 1f : -1f;
        else
            xInput = 0f;

        if (xInput == 0f || velocity.x * xInput < 0f) // If not inputting or inputting opposite direction to velocity
        {
            int mult = velocity.x > 0f ? -1 : 1;
            float deltaV = decceleration * dMultiplier * mult * Time.fixedDeltaTime;

            if (Mathf.Abs(deltaV) >= Mathf.Abs(velocity.x))
                velocity.x = 0;
            else
                velocity.x += deltaV;
        }
        velocity.x += acceleration * aMultiplier * xInput * Time.fixedDeltaTime;

        if (Mathf.Abs(velocity.x) < driftSpeed) // To stop drifting at near zero velocities;
            velocity.x = 0;
        rb.velocity = new Vector2(Mathf.Clamp(velocity.x, -maxSpeedX, maxSpeedX), Mathf.Clamp(velocity.y, -maxSpeedY, maxSpeedY));
    }

    private bool GetFloorContact(Vector2 dir)
    {
        return Physics2D.BoxCast(transform.position, col.size, 0f, dir, groundedDistance, groundMask);
    }


}
