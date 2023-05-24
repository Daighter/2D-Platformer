using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController230523 : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float movePower = 5f;
    [SerializeField] float jumpPower = 10f;

    [SerializeField] LayerMask groundMask;

    private Animator animator;
    private new SpriteRenderer renderer;
    private Vector2 inputDir;
    [SerializeField] bool isGrounded;
    //private bool isHited;

    //private Coroutine moveRoutine;

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
        GroundCheck();
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

    private void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, groundMask);
        if (hit.collider != null)
        {
            isGrounded = true;
            animator.SetBool("IsGround", true);
            Debug.DrawRay(transform.position, new Vector3(hit.point.x, hit.point.y, 0) - transform.position, Color.red);
        }
        else
        {
            isGrounded = false;
            animator.SetBool("IsGround", false);
            Debug.DrawRay(transform.position, Vector2.down * 1.5f, Color.red);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
        animator.SetBool("IsGround", true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isGrounded = false;
        animator.SetBool("IsGround", false);
    }
}
