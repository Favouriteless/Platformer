using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(PlayerDash))]
[RequireComponent(typeof(PlayerClimb))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public PlayerMotor motor;
    public PlayerDash dash;
    public PlayerClimb climb;

    [Header("Control Settings")]
    public float jumpTolerance;

    private int dashCharges = 1;
    private float jumpTimer;
    private PlayerState state = PlayerState.DEFAULT;

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
            jumpTimer = jumpTolerance;

        motor.inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        motor.inputJump = jumpTimer >= 0f;
        motor.jumpHold = Input.GetButton("Jump");
        climb.input = motor.inputVector.y;

        if(state != PlayerState.DASH)
        {
            if(Input.GetButtonDown("Dash"))
            {
                TryDash();
            }

            if (state == PlayerState.CLIMB)
            {
                if (!Input.GetButton("Climb"))
                    state = PlayerState.DEFAULT;

                if (climb.isLeft && !motor.IsWallFootLeft)
                    state = PlayerState.DEFAULT;
                else if (!climb.isLeft && !motor.IsWallFootRight)
                    state = PlayerState.DEFAULT;
            }
            else
            {
                if (Input.GetButton("Climb"))
                    TryClimb();
            }
        }

        jumpTimer -= Time.deltaTime;
    }

    private void TryDash()
    {
        if (Input.GetButtonDown("Dash"))
        {
            if (dashCharges >= 1 && motor.inputVector != Vector2.zero)
            {
                state = PlayerState.DASH;
                dash.StartDash(motor.inputVector.normalized);
                dashCharges--;
            }
        }
    }

    private void TryClimb()
    {
        bool isPressingLeft = motor.inputVector.x < -0.5f;

        if(isPressingLeft) // If pressing left, prioritise left
        {
            if(motor.IsWallFootLeft)
                StartClimb(true);
        }
        else // If not pressing left, prioritise right
        {
            if (motor.IsWallFootRight)
                StartClimb(false);
            else if (motor.IsWallFootLeft)
                StartClimb(true);
        }
    }

    private void StartClimb(bool isLeft)
    {
        state = PlayerState.CLIMB;
        climb.isLeft = isLeft;
    }


    private void FixedUpdate()
    {
        switch (state)
        {
            case PlayerState.DEFAULT:
                motor.Execute();
                if(motor.IsGrounded)
                {
                    dashCharges = 1;
                }
                break;
            case PlayerState.DASH:
                dash.Execute();
                if (dash.State == PlayerDash.DashState.DONE)
                    state = PlayerState.DEFAULT;
                break;
            case PlayerState.CLIMB:
                climb.Execute();
                break;
        }
    }

    public void ResetJumpTimer()
    {
        jumpTimer = -1f;
    }

    public enum PlayerState
    {
        DEFAULT,
        DASH,
        CLIMB
    }
}

public abstract class PlayerStateHandler : MonoBehaviour
{
    public abstract void Execute();
}
