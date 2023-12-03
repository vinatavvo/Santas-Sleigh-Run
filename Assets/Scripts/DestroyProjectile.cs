using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
    //Destroys the object if it hits any object that isnt the player
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer != 7)
            Destroy(gameObject);
    }
}
