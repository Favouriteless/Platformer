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
                state = PlayerState.DASH;
                dash.StartDash(motor.inputVector.normalized);
            }
        }

        jumpTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case PlayerState.DEFAULT:
                motor.Execute();
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
