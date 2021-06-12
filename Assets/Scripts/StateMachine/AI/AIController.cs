using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private StateMachine sm;
    void Start()
    {
        sm = new StateMachine();
        //starting state
        //sm.CurrentState = new State(gameObject, sm)
    }

    void Update()
    {
        //sm.CurrentState.Update();
    }

    void FixedUpdate()
    {
        //sm.CurrentState.FixedUpdate();
    }
}
