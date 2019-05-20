using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskBehavior : MonoBehaviour {

    GameObject GameController;
    GameController gameController;

    bool TaskAvailable;
    public int TaskCooldown;

    string task;

	// Use this for initialization
	void Start () {
        if (GameController == null || gameController == null)
        {
            GameController = GameObject.Find("GameController");
            gameController = GameController.GetComponent<GameController>();
        }

        task = this.tag;

        StartCoroutine(TaskCoolDown());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool CanPerformTask()
    {
        return TaskAvailable;
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

        if (TaskCooldown > 0)
        {

        }
        yield return new WaitForSeconds(1);
    }
}
