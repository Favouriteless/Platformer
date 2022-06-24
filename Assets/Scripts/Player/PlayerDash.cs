using UnityEngine;

public class PlayerDash : PlayerStateHandler
{
    [Header("References")]
    public Rigidbody2D rb;
    public BoxCollider2D col;
    public GameObject graphic;

    [Header("Movement Settings")]
    public float maxRange;
    public float dashSpeed;

    [Header("Startup Settings")]
    public float startTime;
    public float vibrationMax;

    [Header("Ending Settings")]
    public float endTime;

    [Header("Miscellaneous")]
    public LayerMask groundMask;

    public DashState State { 
        get { return state; } 
    }

    private DashState state;
    private float timer;
    private Vector3 destination;
    private float maxTime;
    private float timeStartedAt;

    public override void Execute()
    {
        switch(state) {
            case DashState.START:
                HandleStart();
                break;
            case DashState.MOVE:
                HandleMovement();
                break;
            case DashState.END:
                HandleEnd();
                break;
        }
    }

    private void HandleStart()
    {
        graphic.transform.localPosition = new Vector3(Random.Range(0f, vibrationMax), Random.Range(0f, vibrationMax), 0f);
        timer -= Time.fixedDeltaTime;
        if (timer < 0f)
        {
            graphic.transform.localPosition = Vector3.zero;
            state = DashState.MOVE;
            maxTime = maxRange / dashSpeed;
            timeStartedAt = Time.time;
        }
    }

    private void HandleMovement()
    {
        rb.MovePosition(Vector3.MoveTowards(transform.position, destination, dashSpeed * Time.fixedDeltaTime));
        if (transform.position == destination || Time.time - timeStartedAt > maxTime)
        {
            timer = endTime;
            rb.velocity = Vector2.zero;
            state = DashState.END;
        }
    }

    private void HandleEnd()
    {
        timer -= Time.fixedDeltaTime;
        if(timer < 0)
        {
            state = DashState.DONE;
        }
    }

    public void StartDash(Vector2 _direction)
    {
        rb.velocity = Vector2.zero;
        state = DashState.START;
        timer = startTime;

        RaycastHit2D hit = Physics2D.BoxCast(transform.position, col.size, 0f, _direction, maxRange, groundMask);
        destination = hit ? hit.centroid : transform.position + new Vector3(_direction.x, _direction.y, 0f)*maxRange;
    }

    public enum DashState
    {
        START,
        MOVE,
        END,
        DONE
    }
}
