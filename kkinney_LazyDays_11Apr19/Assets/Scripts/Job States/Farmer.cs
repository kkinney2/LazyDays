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

    public int GetTaskDuration(string a_task)
    {

        if (a_task == "Home")
        {
            taskDuration = 1;
        }

        if (a_task == "Task")
        {
            taskDuration = 2;
        }
        return taskDuration;
    }

    public void PerformTask(string a_location, GameObject a_target)
    {
        if (a_location == "Home")
        {
            a_target.GetComponent<TaskBehavior>().CanPerformTask();
        }

        if (a_location == "Task")
        {
            a_target.GetComponent<TaskBehavior>();
        }
    }
}