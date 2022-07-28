using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlayerController))]
public class PlayerMotor : PlayerStateHandler
{
    [Header("References")]
    public Rigidbody2D rb;
    public BoxCollider2D col;
    public PlayerController controller;

    [Header("Movement Settings")]
    public float maxSpeedX;
    public float maxSpeedY;
    public float acceleration;
    public float decceleration;
    public float accelerationAirMultiplier;
    public float deccelerationAirMultiplier;
    public float jumpVelocity;
    public float wallSlideSpeed;
    public Vector2 wallJumpVelocity;

    [Header("Physics Settings")]
    public float gravity;
    public float gravityMultiplier;
    public float driftSpeed;
    public LayerMask groundMask;
    public float groundedDistance;

    public bool IsGrounded => isGrounded;
    public bool IsWallFootLeft => isWallFootLeft;
    public bool IsWallFootRight => isWallFootRight;

    private bool isGrounded;
    private bool isWallFootLeft;
    private bool isWallFootRight;

    private void Update()
    {
        isGrounded = GetFloorContact(Vector2.down);
        isWallFootLeft = GetFootContact(Vector2.left);
        isWallFootRight = GetFootContact(Vector2.right);
    }

    public override void Execute()
    {
        Vector2 velocity = rb.velocity;
        float aMultiplier = 1f;
        float dMultiplier = 1f;

        if(IsGrounded)
        {
            if (controller.InputJump)
            {
                velocity.y = jumpVelocity;
                controller.ResetJumpTimer();
            }
        }
        else
        {
            aMultiplier = accelerationAirMultiplier;
            dMultiplier = deccelerationAirMultiplier;

            float g = LayerManager.INSTANCE.ActiveLayer == Layers.GRAVITY ? -gravity : gravity;
            if (rb.velocity.y < 0f || !controller.JumpHold)
                g *= gravityMultiplier;
            velocity.y -= g * Time.fixedDeltaTime; // Regular gravity

            if (velocity.y < -wallSlideSpeed) // Wall sliding
            {
                if ((isWallFootLeft && controller.InputVector.x == -1f) || (isWallFootRight && controller.InputVector.x == 1f))
                    velocity.y = -wallSlideSpeed;
            }

            if(controller.InputJump) // Wall jump
            {
                velocity = ApplyWallJump(velocity);
            }
        }

        // ------------------------------------- HORIZONTAL MOVEMENT -------------------------------------

        if (controller.InputVector.x == 0f || velocity.x * controller.InputVector.x < 0f) // If not inputting or inputting opposite direction to velocity
        {
            int mult = velocity.x > 0f ? -1 : 1;
            float deltaV = decceleration * dMultiplier *  mult * Time.fixedDeltaTime;

            if (Mathf.Abs(deltaV) >= Mathf.Abs(velocity.x))
                velocity.x = 0;
            else
                velocity.x += deltaV;
        }
        velocity.x += acceleration * aMultiplier * controller.InputVector.x * Time.fixedDeltaTime;

        if (Mathf.Abs(velocity.x) < driftSpeed) // To stop drifting at near zero velocities;
            velocity.x = 0;
        rb.velocity = new Vector2(Mathf.Clamp(velocity.x, -maxSpeedX, maxSpeedX), Mathf.Clamp(velocity.y, -maxSpeedY, maxSpeedY));
    }

    private bool GetFloorContact(Vector2 dir)
    {
        return Physics2D.BoxCast(transform.position, col.size, 0f, dir, groundedDistance, groundMask);
    }

    private bool GetFootContact(Vector2 dir)
    {
        return Physics2D.BoxCast(transform.position + new Vector3(0f, -col.size.y/4), new Vector2(col.size.x, col.size.y/2), 0f, dir, groundedDistance, groundMask);
    }

    public Vector2 ApplyWallJump(Vector2 velocity)
    {
        float mult = 0f;
        if (isWallFootLeft)
            mult = 1f;
        else if (isWallFootRight)
            mult = -1f;

        if (mult != 0)
        {
            controller.ResetJumpTimer();
            velocity.x += wallJumpVelocity.x * mult;
            velocity.y = wallJumpVelocity.y;
        }
        return velocity;
    }

}
