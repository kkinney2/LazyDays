using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface structure sourced from:
// https://forum.unity.com/threads/c-proper-state-machine.380612/ (Unity Forum)
// https://github.com/libgdx/gdx-ai/wiki/State-Machine (Mat Buckland)

public class Farmer : IState
{
    NPCController owner;

    bool willHarvest = false;
    bool returnHome = false;

    string targetInteractive = "PlantBed";
    GameObject currentTarget;

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
        if (willHarvest || !returnHome)
        {
            // Find PlantBed
            currentTarget = owner.SearchTargetInteractive(targetInteractive);
            if (currentTarget != null && !willHarvest)
            {
                willHarvest = true;
                owner.FindPath(currentTarget);
            }
        }


        // Do what?
        //      Harvest();
        //      ReturnHome();


        // How to do it?
        //      Find(PlantBed); GoTo(PlantBed);
        //      GoTo(Home);

        // Perform it
        //      Harvest();

        // Now what?
        //      FindNewTask();
    }

    public void Exit()
    {
        Debug.Log("Exiting State: Farmer");
    }

    IEnumerator ObjDetectSphere()
    {

        yield return new WaitForSeconds(1);
    }

}