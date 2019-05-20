using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Experience
{
    public int level;
    public float exp;
    public float expLvlMax;
    public float expPerTask;

    public void AddExp()
    {
        exp += expPerTask;
    }

    public void AddExp(float a_Exp)
    {
        exp += a_Exp;
    }

}

public class NPCController : MonoBehaviour {

    [Header("NPC Settings")]
    public GameObject Home;
    public int Sight_InitialRadius = 3;
    public float ExpPerTask;
    public bool isDebugging = false;
    
    GameObject GameController;
    [HideInInspector]
    public GameController gameController;

    [Header("")]

    [Header("Canvas References")]
    public GameObject Canvas;
    public Slider ExpSlider;
    public Text levelText;
    public Text ActionText;
    
    [Header("")]

    public GameObject JobMenu;
    public Experience myExp;

    [Header("Job Masks")]
    public LayerMask Farmer_Mask;
    public LayerMask Lumberjack_Mask;

    StateMachine stateMachine;
    string newJob;

    Pathfinding pathfinder;
    [HideInInspector]
    public Grid grid;
    List<Node> pathToTarget;
    bool movingToTarget = false;
    Coroutine MovingTowardsTarget;

    [HideInInspector]
    public Renderer rend;

    public bool isHome = false;
    bool performingTask = false;
    bool returnHome = false;

    int timeAtLocation = 0;

    GameObject currentTarget;

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

    Coroutine wander;
    Coroutine wanderForces;


    // Use this for initialization
    void Start () {
        if(GameController == null)
        {
            GameController = GameObject.Find("GameController");
            gameController = GameController.GetComponent<GameController>();
        }

        stateMachine = new StateMachine();

        grid = GameController.AddComponent<Grid>();
        grid.gridWorldSize = gameController.GridWorldSize;
        grid.nodeRadius = gameController.NodeRadius;
        grid.Distance = gameController.Distance;
        grid.WallMask = LayerMask.GetMask("Wall");
        grid.DrawGrid = gameController.DrawGridOnSelected;

        pathfinder = this.gameObject.AddComponent<Pathfinding>();
        pathfinder.GameController = GameController;
        pathfinder.owner = this;
        pathfinder.grid = grid;
        pathfinder.enabled = true;


        rend = GetComponent<Renderer>();
        Farmer_Mask = LayerMask.GetMask("Farmer");
        Lumberjack_Mask = LayerMask.GetMask("Lumberjack");

        myExp = new Experience
        {
            level = 1,
            exp = 0,
            expLvlMax = 5,
            expPerTask = gameController.ExpPerTask
        };

        UpdateExpUI();

        Canvas.gameObject.SetActive(false);


        wander = StartCoroutine(Wander());
        wanderForces = StartCoroutine(WanderForces());

        AssignJob("Farmer");

        StartCoroutine(TaskChecker());

        /*
        if (Random.Range(0,3) >=2)
        {
            AssignJob("Farmer");
        }
        else
        {
            AssignJob("Lumberjack");
        }*/
    }
	
	// Update is called once per frame
	void Update () {
        
        stateMachine.Update();

        if (myExp.exp >= myExp.expLvlMax)
        {
            myExp.level++;
            myExp.exp = myExp.exp - myExp.expLvlMax;
            myExp.expLvlMax = myExp.level * (myExp.expLvlMax + 1);

            UpdateExpUI();
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
            /*case "LumberJack":
                stateMachine.ChangeState(new Lumberjack(this));
                StartCoroutine(Search(Lumberjack_Mask));
                break;*/

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
            hitColliders_Sight = null;

            // If within 'sight' needs help idenifying
            hitColliders_Sight = Physics.OverlapSphere(transform.position, (myExp.level + Sight_InitialRadius) * 0.5f, a_Mask);
            //Debug.Log("Sight list length: " + hitColliders_Sight.Length);
            yield return new WaitForSeconds(0.1f);
        }

    }

    public GameObject SearchForTarget(string a_Tag)
    {
        GameObject tempTarget = ClosestObjSight(a_Tag);
        if (tempTarget == null)
        {
            isWandering = true;
        }
        if (tempTarget != null)
        {
            //Debug.Log("Found: " + tempTarget.name);
            isWandering = false;
            return tempTarget;
        }
        return null;
    }

    public GameObject ClosestObjSight(string a_Tag)
    {
        float shortestDistance = -1;
        GameObject closestObj = null;

        foreach (Collider collider in hitColliders_Sight)
        {
            if (collider.gameObject.CompareTag(a_Tag))
            {
                if (a_Tag == "Home")
                {
                    if (collider.gameObject == Home)
                    {
                        return Home;
                    }
                }
                else
                {
                    float tempDist = Vector3.Distance(collider.gameObject.transform.position, transform.position);
                    if (tempDist < shortestDistance || shortestDistance == -1)
                    {
                        shortestDistance = tempDist;
                        closestObj = collider.gameObject;
                    }
                }
                
            }
        }

        if (closestObj == null)
        {
            //Debug.Log("No Object within Sight with Tag: " + a_Tag);
        }

        return closestObj;
    }

    public void FindPath(GameObject a_Target)
    {
        Debug.Log("Finding Path");
        pathfinder.FindPathtoTarget(this.transform, a_Target.transform);
        pathToTarget = grid.FinalPath;
        
        /*
        pathfinder.StartPosition = null;
        pathfinder.TargetPosition = null;
        grid.FinalPath = null;*/
    }

    public string GetTaskTarget()
    {
        return stateMachine.GetTaskTarget();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == currentTarget)
        {
            HasReachedTarget();
        }
    }

    public void HasReachedTarget()
    {
        Debug.Log("HasReachedTarget()");
        timeAtLocation = 0;

        if (returnHome)
        {
            Debug.Log("Received exp");
            myExp.AddExp();
            UpdateExpUI();
            returnHome = false;
        }
        else returnHome = true;

        movingToTarget = false;
        currentTarget = null;
        pathToTarget = null;
        StopCoroutine(MovingTowardsTarget);
    }

    private void OnMouseEnter()
    {
        Canvas.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        Canvas.gameObject.SetActive(false);
    }

    void UpdateExpUI()
    {
        levelText.text = myExp.level.ToString();
        ExpSlider.value = myExp.exp;
        ExpSlider.maxValue = myExp.expLvlMax;
    }

    IEnumerator TaskChecker()
    {
        while (true)
        {
            if (returnHome && !performingTask)
            {
                //Debug.Log("Looking for Home");
                ActionText.text = "Looking for Home";
                currentTarget = SearchForTarget("Home");
                if (currentTarget != null)
                {
                    Debug.Log("Found Home");
                    performingTask = true;
                    FindPath(currentTarget);
                }
            }

            if (!returnHome && !performingTask)
            {
                // Find PlantBed
                //Debug.Log("Looking for Task: " + GetTaskTarget());
                ActionText.text = "Looking for " + stateMachine.GetTaskTarget();
                currentTarget = SearchForTarget(GetTaskTarget());
                if (currentTarget != null)
                {
                    performingTask = true;
                    Debug.Log("Found Task: " + GetTaskTarget());
                    FindPath(currentTarget);
                }
            }

            if (pathToTarget != null && movingToTarget == false)
            {
                movingToTarget = true;
                isWandering = false;
                MovingTowardsTarget = StartCoroutine(MoveToTarget());
            }

            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator PerformTask(string location, GameObject a_target)
    {
        int waitTime = 0;

        stateMachine.PerformTask(location, a_target);
        waitTime += stateMachine.GetTaskDuration(location);
        
        yield return new WaitForSeconds(waitTime);
        performingTask = false;
    }

    IEnumerator MoveToTarget()
    {
        Debug.Log("MovingToTarget()");
        while (Vector3.Distance(pathToTarget[pathToTarget.Count - 1].Position, transform.position) > 0.5f)
        {
            if (isDebugging)
            {
                for (int i = 0; i < pathToTarget.Count; i++)
                {
                    Debug.Log("Node " + i + ": " + pathToTarget[i].Position);
                }
                Debug.Log("*****");

                Debug.Log("Path Node Length: " + pathToTarget.Count);
            }
            

            for (int i = 0; i < pathToTarget.Count; i++)
            {
                Vector3 startPos = transform.position;
                float journeyLength = Vector3.Distance(pathToTarget[i].Position, transform.position);
                float t = 0;

                Vector3 newCurrentPos = new Vector3(
                        transform.position.x * 1,
                        transform.position.y,
                        transform.position.z * 1
                    );

                Vector3 newTargetPos = new Vector3(
                    pathToTarget[i].Position.x * 1,
                    transform.position.y,
                    pathToTarget[i].Position.z * 1
                );

                //while (Vector3.Distance(pathToTarget[pathToTarget.Count - 1].Position, transform.position) > 0.5f) Including a y-axis makes it "impossible" to reach
                while (Vector3.Distance(newCurrentPos, newTargetPos) > 0.5f)
                {
                    float step = Time.deltaTime * moveSpeed;
                    newCurrentPos = new Vector3(
                        transform.position.x * 1,
                        transform.position.y,
                        transform.position.z * 1
                    );

                    newTargetPos = new Vector3(
                        pathToTarget[i].Position.x * 1,
                        transform.position.y,
                        pathToTarget[i].Position.z * 1
                    );
                    transform.position = Vector3.MoveTowards(newCurrentPos, newTargetPos, step);

                    if (isDebugging)
                    {
                        Debug.Log("MovingTowards: " + newTargetPos);
                        //Debug.Log("MovingTowards: " + pathToTarget[i].Position);
                        Debug.Log("MovingTowards Loop: " + i);
                        Debug.Log("Remaining Distance: " + Vector3.Distance(newCurrentPos, newTargetPos));
                        //Debug.Log("Remaining Distance: " + Vector3.Distance(transform.position, pathToTarget[i].Position));
                        //Debug.Log("t: " + t);
                        Debug.Log("*****");
                    }

                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForEndOfFrame();
            }
            Debug.Log("Reached Target");
            if (transform.position == pathToTarget[pathToTarget.Count -1].Position)
            {
                HasReachedTarget();
                yield break;
            }
            yield return new WaitForSeconds(1);
        }
        movingToTarget = false;
        pathToTarget = null;
    }

    IEnumerator Wander()
    {
        while (true)
        {
            while (isWandering)
            {
                if (isDebugging)
                {
                    Debug.Log("IsWandering");
                }
                
                int rotTime = Random.Range(1, 3); // Amount of time rotating
                int rotateWeight = Random.Range(1, maxRotBetweenTime); // Time inbetween rotations
                int rotateLorR = Random.Range(0, 3); // Left or Right
                int walkWeight = Random.Range(1, 4); // Time inbetween Walking
                int walkTime = Random.Range(1, maxWalkTime); // Walking Time

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

                yield return new WaitForSeconds(walkWeight);
                isWalking = true;

                yield return new WaitForSeconds(walkTime);
                isWalking = false;
                
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator WanderForces()
    {
        while (true)
        {
            while (isWandering)
            {
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

                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }
    }
}