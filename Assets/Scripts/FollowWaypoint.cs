using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public GameObject startScene;
    public GameObject winScene;
    public GameObject UI;
    public GameObject loseScene;
    bool started = false;

    [Header("Score Win Settings")]
    public bool finishByScore = false;
    public float goal = 20f;
    public TMP_Text scoreDisp;
    private float score = 0f;

    List<Transform> waypoints = new List<Transform>();
    bool finished = false;
    bool lost = false;
    float distToNext;
    Movement movementHandler;




    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in track.transform)
        {
            waypoints.Add(child);
        }
        distToNext = Vector3.Distance(transform.position, waypoints[currentWaypoint].position);
        movementHandler = FindFirstObjectByType<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (started && !finished && !lost)
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
        if (!started && startScene != null)
        {
            startScene.SetActive(true);
            movementHandler.toggleLock(true);
        } 
        else if (finished)
        {
            startScene.SetActive(false);
            UI.SetActive(false);
            winScene.SetActive(true);
            movementHandler.toggleLock(true);
        } 
        else if (lost)
        {
            startScene.SetActive(false);
            UI.SetActive(false);
            loseScene.SetActive(true);
            movementHandler.toggleLock(true);
        }
        else
        {
            startScene.SetActive(false);
            UI.SetActive(true);
            movementHandler.toggleLock(false);
        }
        if (finishByScore)
        {
            scoreDisp.text = score + "/" + goal;
        }
    }

    public void ActiveLoss()
    {
        lost = true;
    }

    public void StartLevel()
    {
        started = true;
    }

    public void IncreaseScore()
    {
        score++;
        if (score >= goal) finished = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            Debug.Log("HIT BY ENEMY");
        }
    }
}
