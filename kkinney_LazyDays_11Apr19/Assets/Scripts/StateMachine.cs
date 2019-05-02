using System.Collections;
using System.Collections.Generic;

// Interface structure sourced from:
// https://forum.unity.com/threads/c-proper-state-machine.380612/ (Unity Forum)
// https://github.com/libgdx/gdx-ai/wiki/State-Machine (Mat Buckland)

public interface IState
{
    void Enter();
    void Execute();
    void Exit();
}
public class StateMachine
{
    IState currentState;
    IState[] pastStates;

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
            //pastStates[pastStates.Length] = currentState;
        }

        currentState = newState;
        currentState.Enter();
    }

    // Bool return allows to check if it is possible or not
    public bool revertToPreviousState()
    {
        if (pastStates.Length > 0)
        {
            IState previousState = pastStates[pastStates.Length - 1];
            pastStates[pastStates.Length - 1] = null;
            ChangeState(previousState);
            return true;
        }
        else return false;
    }

    public IState getCurrentState()
    {
        return currentState;
    }

    public bool isInState(IState state)
    {
        if (currentState == state)
        {
            return true;
        }
        else return false;
    }

    public void Update()
    {
        if (currentState != null) currentState.Execute();
    }
}

