using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] FollowWaypoint sled;
    [SerializeField] WeaponProperties properties;
    [SerializeField] LayerMask enemy;

    private float curMag;
    private float curStock;
    private float currentCooldown = 0;
    public Transform crosshair;

    public GameObject[] projectile;
    public Transform projectileOrigin;
    float launchVelocity;

    Recoil Recoil_Script;

    LineRenderer lr;
    private Movement movement;


    // Start is called before the first frame update
    void Start()
    {
        //Loads properties of weapon
        curMag = properties.mag;
        curStock = properties.stock;
        launchVelocity = properties.launchVelocity;

        Recoil_Script = transform.GetComponent<Recoil>();
        lr = GetComponent<LineRenderer>();
        movement = FindFirstObjectByType<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if allowed to shoot
        if (!movement.isLocked())
        {
            //If left click and cooldown is up, allow player to shoot
            if (Input.GetMouseButtonDown(0) && currentCooldown <= 0)
            {
                Shoot();
            }

            //Keep track of cooldown to prevent spam click
            if (currentCooldown > 0)
            {
                currentCooldown -= Time.deltaTime;
            }

            //If thrown weapon, generate a predictive line to show where the projectile will land
            if (properties.thrown)
            {
                List<Vector3> simulation = SimulateArc();
                lr.positionCount = simulation.Count;
                for (int a = 0; a < lr.positionCount; a++)
                {
                    lr.SetPosition(a, simulation[a]);
                }
                //lr.SetPositions(SimulateArc().ToArray());
            }
        }
    }

    void Reload()
    {
        //Not used
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
            //Starts cooldown
            currentCooldown = properties.cooldown;
        }
        //Randomizes if multiple projectiles are provided
        int ind = Mathf.RoundToInt(Random.Range(0, projectile.Length));
        GameObject ball = Instantiate(projectile[ind], projectileOrigin.position, projectileOrigin.rotation);
        //Sets initial velocity to be same as the sled
        if (sled != null)
            ball.GetComponent<Rigidbody>().velocity = sled.transform.forward * sled.speed;
        //Adds force to launch the projectile
        ball.GetComponent<Rigidbody>().AddRelativeForce(new(0, launchVelocity, 0));
        // ball.GetComponent<Rigidbody>().
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

    //Method to check if the prediction line is hitting any colliders
    private bool CheckForCollision(Vector3 pos)
    {
        Collider[] hitColliders = Physics.OverlapSphere(pos, 1f, 0);
        return hitColliders.Length > 0;
    }


    //Method to simulate the path of the projectile
    private List<Vector3> SimulateArc()
    {
        float maxDuration = 5f;
        float timeStepInterval = 0.1f;
        int maxSteps = (int)(maxDuration / timeStepInterval);
        List<Vector3> lineRendererPoints = new List<Vector3>();
        //f(t) = (x0 + x*t, y0 + y*t - 9.81t^2/2, z0 + z*t);

        Vector3 directionVector = projectileOrigin.up;
        Vector3 launchPosition = projectileOrigin.position + projectileOrigin.up;
        //Adds initial position
        lineRendererPoints.Add(launchPosition);

        float vel = launchVelocity / 1f * Time.fixedDeltaTime;

        //For each step, calculate projectiles position
        for (int i = 0; i < maxSteps; ++i)
        {
            //calculates position based on launch velocity and sled velocity
            Vector3 calculatedPosition = launchPosition + directionVector * (vel * i * timeStepInterval);
            //Accounts for gravity
            calculatedPosition.y += Physics.gravity.y / 2 * Mathf.Pow(i * timeStepInterval, 2);

            lineRendererPoints.Add(calculatedPosition);
            //If simulated point collides, stop simulating
            if (CheckForCollision(calculatedPosition))
            {
                break;
            }
        }

        return lineRendererPoints;
    }
}