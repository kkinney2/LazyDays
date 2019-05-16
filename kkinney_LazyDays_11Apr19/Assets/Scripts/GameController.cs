using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject NPC;
    public GameObject PlantBed;
    public GameObject Tree;
    public Camera mainCamera;

    bool canSpawnObj = false;
    bool objSpawned = false;
    GameObject spawnedObj;
    GameObject objToSpawn;

    public enum SpawnList
    {
        // TODO: Create array for obj's to spawn
        NPC = 0,
        PlantBed,
        Tree
    }

    // Use this for initialization
    void Start () {

        // References camera in scene tagged 'MainCamera'
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Keep mouse locked to center of screen
        // Use 'esc' to break in editor play mode
        Cursor.lockState = CursorLockMode.Locked;
    }
	
	// Update is called once per frame
	void Update () {
        if (canSpawnObj)
        {
            // What Obj to spawn
            //     >> Buttons will assign Obj

            // Determine if space is acceptable
            //     >> Spawn Obj and move with raycast?

            // 'Spawn' object
            //     >> Release Obj from camera if acceptable, else destroy

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                Debug.Log("Ray hit: " + hitInfo.collider.gameObject.name);

                // TODO: Obj following camera for spawning
                // Spawn the Obj
                if (!objSpawned)
                {
                    spawnedObj = objToSpawn;
                    SpawnObj(spawnedObj, hitInfo.point);
                    objSpawned = true;
                }
                else
                {
                    spawnedObj.transform.position = hitInfo.point;
                }

                // Basic Framework for Spawning
                /* << WORKS >>
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                    RaycastHit hitInfo;

                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        Debug.Log("Ray hit: " + hitInfo.collider.gameObject.name);

                        // Spawn the Obj
                        if (true)
                        {
                            SpawnObj(objToSpawn, hitInfo.point);
                        }
                    }
                }*/
            }
        }
	}

    void SetObjToSpawn(SpawnList obj)
    {
        switch (obj)
        {
            case SpawnList.NPC:
                objToSpawn = NPC;
                break;
            case SpawnList.PlantBed:
                objToSpawn = PlantBed;
                break;
            case SpawnList.Tree:
                objToSpawn = Tree;
                break;
            default:
                Debug.Log("Null or No such Item to Spawn");
                break;
        }
    }

    void SpawnObj(GameObject obj, Vector3 pos)
    {
        Instantiate(NPC, pos, Quaternion.identity);
    }

    public void ToggleObjSpawner()
    {
        canSpawnObj = !canSpawnObj;
    }
}
