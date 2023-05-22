using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
    private new Rigidbody2D rb;

    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float movePower = 5f;
    [SerializeField] float jumpPower = 10f;

    [SerializeField] LayerMask groundMask;

    private Animator animator;
    private new SpriteRenderer renderer;
    private Vector2 inputDir;
    private bool isGrounded;
    private bool isHited;

    private Coroutine moveRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        //moveRoutine = StartCoroutine(MoveRoutine());
    }

    private void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {
        //GroundCheck();
    }

    private void Move()
    {
        if (inputDir.x < 0 && rb.velocity.x > -maxSpeed)
            rb.AddForce(Vector2.right * inputDir.x * movePower, ForceMode2D.Force);
        else if (inputDir.x > 0 && rb.velocity.x < maxSpeed)
            rb.AddForce(Vector2.right * inputDir.x * movePower, ForceMode2D.Force);

        if (inputDir.x > 0)
            renderer.flipX = false;
        else if (inputDir.x < 0)
            renderer.flipX = true;
    }

    private void OnMove(InputValue value)
    {
        inputDir = value.Get<Vector2>();
        animator.SetFloat("MoveSpeed", Mathf.Abs(inputDir.x));
    }

    private void OnJump(InputValue value)
    {/*
        if (!value.isPressed)
            return;
        if (!isGrounded)
            return;
        if (isHited)
            return;
        */
        Jump();
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    private void GroundCheck()
    {
        Debug.DrawRay(transform.position, Vector2.down * 1.5f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, groundMask);
        if (hit.collider != null)
        {
            isGrounded = true;
            animator.SetBool("IsGrounded", true);

            // Smooth landing
            if (rb.velocity.y < -3)
            {
                rb.velocity = new Vector2(rb.velocity.x, -3);
            }
        }
        else
        {
            isGrounded = false;
            animator.SetBool("IsGrounded", false);
        }
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            if (inputDir.x < 0 && rb.velocity.x > -maxSpeed)
                rb.AddForce(Vector2.right * inputDir.x * movePower, ForceMode2D.Force);
            else if (inputDir.x > 0 && rb.velocity.x < maxSpeed)
                rb.AddForce(Vector2.right * inputDir.x * movePower, ForceMode2D.Force);

            animator.SetFloat("MoveDirX", Mathf.Abs(inputDir.x));
            if (inputDir.x > 0)
                renderer.flipX = false;
            else if (inputDir.x < 0)
                renderer.flipX = true;

            yield return null;
        }
    }

    public void Hit()
    {
        StartCoroutine(HitRoutine());
    }

    private IEnumerator HitRoutine()
    {
        StopCoroutine(moveRoutine);
        animator.SetBool("IsHit", true);
        isHited = true;
        yield return new WaitForSeconds(1f);
        animator.SetBool("IsHit", false);
        isHited = false;
        moveRoutine = StartCoroutine(MoveRoutine());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetBool("IsGround", true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        animator.SetBool("IsGround", false);
    }
}
