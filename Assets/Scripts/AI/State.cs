using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STATE
    {
        IDLE, PATROL, REVOLT
    };

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };

    protected STATE name;
    protected EVENT stage;
    protected State nextState;

    protected GameObject npc;
    protected NavMeshAgent agent;
    protected Animator npc_animator;
    protected float timer;


    public State(GameObject _npc, NavMeshAgent _agent, Animator _npc_animator)
    {
        npc = _npc;
        agent = _agent;
        npc_animator = _npc_animator;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    public State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if(stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }
}

public class Idle : State
{
    public Idle(GameObject _npc, NavMeshAgent _agent, Animator _npc_animator) : base(_npc, _agent, _npc_animator)
    {
        name = STATE.IDLE; 
    }

    public override void Enter()
    {
        //Play Idle animation
        Debug.Log("In Idel state");
        npc_animator.SetTrigger("Idle");
        base.Enter();
    }

    public override void Update()
    {
        timer += Time.deltaTime;
        Debug.Log(timer);
        if (Random.Range(0, 100) < 10)
        {
            nextState = new Patrol(npc, agent, npc_animator);
            timer = 0;
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        //Reset animations
        npc_animator.ResetTrigger("Idle");
        base.Exit();
    }
}

public class Patrol : State
{
    int currentIndex = 0;

    public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _npc_animator) : base(_npc, _agent, _npc_animator)
    {
        name = STATE.PATROL;
    }

    public override void Enter()
    {
        //Play Idle animation
        Debug.Log("In patrol state");
        npc_animator.SetTrigger("Patrol");
        currentIndex = 0;
        base.Enter();
    }

    public override void Update()
    {
        timer += Time.deltaTime;
        Debug.Log(timer);

        /*
        if (timer > 6)
        {
            timer = 0;
            nextState = new Idle(npc, agent, npc_animator);
            stage = EVENT.EXIT;
        }*/
        if (timer > 1)
        {
            timer = 0;
            nextState = new Revolt(npc, agent, npc_animator);
            stage = EVENT.EXIT;
        }
        
        if (agent.remainingDistance < 1)
        {
            currentIndex = (currentIndex + 1) % GameEnvironment.Singleton.Waypoints.Count;
            agent.SetDestination(GameEnvironment.Singleton.Waypoints[currentIndex].transform.position);
            /*
                        if (currentIndex > GameEnvironment.Singleton.Waypoints.Count - 1 || currentIndex < 0) 
                        {
                            currentIndex = 0;
                        }
                        else
                        {
                            currentIndex++;
                        }

               */
        }
    }

    public override void Exit()
    {
        //Reset animations
        npc_animator.ResetTrigger("Patrol");
        base.Exit();
    }
}


public class Revolt : State
{
    int currentIndex = 0;

    public Revolt(GameObject _npc, NavMeshAgent _agent, Animator _npc_animator) : base(_npc, _agent, _npc_animator)
    {
        name = STATE.REVOLT;
    }

    public override void Enter()
    {
        //Play Idle animation
        int revoltAnimation = Random.Range(0, 2);
        Debug.Log("In Revolt state");
        if(revoltAnimation == 1)
        {
            npc_animator.SetTrigger("Revolt1");
        }
        else
        {
            npc_animator.SetTrigger("Revolt2");
        }
        currentIndex = 0;
        base.Enter();
    }

    public override void Update()
    {
        timer += Time.deltaTime;
        Debug.Log(timer);


        if (timer > 6)
        {
            timer = 0;
            nextState = new Patrol(npc, agent, npc_animator);
            stage = EVENT.EXIT;
        }

        if (agent.remainingDistance < 1)
        {
            currentIndex = (currentIndex + 1) % GameEnvironment.Singleton.Waypoints.Count;
            agent.SetDestination(GameEnvironment.Singleton.Waypoints[currentIndex].transform.position);
            /*
                        if (currentIndex > GameEnvironment.Singleton.Waypoints.Count - 1 || currentIndex < 0) 
                        {
                            currentIndex = 0;
                        }
                        else
                        {
                            currentIndex++;
                        }

               */
        }
    }

    public override void Exit()
    {
        //Reset animations
        npc_animator.ResetTrigger("Revolt1");
        npc_animator.ResetTrigger("Revolt2");
        base.Exit();
    }
}
