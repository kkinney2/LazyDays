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
            
            // If object within really close radius, can function individually
            Collider[] hitColliders_Close = Physics.OverlapSphere(owner.transform.position, owner.myExp.level * 0.5f);

            // If within 'sight' needs help idenifying
            Collider[] hitColliders_Sight = Physics.OverlapSphere(owner.transform.position, owner.myExp.level * 0.5f);
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