using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public PlayerMotor motor;

    [Header("Control Settings")]
    public float jumpTolerance;

    private float jumpTimer;
    private PlayerState state = PlayerState.DEFAULT;

    private void Update()
    {
        switch(state)
        {
            case PlayerState.DEFAULT:
                if (Input.GetButtonDown("Jump"))
                    jumpTimer = jumpTolerance;

                motor.inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                motor.inputJump = jumpTimer >= 0f;
                motor.jumpHold = Input.GetButton("Jump");
                break;

            default:
                break;
        }
        jumpTimer -= Time.deltaTime;
    }

    private enum PlayerState
    {
        DEFAULT,
        DASH,
        CLIMB
    }
}
