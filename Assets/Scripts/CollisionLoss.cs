using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionLoss : MonoBehaviour
{

    public LayerMask loseIfHit;
    public FollowWaypoint controller;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Used for obstacles, will declare level lost if hit
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & loseIfHit) != 0)
        {
            controller.ActiveLoss();
        }
    }
}
