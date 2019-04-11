using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject NPC;
    public GameObject Camera;

    bool canSpawnNPC = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnNPC(Vector3 pos)
    {
        Instantiate(NPC, pos, Camera.transform.rotation);
    }

    public void ToggleNPCSpawner()
    {
        canSpawnNPC = !canSpawnNPC;
    }
}
