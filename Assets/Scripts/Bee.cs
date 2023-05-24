using BeeState;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;

public class Bee : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float detectRange = 10f;
    public float attackRange = 6f;
    public float runRange = 2f;

    private StateBase[] states;
    public State curState;

    public Transform player;
    public Vector3 prevPos;

    private void Awake()
    {
        states = new StateBase[(int)State.Size];
        states[(int)State.Idle] = new IdleState(this);
        states[(int)State.Trace] = new TraceState(this);
        states[(int)State.Return] = new ReturnState(this);
        states[(int)State.Attack] = new AttackState(this);
        states[(int)State.Attack] = new RunawayState(this);

    }

    private void Start()
    {
        curState = State.Idle;
        states[(int)curState].Enter();
        prevPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        states[(int)curState].Update();
    }

    public void ChangeState(State state)
    {
        states[(int)curState].Exit();
        curState = state;
        states[(int)curState].Enter();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, runRange);
    }
}

namespace BeeState
{
    public enum State { Idle, Trace, Return, Attack, Runaway, Size }

    public class IdleState : StateBase
    {
        private Bee bee;
        private float idieTime;

        public IdleState(Bee bee)
        {
            this.bee = bee;
        }

        public override void Enter()
        {
            
        }

        public override void Update()
        {
            if (Vector2.Distance(bee.player.position, bee.transform.position) <= bee.detectRange)
            {
                bee.ChangeState(State.Trace);
            }
        }

        public override void Exit()
        {
            
        }
    }

    public class TraceState : StateBase
    {
        private Bee bee;

        public TraceState(Bee bee)
        {
            this.bee = bee;
        }

        public override void Enter()
        {
            
        }

        public override void Update()
        {
            Vector2 dir = (bee.player.position - bee.transform.position).normalized;
            bee.transform.Translate(dir * bee.moveSpeed * Time.deltaTime);

            if (Vector2.Distance(bee.player.position, bee.transform.position) <= bee.attackRange)
            {
                bee.ChangeState(State.Attack);
            }
            else if (Vector2.Distance(bee.player.position, bee.transform.position) > bee.detectRange)
            {
                bee.ChangeState(State.Return);
            }
        }

        public override void Exit()
        {
            
        }
    }

    public class ReturnState : StateBase
    {
        private Bee bee;

        public ReturnState(Bee bee)
        {
            this.bee = bee;
        }

        public override void Enter()
        {
            
        }

        public override void Update()
        {
            Vector2 dir = (bee.prevPos - bee.transform.position).normalized;
            bee.transform.Translate(dir * bee.moveSpeed * Time.deltaTime);

            if (Vector2.Distance(bee.transform.position, bee.prevPos) < 0.02f)
            {
                bee.ChangeState(State.Idle);
            }
            else if (Vector2.Distance(bee.player.position, bee.transform.position) <= bee.detectRange)
            {
                bee.ChangeState(State.Trace);
            }
        }

        public override void Exit()
        {
            
        }
    }

    public class AttackState : StateBase
    {
        private Bee bee;
        private float attackCoolTime = 1;

        public AttackState(Bee bee)
        {
            this.bee = bee;
        }

        public override void Enter()
        {
            
        }

        public override void Update()
        {
            attackCoolTime += Time.deltaTime;
            if (attackCoolTime > 0.5f)
            {
                Debug.Log("Attack");
                attackCoolTime = 0;
            }

            if (Vector2.Distance(bee.player.position, bee.transform.position) > bee.attackRange)
            {
                bee.ChangeState(State.Trace);
            }
            else if (Vector2.Distance(bee.player.position, bee.transform.position) <= bee.runRange)
            {
                bee.ChangeState(State.Runaway);
            }
        }

        public override void Exit()
        {
            
        }
    }

    public class RunawayState : StateBase
    {
        private Bee bee;

        public RunawayState(Bee bee)
        {
            this.bee = bee;
        }

        public override void Enter()
        {

        }

        public override void Update()
        {
            Vector2 dir = (bee.transform.position - bee.player.position).normalized;
            bee.transform.Translate(dir * bee.moveSpeed * Time.deltaTime);

            if (Vector2.Distance(bee.player.position, bee.transform.position) > bee.runRange)
            {
                bee.ChangeState(State.Attack);
            }
        }

        public override void Exit()
        {

        }
    }
        
}
