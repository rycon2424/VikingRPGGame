using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public State currentState;
    public List<State> allStates = new List<State>();
    bool stateExists;
    public void GoToState(PlayerBehaviour pb, string newstate)
    {
        stateExists = false;
        foreach (var s in allStates)
        {
            if (s.GetType().ToString() == newstate)
            {
                stateExists = true;
                break;
            }
        }
        if (stateExists == false)
        {
            Debug.LogWarning("State " + "'" + newstate + "'" + " doesnt exist");
            return;
        }
        if (currentState != null)
        {
            if (currentState.GetType().ToString() == newstate)
            {
                Debug.LogWarning("State " + "'" + newstate + "'" + " is already the current state");
                return;
            }
        }
        if (currentState != null)
        {
            currentState.OnStateExit(pb);
        }
        foreach (var s in allStates)
        {
            if (s.GetType().ToString() == newstate)
            {
                currentState = s;

                pb.ChangeState(currentState);

                s.OnStateEnter(pb);
                //Debug.Log("Changed state to " + currentState.GetType().ToString());
                return;
            }
        }
    }

    public State CurrentState()
    {
        return currentState;
    }

    public bool IsInState(string state)
    {
        if (state == currentState.GetType().ToString())
        {
            return true;
        }
        return false;
    }
}