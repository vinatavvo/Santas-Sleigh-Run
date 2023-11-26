using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyThrower : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < 450)
        {
            if(!AnimatorIsPlaying("Throw Snowball") && Time.time - lastThrowTime >= cooldown)
            {
                anim.SetTrigger("ThrowTrigger");
                lastThrowTime = Time.time;
                toThrow = Instantiate(thrown, throwSpawn);
                Invoke("throwObject", 1.45f);
            }
        }

        Quaternion lookatWP = Quaternion.LookRotation(player.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookatWP, turnSpeed * Time.deltaTime);

    }

    bool AnimatorIsPlaying()
    {
        return anim.GetCurrentAnimatorStateInfo(0).length >
               anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    bool AnimatorIsPlaying(string stateName)
    {
        return AnimatorIsPlaying() && anim.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    void throwObject()
    {
        toThrow.transform.parent = null;
        Rigidbody rb = toThrow.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        toThrow.transform.LookAt(player);
        Vector3 direction = player.position - toThrow.transform.position;
        rb.AddForce(direction.normalized * launchVelocity, ForceMode.Impulse);
    }
}
