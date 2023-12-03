using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shatter : MonoBehaviour
{
    public GameObject shatterParticles;
    public bool collided;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //For ornament, if hits a collider, create shatter particles to look like ornament breaks
    private void OnCollisionEnter(Collision collision)
    {
        if (!collided)
        {
            Debug.Log(collision.gameObject.CompareTag("Player"));
            collided = true;
            Instantiate(shatterParticles, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
