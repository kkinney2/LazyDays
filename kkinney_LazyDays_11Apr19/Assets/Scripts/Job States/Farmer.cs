using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface structure sourced from:
// https://forum.unity.com/threads/c-proper-state-machine.380612/ (Unity Forum)
// https://github.com/libgdx/gdx-ai/wiki/State-Machine (Mat Buckland)

public class Farmer : IState
{
    NPCController owner;

    public Farmer(NPCController newOwner)
    {
        this.owner = newOwner;
    }

    public void Enter()
    {
        Debug.Log("Entering State: Farmer");
    }

    public void Execute()
    {

    }

    public void Exit()
    {
        Debug.Log("Exiting State: Farmer");
    }

}
