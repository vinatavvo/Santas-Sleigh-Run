using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseHandler : MonoBehaviour
{
    public GameObject activeProps;
    public Material LightsOn;
    public int WindowInd = 1;
    bool activated = false;
    private FollowWaypoint levelHandler;

    // Start is called before the first frame update
    void Start()
    {
        levelHandler = FindFirstObjectByType<FollowWaypoint>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 && !activated)
        {
            activated = true;
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            List<Material> materials = new List<Material>();
            renderer.GetMaterials(materials);
            materials[WindowInd] = LightsOn;
            renderer.SetMaterials(materials);
            activeProps.SetActive(true);
            levelHandler.IncreaseScore();
        }
        if (other.gameObject.layer == 8)
        {
            other.transform.GetComponent<DestroyProjectile>().DestroyObject();
        }
    }
}
