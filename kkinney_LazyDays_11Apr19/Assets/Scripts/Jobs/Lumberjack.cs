using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface structure sourced from:
// https://forum.unity.com/threads/c-proper-state-machine.380612/ (Unity Forum)
// https://github.com/libgdx/gdx-ai/wiki/State-Machine (Mat Buckland)

public class Lumberjack : IState
{
    NPCController owner;
    StateMachine stateMachine;

    public Lumberjack(NPCController owner) { this.owner = owner; }

    public void Enter()
    {
        Debug.Log("Entering State: Lumberjack ");
    }

    public void Execute()
    {
        Debug.Log("Updating State: Lumberjack");
        stateMachine.Update();
    }

    public void Exit()
    {
        Debug.Log("Exiting State: Lumberjack");
    }
}