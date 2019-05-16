using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [Header("NPC Settings")]
    public int Touch_InitialRadius = 3;
    public int Sight_InitialRadius = 10;
    [Header("")]

    [Header("Grid Settings")]
    public Vector2 GridWorldSize;
    public float NodeRadius;
    public float Distance;

    [Header("")]
    public GameObject NPC;
    public GameObject PlantBed;
    public GameObject Tree;
    public Camera mainCamera;
    public LayerMask hitLayers;

    bool canSpawnObj = true;
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
        if (Input.GetButton("Cancel"))
        {
            Application.Quit();
        }

        if (canSpawnObj)
        {
            // What Obj to spawn
            //     >> Buttons will assign Obj

            // Determine if space is acceptable
            //     >> Spawn Obj and move with raycast?

            // 'Spawn' object
            //     >> Release Obj from camera if acceptable, else destroy

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, hitLayers))
                {
                    Debug.Log("Ray hit: " + hitInfo.collider.gameObject.name);

                    // TODO: Obj following camera for spawning
                    // Spawn the Obj
                    if (!objSpawned)
                    {
                        spawnedObj = objToSpawn;

                        

                        spawnedObj = SpawnObj(objToSpawn, hitInfo.point);

                        if (spawnedObj.GetComponent<NPCController>())
                        {
                            NPCController npc = spawnedObj.GetComponent<NPCController>();
                            npc.Touch_InitialRadius = Touch_InitialRadius;
                            npc.Sight_InitialRadius = Sight_InitialRadius;
                        }
                        objSpawned = true;
                    }
                }

                if (canSpawnObj)
                {
                    objSpawned = false;
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

    /*void SpawnObj(GameObject obj, Vector3 pos)
    {
        Instantiate(NPC, pos, Quaternion.identity);
    }*/

    GameObject SpawnObj(GameObject obj, Vector3 pos)
    {
        return Instantiate(NPC, pos, Quaternion.identity);
    }

    public void ToggleObjSpawner()
    {
        canSpawnObj = !canSpawnObj;
    }
}
