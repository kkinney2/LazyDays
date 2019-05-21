using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskBehavior : MonoBehaviour {

    GameObject GameController;
    GameController gameController;

    bool TaskAvailable;
    public int TaskCooldown;
    public int taskCooldownMax;
    public Slider CooldownSlider;

    public int TaskDuration;

    string task;

	// Use this for initialization
	void Start () {
        if (GameController == null || gameController == null)
        {
            GameController = GameObject.Find("GameController");
            gameController = GameController.GetComponent<GameController>();
        }

        task = this.tag;

        if (task == "PlantBed")
        {
            taskCooldownMax = TaskCooldown;
            CooldownSlider.maxValue = taskCooldownMax;
        }
        

        StartCoroutine(TaskCoolDown());
	}
	
	// Update is called once per frame
	void Update () {
        if (task == "PlantBed")
        {
            CooldownSlider.value = TaskCooldown;
        }
	}

    public bool CanPerformTask()
    {
        return TaskAvailable;
    }

    public void PerformTask(Farmer owner)
    {
        if (task == "Home")
        {
            Debug.Log("FoodStockpiled: " + owner.GetFoodCarryingNum());
            gameController.AddFood(owner.GetFoodCarryingNum());
            owner.AddFoodCarryingNum(-owner.GetFoodCarryingNum());
        }

        if(task == "PlantBed")
        {
            owner.AddFoodCarryingNum(Mathf.RoundToInt(gameController.PlantBed_FoodYield * owner.GetNPCLevel() / 2f));
            //Debug.Log("Food to be added:" + Mathf.RoundToInt(gameController.PlantBed_FoodYield * owner.GetNPCLevel() / 2f));
            //Debug.Log("Food Carried:" + owner.GetFoodCarryingNum());
        }
        TaskCooldown = taskCooldownMax;
    }

    IEnumerator TaskCoolDown()
    {
        while (true)
        {
            if (TaskCooldown > 0)
            {
                TaskAvailable = false;
                TaskCooldown--;
            }
            else TaskAvailable = true;
            yield return new WaitForSeconds(1);
        }
    }

    public int GetTaskDuration()
    {
        return TaskDuration;
    }
}
