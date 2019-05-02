using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject NPC;
    public Camera mainCamera;

    bool canSpawnNPC = true;

	// Use this for initialization
	void Start () {
        // References camera in scene tagged 'MainCamera'
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Keep mouse locked to center of screen
        Cursor.lockState = CursorLockMode.Locked;
    }
	
	// Update is called once per frame
	void Update () {
        if (canSpawnNPC)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo))
                {
                    Debug.Log("Ray hit: " + hitInfo.collider.gameObject.name);
                }
            }
        }
	}

    public void SpawnNPC(Vector3 pos)
    {
        Instantiate(NPC, pos, mainCamera.transform.rotation);
    }

    public void ToggleNPCSpawner()
    {
        canSpawnNPC = !canSpawnNPC;
    }
}
