using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantRowBehaviour : MonoBehaviour {

    public int RowNumber = 0;

    GameObject plantBed;
    TaskBehavior plantBedBehaviour;


	// Use this for initialization
	void Start () {
        plantBed = gameObject.transform.parent.gameObject;
        plantBedBehaviour = plantBed.GetComponent<TaskBehavior>();
    }
	
	// Update is called once per frame
	void Update () {
        if (plantBedBehaviour.TaskCooldown > 0)
        {
            //float newY = Mathf.Lerp(-1f, 0f, (Mathf.Abs(plantBedBehaviour.TaskCooldown - plantBedBehaviour.taskCooldownMax) / plantBedBehaviour.taskCooldownMax));
            //gameObject.transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            for (int i = 0; i < transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        else
        {
            //transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            for (int i = 0; i < transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(true);
            }
                
        }

    }
}
