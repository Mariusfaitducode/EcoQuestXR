using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{

    NavMeshAgent agent;
    State currentState;

    private void Start()
    {
       agent = this.GetComponent<NavMeshAgent>();
       currentState = new Idle(this.gameObject, agent);
    }
    private void Update()
    {
        currentState = currentState.Process();
    }
}
