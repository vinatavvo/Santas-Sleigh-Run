using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] WeaponProperties properties;
    [SerializeField] LayerMask enemy;

    private float curMag;
    private float curStock;
    private float currentCooldown = 0;
    public Transform crosshair;

    public GameObject[] projectile;
    public Transform projectileOrigin;
    public float launchVelocity = 700f;

    Recoil Recoil_Script;
    

    // Start is called before the first frame update
    void Start()
    {
        curMag = properties.mag;
        curStock = properties.stock;

        Recoil_Script = transform.GetComponent<Recoil>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentCooldown <= 0)
        {
            Shoot();
        }


        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    void Reload()
    {
        if (curStock == 0)
        {
            return;
        }
        if (curStock >= properties.mag)
        {
            curMag = properties.mag;
            curStock -= properties.mag;
        }
        else
        {
            curMag = curStock;
            curStock = 0;
        }
    }

    void Shoot()
    {
        if (curMag == 0)
        {
            Reload();
        }
        else
        {
            curMag--;
            currentCooldown = properties.cooldown;
        }
        int ind = Mathf.RoundToInt(Random.Range(0, projectile.Length));
        GameObject ball = Instantiate(projectile[ind], projectileOrigin.position, projectileOrigin.rotation);
        ball.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, launchVelocity, 0));
        /*RaycastHit t_hit = new RaycastHit();
        Vector3 t_bloom = crosshair.position + crosshair.forward * 1000f;
        t_bloom += UnityEngine.Random.Range(-properties.bloom, properties.bloom) * crosshair.up;
        t_bloom += UnityEngine.Random.Range(-properties.bloom, properties.bloom) * crosshair.right;
        t_bloom -= crosshair.position;
        t_bloom.Normalize();
        Recoil_Script.RecoilFire();

        if (Physics.Raycast(crosshair.position, t_bloom, out t_hit, 1000f, enemy))
        {
            Transform hit = t_hit.transform;
            //Debug.Log(hit);
        }*/
    }
}
