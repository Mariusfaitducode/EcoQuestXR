using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{

    NavMeshAgent agent;
    State currentState;
    private Animator animator;

    private void Start()
    {
       agent = this.GetComponent<NavMeshAgent>();
       animator = this.GetComponent<Animator>();
       currentState = new Idle(this.gameObject, agent, animator);
    }
    private void Update()
    {
        currentState = currentState.Process();
    }
}
