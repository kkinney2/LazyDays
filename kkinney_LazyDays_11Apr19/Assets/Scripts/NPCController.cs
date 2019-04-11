using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour {

    public enum Job
    {
        None = 0,
        Lumberjack,
        Farmer
    }

    Job job;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AssignJob()
    {
        switch (job)
        {
            case Job.Lumberjack:
                gameObject.AddComponent<Lumberjack>();
                break;

            case Job.Farmer:
                gameObject.AddComponent<Farmer>();
                break;

            default:
                break;
        }
    }

    public void FindTaskObject()
    {
        // Find Object
    }

    public void Idle()
    {

    }
}
