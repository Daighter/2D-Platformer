using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    //private bool isDie = false;

    [SerializeField] float moveSpeed;
    [SerializeField] Transform groundCheckPosition;
    [SerializeField] LayerMask groundLayer;

    protected new Rigidbody2D rigidbody;
    protected Animator animator;
    protected new Collider2D collider;
    protected new SpriteRenderer renderer;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        if (!IsGroundExist())
            Turn();
    }

    private void Move()
    {
        //animator.SetBool("Move", true);
        rigidbody.velocity = new Vector2(transform.right.x * -1 * moveSpeed, rigidbody.velocity.y);
    }

    private void Turn()
    {
        transform.Rotate(Vector3.up, 180);
    }
    /*
    protected override void Die()
    {
        base.Die();

        isDie = true;
    }*/

    private bool IsGroundExist()
    {
        Debug.DrawRay(groundCheckPosition.position, Vector2.down, Color.red);
        return Physics2D.Raycast(groundCheckPosition.position, Vector2.down, 1f, groundLayer);
    }
}
