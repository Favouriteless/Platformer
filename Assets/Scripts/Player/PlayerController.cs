using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(PlayerDash))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public PlayerMotor motor;
    public PlayerDash dash;

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

        if(state != PlayerState.DASH)
        {
            if(Input.GetButtonDown("Dash"))
            {
                TryDash();
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
            default:
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
