using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowWaypoint : MonoBehaviour
{
    [Header("Track Settings")]
    public GameObject track;
    int currentWaypoint = 0;

    [Header("Traversal Settings")]
    public float speed = 10f;
    public float rotSpeed = 10f;
    public bool loop = false;
    public float sideMovement = 0f;
    public float maxHorizontalDisplacement = 10f;
    public Transform sled;

    [Header("Gameplay Settings")]
    public Slider progress;
    public GameObject winScene;
    public GameObject UI;
    public GameObject loseScene;

    List<Transform> waypoints = new List<Transform>();
    bool finished = false;
    bool lost = false;
    float distToNext;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in track.transform)
        {
            waypoints.Add(child);
        }
        distToNext = Vector3.Distance(transform.position, waypoints[currentWaypoint].position);
        UI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!finished && !lost)
        {
            if (progress != null)
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

            if (sideMovement != 0 && sled != null)
            {
                float h = sideMovement * Input.GetAxis("Horizontal");
                sled.localPosition = new Vector3(Mathf.Clamp(sled.localPosition.x + (h * Time.deltaTime), -maxHorizontalDisplacement, maxHorizontalDisplacement), 0, 0);
            }
        }
        ManageUI();
    }

    void ManageUI()
    {
        if (finished)
        {
            UI.SetActive(false);
            winScene.SetActive(true);
        }
        if (lost)
        {
            UI.SetActive(false);
            loseScene.SetActive(true);
        }
    }

    public void ActiveLoss()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        lost = true;
    }
}
