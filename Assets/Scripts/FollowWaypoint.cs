using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWaypoint : MonoBehaviour
{
    public GameObject[] waypoints;
    int currentWaypoint = 0;

    public float speed = 10f;
    public float rotSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].transform.position) < 3)
        {
            currentWaypoint++;
        }
        if (currentWaypoint >= waypoints.Length)//For loop
            currentWaypoint = 0;

        //transform.LookAt(waypoints[currentWaypoint].transform);

        Quaternion lookatWP = Quaternion.LookRotation(waypoints[currentWaypoint].transform.position - this.transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookatWP, rotSpeed * Time.deltaTime);

        transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
