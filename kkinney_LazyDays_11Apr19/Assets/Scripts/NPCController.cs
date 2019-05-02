using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour {

    public GameObject JobMenu;
    StateMachine stateMachine;
    string newJob;

	// Use this for initialization
	void Start () {
        stateMachine = new StateMachine();
	}
	
	// Update is called once per frame
	void Update () {
        /* TODO: Commented out due to unfinished work
        if (stateMachine.getCurrentState() == null)
        {
            // Ask user for Job
            StartCoroutine(ObtainJob());
            JobMenu.gameObject.SetActive(false);
            StopCoroutine(ObtainJob());

            if (newJob != null || newJob != "")
            {
                // Assign new Job
                AssignJob(newJob);
            }
            
        }*/
        stateMachine.Update();
	}

    IEnumerator ObtainJob()
    {
        while (true)
        {
            if(JobMenu.gameObject.activeSelf == false)
            {
                JobMenu.gameObject.SetActive(true);
            }
            if(newJob != null || newJob != "")
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }

    void AssignJob(string state)
    {

        switch (state)
        {
            case "LumberJack":
                stateMachine.ChangeState(new Lumberjack(this));
                break;

            case "Farmer":
                stateMachine.ChangeState(new Farmer(this));
                break;

            default:
                Debug.Log("State doesn't exist or has no case");
                break;
        }
    }

    void Logic()
    {

    }
}