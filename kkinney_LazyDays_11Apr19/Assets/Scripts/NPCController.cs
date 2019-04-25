using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour {

    StateMachine stateMachine;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        stateMachine.Update();
	}
}

public class Find : IState
{

    NPCController owner;
    StateMachine stateMachine;

    public Find(NPCController owner) { this.owner = owner; }

    public void Enter()
    {
        Debug.Log("Entering State: Find ");
    }

    public void Execute()
    {
        Debug.Log("Updating State: Find");
        stateMachine.Update();
    }

    public void Exit()
    {
        Debug.Log("Exiting State: Find");
    }
}


