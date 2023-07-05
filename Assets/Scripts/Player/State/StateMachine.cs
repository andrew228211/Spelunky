using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class StateMachine
{
    public State currentState { get; private set; }
    public State previosState { get; private set; }

    public void ChangeState(State newState)
    {
        if (currentState != null)
        {
            if (newState == currentState)
            {
                return;
            }
            if (!newState.CanEnter())
            {
                return;
            }
            if (currentState.GetType() == typeof(DieState))
            {
                return;
            }
            currentState.Exit();
            previosState = currentState;
        }
        currentState = newState;        
        currentState.Enter();
    }
}
