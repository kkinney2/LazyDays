using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour {

    private GameObject target;

    private void Start()
    {
        target = GameObject.Find("Player");
    }
    // Update is called once per frame
    void Update () {
        this.transform.LookAt(target.transform.position);
    }
}
