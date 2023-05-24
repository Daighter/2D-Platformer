using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float detectRange = 4f;
    [SerializeField] float attackRange = 1f;

    public enum State { Idle, Trace, Return, Attack }

    private State curState;

    private Transform player;

    Vector3 prevPos;

    private void Start()
    {
        prevPos = transform.position;
        curState = State.Idle;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        switch (curState)
        {
            case State.Idle:
                IdleUpdate();
                break;
            case State.Trace:
                TraceUpdate();
                break;
            case State.Return:
                ReturnUpdate();
                break;
            case State.Attack:
                AttackUpdate();
                break;
        }
    }

    private void IdleUpdate()
    {
        if (Vector2.Distance(player.position, transform.position) <= detectRange)
        {
            curState = State.Trace;
        }
    }

    private void TraceUpdate()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime);

        if (Vector2.Distance(player.position, transform.position) > detectRange)
        {
            curState = State.Return;
        }
        else if (Vector2.Distance(player.position, transform.position) <= attackRange)
        {
            curState = State.Attack;
        }
    }

    private void ReturnUpdate()
    {
        Vector2 dir = (prevPos - transform.position).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, prevPos) < 0.02f)
        {
            curState = State.Idle;
        }
        else if (Vector2.Distance(player.position, transform.position) <= detectRange)
        {
            curState = State.Trace;
        }

    }

    private float attackCoolTime = 1;

    private void AttackUpdate()
    {
        attackCoolTime += Time.deltaTime;
        if (attackCoolTime > 1f)
        {
            Debug.Log("Attack");
            attackCoolTime = 0;
        }
        else if (Vector2.Distance(player.position, transform.position) > attackRange)
        {
            curState = State.Trace;
        }
    }
}
