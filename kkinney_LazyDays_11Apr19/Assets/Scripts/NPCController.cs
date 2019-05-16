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

    [Header("Job Masks")]
    public LayerMask Farmer_Mask;
    public LayerMask Lumberjack_Mask;

    StateMachine stateMachine;
    string newJob;

    Collider[] hitColliders_Touch;
    Collider[] hitColliders_Sight;

    [Header("Wandering Settings")]
    public float moveSpeed = 3f;
    public float rotSpeed = 100f;
    public int maxWalkTime = 5;
    public int maxRotBetweenTime = 4;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;


    // Use this for initialization
    void Start () {
        stateMachine = new StateMachine();
        Farmer_Mask = LayerMask.GetMask("Farmer");
        Lumberjack_Mask = LayerMask.GetMask("Lumberjack");

        myExp = new Experience
        {
            level = 1,
            exp = 0,
            expLvlMax = 5
        };
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
                StartCoroutine(Search(Lumberjack_Mask));
                break;

            case "Farmer":
                stateMachine.ChangeState(new Farmer(this));
                StartCoroutine(Search(Farmer_Mask));
                break;

            default:
                Debug.Log("State doesn't exist or has no case");
                break;
        }
    }

    IEnumerator Search(LayerMask a_Mask)
    {
        while (true)
        {
            // If object within really close radius, can function individually
            hitColliders_Touch = Physics.OverlapSphere(transform.position, myExp.level * 0.5f, a_Mask);

            // If within 'sight' needs help idenifying
            hitColliders_Sight = Physics.OverlapSphere(transform.position, myExp.level * 0.5f, a_Mask);

            yield return new WaitForSeconds(1);
        }

    }

    public GameObject SearchTargetInteractive(string a_Tag)
    {
        GameObject tempTarget = ClosestObjTouch(a_Tag);
        if (tempTarget == null)
        {
            tempTarget = ClosestObjSight(a_Tag);
            if (tempTarget == null)
            {
                Debug.Log("Cannot Find: " + a_Tag);
            }
        }
        return tempTarget;
    }

    public GameObject ClosestObjTouch(string a_Tag)
    {
        float shortestDistance = -1;
        GameObject closestObj = null;

        foreach (Collider collider in hitColliders_Touch)
        {
            if (collider.gameObject.CompareTag(a_Tag))
            {
                float tempDist = Vector3.Distance(collider.gameObject.transform.position, transform.position);
                if (tempDist < shortestDistance || shortestDistance == -1)
                {
                    shortestDistance = tempDist;
                    closestObj = collider.gameObject;
                }
            }
        }

        if (closestObj == null)
        {
            Debug.Log("No Object within Touch with Tag: " + a_Tag);
        }

        return closestObj;
    }

    public GameObject ClosestObjSight(string a_Tag)
    {
        float shortestDistance = -1;
        GameObject closestObj = null;

        foreach (Collider collider in hitColliders_Sight)
        {
            if (collider.gameObject.CompareTag(a_Tag))
            {
                float tempDist = Vector3.Distance(collider.gameObject.transform.position, transform.position);
                if (tempDist < shortestDistance || shortestDistance == -1)
                {
                    shortestDistance = tempDist;
                    closestObj = collider.gameObject;
                }
            }
        }

        if (closestObj == null)
        {
            Debug.Log("No Object within Sight with Tag: " + a_Tag);
        }

        return closestObj;
    }

    IEnumerator Wander()
    {
        int rotTime = Random.Range(1, 3); // Amount of time rotating
        int rotateWeight = Random.Range(1, maxRotBetweenTime; // Time inbetween rotations
        int rotateLorR = Random.Range(1, 2); // Left or Right
        int walkWeight = Random.Range(1, 4); // Time inbetween Walking
        int walkTime = Random.Range(1, maxWalkTime); // Walking Time

        isWandering = true;

        yield return new WaitForSeconds(walkWeight);
        isWalking = true;

        yield return new WaitForSeconds(walkTime);
        isWalking = false;

        yield return new WaitForSeconds(rotateWeight);
        if (rotateLorR == 1)
        {
            isRotatingRight = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingRight = false;
        }
        if (rotateLorR == 2)
        {
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingLeft = false;
        }
        isWandering = false;
    }

    IEnumerator WanderForces()
    {
        while (true)
        {
            if (isWandering == false)
            {
                yield break;
            }
            if (isRotatingRight)
            {
                transform.Rotate(transform.up * Time.deltaTime * rotSpeed);
            }
            if (isRotatingLeft)
            {
                transform.Rotate(transform.up * Time.deltaTime * -rotSpeed);
            }
            if (isWalking)
            {
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
        }
    }
}