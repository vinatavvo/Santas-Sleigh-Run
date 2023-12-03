using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleSystem : MonoBehaviour
{
    private ParticleSystem pSystem;

    //For explosion particles, deletes the particles after they finish
    //Used for ornament
    private void Start()
    {
        pSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        // Check if the particle system has finished emitting particles.
        if (!pSystem.isEmitting)
        {
            // Destroy the GameObject to remove the particle system.
            Destroy(gameObject);
        }
    }
}
