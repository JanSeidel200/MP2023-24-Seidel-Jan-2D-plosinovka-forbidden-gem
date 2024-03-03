using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 9f;
    [SerializeField] private float jumpForce = 14f;
    private float originalMoveSpeed; 
    private bool isSpeedBoosted = false;
    private bool isSpeedBoostedDown = false;
    private float speedBoostDuration = 5f; 
    private Coroutine speedBoostCoroutine;

    private enum MovementState { idle, running, jumping, falling, landing }

    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource buffSoundEffect;
    [SerializeField] private AudioSource debuffSoundEffect;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        originalMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene("Menu");
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            float gravityDirection = Mathf.Sign(Physics2D.gravity.y);
            if (gravityDirection > 0f)
            {
                jumpSoundEffect.Play();
                rb.velocity = new Vector2(rb.velocity.x, -jumpForce);
            }
            else
            {
                jumpSoundEffect.Play();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
                
        }

        UpdateAnimationState();
    }
    
    private void UpdateAnimationState()
    {
        MovementState state;

        float gravityDirection = Mathf.Sign(Physics2D.gravity.y);

        if (dirX > 0f && gravityDirection > 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else if (dirX < 0f && gravityDirection > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX > 0f && gravityDirection < 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f && gravityDirection < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
            if (IsGrounded())
            {
                state = MovementState.landing;
            }
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        float gravityDirection = Mathf.Sign(Physics2D.gravity.y);
        if(gravityDirection > 0f)
        {
            return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.up, .1f, jumpableGround);
        }
        else
        {
            return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
        }
        
    }
    public void ApplySpeedBoost(float multiplier, bool isBoosted)
    {
        RemoveSpeedBoost();
        StopAllCoroutines();
        if (isBoosted)
        {
            if (!isSpeedBoosted)
            {
                isSpeedBoosted = true;
                moveSpeed *= multiplier;
                speedBoostCoroutine = StartCoroutine(SpeedBoostTimer());
                buffSoundEffect.Play();
            }
            else
            {
                StopCoroutine(speedBoostCoroutine);
                speedBoostCoroutine = StartCoroutine(SpeedBoostTimer());
                buffSoundEffect.Play();
            }
        }
        else
        {
            if (!isSpeedBoostedDown)
            {
                isSpeedBoostedDown = true;
                moveSpeed *= multiplier;
                speedBoostCoroutine = StartCoroutine(SpeedBoostTimer());
                debuffSoundEffect.Play();
            }
            else
            {
                StopCoroutine(speedBoostCoroutine);
                speedBoostCoroutine = StartCoroutine(SpeedBoostTimer());
                debuffSoundEffect.Play();
            }
        }
        
    }

    private IEnumerator SpeedBoostTimer()
    {
        yield return new WaitForSeconds(speedBoostDuration);
        RemoveSpeedBoost();
    }

    private void RemoveSpeedBoost()
    {
        isSpeedBoosted = false;
        isSpeedBoostedDown = false;
        moveSpeed = originalMoveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SpeedBoost"))
        {
            ApplySpeedBoost(2f, true);
        }
        if (collision.CompareTag("SpeedBoostDown"))
        {
            ApplySpeedBoost(0.5f, false);
        }
    }

    public Vector2 GetVelocity()
    {
        return rb.velocity;
    }
}