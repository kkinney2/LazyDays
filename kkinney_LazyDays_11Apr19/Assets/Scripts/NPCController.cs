using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience
{
    public int level;
    public float exp;
    public float expLvlMax;

}

public class NPCController : MonoBehaviour {

    public GameObject JobMenu;
    public Experience myExp;

    StateMachine stateMachine;
    string newJob;
    

	// Use this for initialization
	void Start () {
        stateMachine = new StateMachine();

        myExp.level = 1;
        myExp.exp = 0;
        myExp.expLvlMax = 5;
	}
	
	// Update is called once per frame
	void Update () {
        /* TODO: Commented out due to unfinished work
        if (stateMachine.getCurrentState() == null)
        {
            // Ask user for Job
            TODO: Unlock mouse or numpad for options
            Cursor.lockState = CursorLockMode.???;
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

        if(myExp.exp >= myExp.expLvlMax)
        {
            myExp.level++;
            myExp.exp = 0;
            myExp.expLvlMax = myExp.level * (myExp.expLvlMax + 1);
        }
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