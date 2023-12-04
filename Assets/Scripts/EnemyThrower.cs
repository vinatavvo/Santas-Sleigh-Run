using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyThrower : MonoBehaviour
{
    //Script to handle an enemy of thrower class
    [SerializeField] Transform player;
    [SerializeField] float cooldown = 5f;
    [SerializeField] float turnSpeed = 30f;

    [Header("Thrower Info")]
    public Animator anim;
    public Transform throwSpawn;
    public GameObject thrown;
    public float launchVelocity = 10f;

    private float lastThrowTime;
    private GameObject toThrow;
    bool dead = false;
    FollowWaypoint handler;

    // Start is called before the first frame update
    void Start()
    {
        handler = player.GetComponent<FollowWaypoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if (handler.activeGame())
        //If not dead, enemy should look at the player and throw projectiles as long as the player
        //is within 450 units
        if (Vector3.Distance(transform.position, player.position) < 50 && !dead)
        {
            //If animator finishes cycle, throw another snowball and trigger animation
            if(!AnimatorIsPlaying("Throw Snowball") && Time.time - lastThrowTime >= cooldown)
            {
                anim.SetTrigger("ThrowTrigger");
                lastThrowTime = Time.time;
                toThrow = Instantiate(thrown, throwSpawn);
                Invoke("throwObject", 1.45f);
            }
        }
        if (!dead)
        {
            Quaternion lookatWP = Quaternion.LookRotation(player.position - transform.position);

            // Ensure rotation only around y-axis
            lookatWP.eulerAngles = new Vector3(0, lookatWP.eulerAngles.y, 0);

            // Clamp x and z rotation to +- 45 degrees
            float clampedX = Mathf.Clamp(lookatWP.eulerAngles.x, -45f, 45f);
            float clampedZ = Mathf.Clamp(lookatWP.eulerAngles.z, -45f, 45f);

            lookatWP.eulerAngles = new Vector3(clampedX, lookatWP.eulerAngles.y, clampedZ);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookatWP, turnSpeed * Time.deltaTime);

        }
        else
        {
            gameObject.layer = 0;
            //If enemy dies destroy its object after some time
            StartCoroutine(DestroyEnemy());
        }
    }

    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    //Check if the animator is playing an animation
    bool AnimatorIsPlaying()
    {
        return anim.GetCurrentAnimatorStateInfo(0).length >
               anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    //Checks if the current animation is the given name
    bool AnimatorIsPlaying(string stateName)
    {
        return AnimatorIsPlaying() && anim.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    //Throws object at player
    void throwObject()
    {
        toThrow.transform.parent = null;
        Rigidbody rb = toThrow.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        toThrow.transform.LookAt(player);
        Vector3 direction = player.position - toThrow.transform.position;
        //Makes it throw slightly higher
        direction.y += 5f;
        rb.AddForce(direction.normalized * launchVelocity, ForceMode.Impulse);
    }


    //If hit by a projectile, from player, declare enemy dead
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            gameObject.layer = 0;
            anim.SetTrigger("DeathTrigger");
            dead = true;
        }
    }
}
