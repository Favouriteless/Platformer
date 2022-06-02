using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMotor : MonoBehaviour
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
    public float jumpVelocity;

    [Header("Physics Settings")]
    public float gravity;
    public float gravityMultiplier;
    public float driftSpeed;
    public LayerMask groundMask;
    public float groundedDistance;

    [Header("Miscellaneous")]
    public Vector2 inputVector = new Vector2();
    public bool inputJump = false;
    public bool jumpHold = false;

    public bool IsGrounded {
        get { return isGrounded; }
    }

    private bool isGrounded;

    private void Update()
    {
        isGrounded = GetGrounded();
    }

    private void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;
        float aMultiplier = 1f;
        float dMultiplier = 1f;

        if(IsGrounded)
        {
            if (inputJump)
                velocity.y = jumpVelocity;
            else
                velocity.y = 0f;
        }
        else
        {
            aMultiplier = accelerationAirMultiplier;
            dMultiplier = deccelerationAirMultiplier;

            float g = gravity;
            if (rb.velocity.y < 0f || !jumpHold)
                g *= gravityMultiplier;

            velocity.y -= g * Time.fixedDeltaTime;
        }

        // ------------------------------------- HORIZONTAL MOVEMENT -------------------------------------

        if (inputVector.x == 0f || velocity.x * inputVector.x < 0f) // If not inputting or inputting opposite direction to velocity
        {
            int mult = velocity.x > 0f ? -1 : 1;
            float deltaV = decceleration * dMultiplier *  mult * Time.fixedDeltaTime;

            if (Mathf.Abs(deltaV) >= Mathf.Abs(velocity.x))
                velocity.x = 0;
            else
                velocity.x += deltaV;
        }
        velocity.x += acceleration * aMultiplier * inputVector.x * Time.fixedDeltaTime;

        if (Mathf.Abs(velocity.x) < driftSpeed) // To stop drifting at near zero velocities;
            velocity.x = 0;
        rb.velocity = new Vector2(Mathf.Clamp(velocity.x, -maxSpeedX, maxSpeedX), Mathf.Clamp(velocity.y, -maxSpeedY, maxSpeedY));
    }

    private bool GetGrounded()
    {
        return Physics2D.BoxCast(transform.position, col.size, 0f, Vector2.down, groundedDistance, groundMask);
    }

}
