using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    [Header("NPC Settings")]
    public int Sight_InitialRadius = 10;
    public float ExpPerTask;
    public int FoodReq;

    [Header("")]

    [Header("Task Settings")]
    public int PlantBed_FoodYield;

    [Header("")]

    [Header("Grid Settings")]
    public Vector2 GridWorldSize;
    public float NodeRadius;
    public float Distance;
    public bool DrawGridOnSelected;

    [Header("")]
    public GameObject NPC;
    ArrayList NPCs;
    public GameObject PlantBed;
    public GameObject Tree;
    public Camera mainCamera;
    public LayerMask hitLayers;
    public int FoodCount;
    public Text FoodCountUI;
    public Text TimeUI;

    bool canSpawnObj = false;
    bool objSpawned = false;
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

        NPCs = new ArrayList(); // Need to instantiate for storage

        // References camera in scene tagged 'MainCamera'
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Keep mouse locked to center of screen
        // Use 'esc' to break in editor play mode
        Cursor.lockState = CursorLockMode.Locked;

        // Game Order

        GameObject spawnedObj = Instantiate(NPC, GameObject.Find("NPC_Spawn").transform.position, Quaternion.identity) as GameObject;
        NPCs.Add(spawnedObj);
        NPCController npc = spawnedObj.GetComponent<NPCController>();
        npc.Sight_InitialRadius = 5;
        npc.Level = 10;

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButton("Cancel"))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        UpdateUI();

        if (FoodCount >= FoodReq)
        {
            FoodCount = FoodCount - FoodReq;
            FoodReq = FoodReq + 30 * NPCs.Count;
            GameObject temp = GameObject.Find("NPC_Spawn");
            GameObject spawnedObj = Instantiate(NPC, GameObject.Find("NPC_Spawn").transform.position, Quaternion.identity) as GameObject;
            NPCs.Add(spawnedObj);
            NPCController npc = spawnedObj.GetComponent<NPCController>();
            npc.Sight_InitialRadius = Sight_InitialRadius;
            npc.Level = 1;
            npc.moveSpeed = Random.Range(1f,5f);
            npc.rotSpeed = Random.Range(50f, 150f);
            npc.maxWalkTime = Random.Range(1, 5);
            npc.maxRotBetweenTime = Random.Range(1,5);
}
        /*
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
                }
            }
        }*/
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
        return Instantiate(obj.gameObject, pos, Quaternion.identity) as GameObject;
    }

    public void ToggleObjSpawner()
    {
        canSpawnObj = !canSpawnObj;
    }

    public void AddFood(int new_Value)
    {
        FoodCount = FoodCount + new_Value;
        UpdateUI();
    }

    public void UpdateUI()
    {
        FoodCountUI.text = FoodCount + " / " + FoodReq;
        if (Time.realtimeSinceStartup > 60)
        {
            TimeUI.text = Mathf.RoundToInt(Time.realtimeSinceStartup /60f) + " minutes  " + (Mathf.RoundToInt(Time.realtimeSinceStartup * 10f) / 10f)%60 + " seconds";
        }
        else TimeUI.text = (Mathf.RoundToInt(Time.realtimeSinceStartup * 10f) / 10f) + " seconds";
    }
}
