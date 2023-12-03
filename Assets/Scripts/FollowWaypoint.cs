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
        //Gets all the different points in the track and stores them as waypoints
        foreach (Transform child in track.transform)
        {
            waypoints.Add(child);
        }
        distToNext = Vector3.Distance(transform.position, waypoints[currentWaypoint].position);
        //Saves reference to Movement Hanlder, which controls mouse movement
        movementHandler = FindFirstObjectByType<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        //If playing
        if (started && !finished && !lost)
        {
            //If ui has a progress bar, this updates the current progress
            if (progress != null)
                progress.value = Mathf.Max(currentWaypoint - 1, 0) / (float)waypoints.Count;
            //If within a certain distance of the current waypoint, start going to next one.
            if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 3)
            {
                distToNext = Vector3.Distance(transform.position, waypoints[currentWaypoint].position);
                currentWaypoint++;
            }
            //If loop is set to true, it will head back to the first waypoint
            //Otherwise, getting to the final waypoint marks it as finished and the win ui will be displayed
            if (currentWaypoint >= waypoints.Count)
                if (loop) currentWaypoint = 0;
                else finished = true;

            //transform.LookAt(waypoints[currentWaypoint].transform);

            //Calculates the transformation to rotate the transform to face the next waypoint
            Quaternion lookatWP = Quaternion.LookRotation(waypoints[currentWaypoint].position - this.transform.position);

            //Rotates to look at the next waypoint based on the rotation speed
            transform.rotation = Quaternion.Slerp(transform.rotation, lookatWP, rotSpeed * Time.deltaTime);
            //Moves the sled forwards at the defined speed
            transform.Translate(0, 0, speed * Time.deltaTime);

            //For final level, if defined allows for horizontal movement
            if (sideMovement != 0 && sled != null)
            {
                float h = sideMovement * Input.GetAxis("Horizontal");
                //Restricts the horizontal movement
                sled.localPosition = new Vector3(Mathf.Clamp(sled.localPosition.x + (h * Time.deltaTime), -maxHorizontalDisplacement, maxHorizontalDisplacement), 0, 0);
            }
        }
        //Function to handle which ui is currently displayed and locks movement
        ManageUI();
    }

    void ManageUI()
    {
        //If the level hasnt been started, it will show the start UI and lock the movement
        if (!started && startScene != null)
        {
            startScene.SetActive(true);
            loseScene.SetActive(false);
            movementHandler.toggleLock(true);
        } // If the user finished, hides all uis but win ui
        else if (finished)
        {
            startScene.SetActive(false);
            UI.SetActive(false);
            winScene.SetActive(true);
            loseScene.SetActive(false);
            movementHandler.toggleLock(true);
        } // If lost hides all uis but losing ui
        else if (lost)
        {
            startScene.SetActive(false);
            UI.SetActive(false);
            loseScene.SetActive(true);
            movementHandler.toggleLock(true);
        } //If playing
        else
        {
            startScene.SetActive(false);
            UI.SetActive(true);
            loseScene.SetActive(false);
            movementHandler.toggleLock(false);
        } //Updates goal score text
        if (finishByScore)
        {
            scoreDisp.text = score + "/" + goal;
        }
    }

    //Method to declare the level lost
    public void ActiveLoss()
    {
        lost = true;
    }


    //Method to start the level
    public void StartLevel()
    {
        started = true;
    }

    //Method to increate the score and mark level as finished if reach defined goal
    public void IncreaseScore()
    {
        score++;
        if (score >= goal) finished = true;
    }

    //Detect if player is hit by an object with enemy collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            Debug.Log("HIT BY ENEMY");
        }
    }
}
