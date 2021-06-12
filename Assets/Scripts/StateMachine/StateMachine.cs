using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private State currentState;
    public State CurrentState
    {
        get => currentState;
        set
        {
            if(currentState != null)
            {
                currentState.Exit();
            }

            currentState = value;

            if(currentState != null)
            {
                currentState.Enter();
            }
        }
    }
}
