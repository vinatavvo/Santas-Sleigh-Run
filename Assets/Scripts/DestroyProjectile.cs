using UnityEngine;

public class DestroyProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3.0f);
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    // Destroy the object when it hits something
    void OnCollisionEnter(Collision _)
    {
        Destroy(gameObject);
    }
}