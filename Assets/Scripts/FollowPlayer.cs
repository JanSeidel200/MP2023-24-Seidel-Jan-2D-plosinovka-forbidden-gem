using UnityEngine;

public class FollowerPlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private Transform playerTransform;

    private float moveSpeed = 8.5f;
    private float jumpForce = 14f;
    private bool canJump = false;

    private enum MovementState { idle, running, jumping, falling, landing }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

        bool isGrounded = IsGrounded();

        bool canReachPlayer = CanReachPlayer();

        if (isGrounded && !canReachPlayer && !canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            canJump = true;
        }
        UpdateAnimationState();
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private bool CanReachPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        return distanceToPlayer < jumpForce/2;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canJump = false;
        }
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (rb.velocity.x > 0.1f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (rb.velocity.x < -0.1f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > 0.1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = MovementState.falling;
        }

        if (IsGrounded() && Mathf.Abs(rb.velocity.x) < 0.1f)
        {
            state = MovementState.idle;
        }

        anim.SetInteger("state", (int)state);
    }

}