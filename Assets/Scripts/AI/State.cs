using System.Collections;
using System.Collections.Generic;
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


    public State(GameObject _npc, NavMeshAgent _agent)
    {
        npc = _npc;
        agent = _agent;
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
    public Idle(GameObject _npc, NavMeshAgent _agent) : base(_npc, _agent)
    {
        name = STATE.IDLE; 
    }

    public override void Enter()
    {
        //Play Idle animation
        base.Enter();
    }

    public override void Update()
    {
        if(Random.Range(0, 100) < 10)
        {
            nextState = new Patrol(npc, agent);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        //Reset animations
        base.Exit();
    }
}

public class Patrol : State
{
    int currentIndex = -1;

    public Patrol(GameObject _npc, NavMeshAgent _agent) : base(_npc, _agent)
    {
        name = STATE.PATROL;
    }

    public override void Enter()
    {
        //Play Idle animation
        currentIndex = 0;
        base.Enter();
    }

    public override void Update()
    {
        if(agent.remainingDistance < 1)
        {
            if(currentIndex > GameEnvironment.Singleton.Waypoints.Count) 
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }
            agent.SetDestination(GameEnvironment.Singleton.Waypoints[currentIndex].transform.position);
        }
    }

    public override void Exit()
    {
        //Reset animations
        base.Exit();
    }
}
