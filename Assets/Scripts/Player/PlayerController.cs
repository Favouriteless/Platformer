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
    public LayerManager layerManager;

    [Header("Control Settings")]
    public float jumpTolerance;

    private int dashCharges = 1;
    private float jumpTimer;
    private PlayerState state = PlayerState.DEFAULT;

    private Vector2 inputVector;
    private bool inputJump;
    private bool jumpHold;

    public Vector2 InputVector { get { return inputVector; } }
    public bool InputJump { get { return inputJump; } }
    public bool JumpHold { get { return jumpHold; } }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
            jumpTimer = jumpTolerance;

        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputJump = jumpTimer >= 0f;
        jumpHold = Input.GetButton("Jump");

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

        if(Input.GetButtonDown("Layer"))
        {
            layerManager.ToggleLayer(gameObject);
        }

        jumpTimer -= Time.deltaTime;
    }

    private void TryDash()
    {
        if (Input.GetButtonDown("Dash"))
        {
            if (dashCharges >= 1 && inputVector != Vector2.zero)
            {
                state = PlayerState.DASH;
                dash.StartDash(inputVector.normalized);
                dashCharges--;
            }
        }
    }

    private void TryClimb()
    {
        bool isPressingLeft = inputVector.x < -0.5f;

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
                    dashCharges = 1;
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
