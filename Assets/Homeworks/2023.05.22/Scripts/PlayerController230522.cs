using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController230522 : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float movePower = 5f;
    [SerializeField] float jumpPower = 10f;

    private Animator animator;
    private new SpriteRenderer renderer;
    private Vector2 inputDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Move();
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
    {
        Jump();
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
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
