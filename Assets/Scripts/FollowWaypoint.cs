using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowWaypoint : MonoBehaviour
{
    public GameObject track;
    int currentWaypoint = 0;

    public float speed = 10f;
    public float rotSpeed = 10f;
    public bool loop = false;

    public float sideMovement = 0f;
    public Transform sled;
    public Slider progress;

    List<Transform> waypoints = new List<Transform>();
    bool finished = false;
    float distToNext;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in track.transform)
        {
            waypoints.Add(child);
        }
        distToNext = Vector3.Distance(transform.position, waypoints[currentWaypoint].position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!finished)
        {
            progress.value = Mathf.Max(currentWaypoint - 1, 0) / (float)waypoints.Count;
            if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 3)
            {
                distToNext = Vector3.Distance(transform.position, waypoints[currentWaypoint].position);
                currentWaypoint++;
            }
            if (currentWaypoint >= waypoints.Count)//For loop
                if (loop) currentWaypoint = 0;
                else finished = true;

            //transform.LookAt(waypoints[currentWaypoint].transform);

            Quaternion lookatWP = Quaternion.LookRotation(waypoints[currentWaypoint].position - this.transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookatWP, rotSpeed * Time.deltaTime);
            transform.Translate(0, 0, speed * Time.deltaTime);

            float h = sideMovement * Input.GetAxis("Horizontal");
            sled.localPosition = new Vector3(Mathf.Clamp(sled.localPosition.x + (h * Time.deltaTime), -7f, 7f), 0, 0);
        }
    }
}
