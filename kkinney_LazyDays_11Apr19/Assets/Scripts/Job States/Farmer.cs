using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface structure sourced from:
// https://forum.unity.com/threads/c-proper-state-machine.380612/ (Unity Forum)
// https://github.com/libgdx/gdx-ai/wiki/State-Machine (Mat Buckland)

public class Farmer : IState
{
    NPCController owner;

    string taskTarget = "PlantBed";
    bool hasFood = false;
    int foodCarryingNum = 0;
    int taskDuration = 2;

    public Farmer(NPCController newOwner)
    {
        this.owner = newOwner;
    }

    public void Enter()
    {
        Debug.Log("Entering State: Farmer");
        owner.rend.material.shader = Shader.Find("_Color");
        owner.rend.material.SetColor("_Color", Color.magenta);

        owner.rend.material.shader = Shader.Find("Specular");
        owner.rend.material.SetColor("_SpecColor", Color.red);
    }

    public void Execute()
    {/*
        if (hasFood && owner.)
        {

        }*/
    }

    public void Exit()
    {
        Debug.Log("Exiting State: Farmer");
    }

    public string GetTaskTarget()
    {
        return taskTarget;
    }

    public void PerformTask(string a_location, GameObject a_target)
    {
        a_target.GetComponent<TaskBehavior>().PerformTask(this);
        owner.ActionText.text = "Harvesting Crops";
    }

    public int GetFoodCarryingNum()
    {
        return foodCarryingNum;
    }

    public void AddFoodCarryingNum(int new_Value)
    {
        foodCarryingNum = foodCarryingNum + new_Value;
    }

    public int GetNPCLevel()
    {
        return owner.myExp.level;
    }
}