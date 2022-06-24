using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerClimb : PlayerStateHandler
{
    [Header("References")]
    public PlayerMotor motor;
    public Rigidbody2D rb;
    public PlayerController controller;

    [Header("Climb Settings")]
    public float climbSpeed;
    public float acceleration;
    public float decceleration;
    public float driftSpeed;

    [Header("Miscellaneous")]
    public bool isLeft;

    public override void Execute()
    {
        Vector2 velocity = rb.velocity;

        if (controller.InputVector.y == 0f || velocity.y * controller.InputVector.y < 0f) // If not inputting or inputting opposite direction to velocity
        {
            int mult = velocity.y > 0f ? -1 : 1;
            float deltaV = decceleration * mult * Time.fixedDeltaTime;

            if (Mathf.Abs(deltaV) >= Mathf.Abs(velocity.y))
                velocity.y = 0;
            else
                velocity.y += deltaV;
        }
        velocity.y += acceleration * controller.InputVector.y * Time.fixedDeltaTime;

        if (controller.InputJump) // Wall jump
        {
            velocity = motor.ApplyWallJump(velocity);
        }

        if (Mathf.Abs(velocity.y) < driftSpeed) // To stop drifting at near zero velocities;
            velocity.y = 0;
        rb.velocity = new Vector2(velocity.x, Mathf.Clamp(velocity.y, -climbSpeed, climbSpeed));
    }
}
