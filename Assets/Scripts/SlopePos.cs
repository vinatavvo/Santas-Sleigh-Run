using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopePos : MonoBehaviour
{
    // Start is called before the first frame update
    public Terrain terrain;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (terrain == null)
        {
            Debug.LogError("Terrain not assigned!");
            return;
        }

        // Get the position of the GameObject and convert it to terrain local coordinates
        Vector3 objectPosition = transform.position;
        Vector3 terrainLocalPosition = objectPosition - terrain.transform.position;

        // Sample the terrain height at the object's position
        float terrainHeight = terrain.SampleHeight(terrainLocalPosition);

        // Calculate the target position on the terrain
        Vector3 targetPosition = new Vector3(objectPosition.x, terrainHeight, objectPosition.z);

        // Calculate the direction from the object to the target position
        Vector3 directionToTarget = targetPosition - objectPosition;

        // Rotate the object to face the target position
        transform.rotation = Quaternion.LookRotation(directionToTarget, Vector3.forward);*/
    }
}
