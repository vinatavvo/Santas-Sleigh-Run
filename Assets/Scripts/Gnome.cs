using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Controls the gnome enemy in level 2 (Santa's Workshop).
/// </summary>
public class Gnome : MonoBehaviour
{
    /// <summary>
    /// The player (sled).
    /// </summary>
    [SerializeField] FollowWaypoint player;

    /// <summary>
    /// The lid of the present this gnome is hiding in.
    /// </summary>
    [SerializeField] Transform lid;

    /// <summary>
    /// The radius around the gnome the player needs to enter to make the gnome start attacking.
    /// </summary>
    [SerializeField] float triggerRadius = 4;

    /// <summary>
    /// How far up the gnome should pop out of the present when the player gets close.
    /// </summary>
    [SerializeField] float popUpDistance = 0.5f;

    /// <summary>
    /// How many seconds the gnome should take for its pop-up animation.
    /// </summary>
    [SerializeField] float popUpTime = 0.3f;

    /// <summary>
    /// A curve describing the vertical movement of the gnome when going up or down.
    /// </summary>
    [FormerlySerializedAs("curve")] [SerializeField]
    AnimationCurve popUpCurve;

    /// <summary>
    /// The projectile that will be instantiated when attacking.
    /// </summary>
    [SerializeField] GameObject projectile;

    /// <summary>
    /// How many seconds to wait between each attack.
    /// </summary>
    [SerializeField] float attackCooldown = 1;

    /// <summary>
    /// The initial speed of the projectile.
    /// </summary>
    [SerializeField] float projectileSpeed = 1;

    /// <summary>
    /// Extra factor in making projectile arc upwards.
    /// </summary>
    [SerializeField] float projectileBoost = 3;

    /// <summary>
    /// The starting position for the projectile.
    /// </summary>
    [SerializeField] Transform projectileOrigin;

    /// <summary>
    /// How many hits the gnome can take before it dies.
    /// </summary>
    [SerializeField] int health = 5;

    /// <summary>
    /// Reference to the audio source.
    /// </summary>
    [SerializeField] AudioSource audioSource;

    /// <summary>
    /// Sound to play when the gnome dies.
    /// </summary>
    [SerializeField] AudioClip deathSound;

    /// <summary>
    /// Material to show when being damaged.
    /// </summary>
    [SerializeField] Material damagedMaterial;

    /// <summary>
    /// How many seconds to show the damageMaterial when taking damage.
    /// </summary>
    [SerializeField] float damageIndicatorDuration = 0.2f;

    /// <summary>
    /// The starting Y position of the gnome and the present lid.
    /// </summary>
    float startY, lidStartY;

    /// <summary>
    /// Is this gnome in the "up"/aggro state?
    /// </summary>
    bool up;

    /// <summary>
    /// Is an up or down animation currently playing?
    /// </summary>
    bool animating;

    /// <summary>
    /// When was the last attack (used for attack cooldown)?
    /// </summary>
    float lastAttackTime;

    void Start()
    {
        // Record initial values
        startY = transform.position.y;
        if (lid != null)
            lidStartY = lid.transform.position.y;
    }

    void Update()
    {
        // Look at the player at all times
        var diff = player.transform.position - transform.position;
        diff.y = 0;
        transform.rotation = Quaternion.LookRotation(diff) * Quaternion.Euler(-90, 0, 90);

        // Adjust present lid height
        if (lid != null)
            lid.transform.position = new(
                lid.transform.position.x,
                lidStartY + (transform.position.y - startY),
                lid.transform.position.z);

        // Do nothing if dead
        if (health <= 0) return;

        // Play animations when player is entering/leaving triggerRadius
        if (!up && (player.transform.position - transform.position).magnitude < triggerRadius)
        {
            up = true;
            lastAttackTime = Time.time; // Don't attack immediately
            StartCoroutine(Up());
        }
        else if (up && (player.transform.position - transform.position).magnitude > triggerRadius)
        {
            up = false;
            StartCoroutine(Down());
        }

        // Attack the player if the cooldown is off
        if (up && Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            Attack();
        }
    }

    /// <summary>
    /// Attack the player, throwing a projectile at them.
    /// </summary>
    void Attack()
    {
        var proj = Instantiate(projectile, projectileOrigin.position, projectileOrigin.rotation);
        proj.transform.LookAt(player.transform);
        var direction = player.transform.position - proj.transform.position;
        direction.y += projectileBoost;
        proj.GetComponent<Rigidbody>().AddForce(direction.normalized * projectileSpeed, ForceMode.VelocityChange);
    }

    /// <summary>
    /// Make the gnome pop out of the present and start shooting the player.
    /// </summary>
    IEnumerator Up()
    {
        if (animating) yield break;
        animating = true;
        audioSource.Play();
        var startTime = Time.time;
        var endTime = startTime + popUpTime;
        while (Time.time < endTime)
        {
            var t = (Time.time - startTime) / (endTime - startTime);
            var y = popUpCurve.Evaluate(t) * popUpDistance;
            transform.position = new(transform.position.x, startY + y, transform.position.z);
            yield return null;
        }

        animating = false;
    }

    /// <summary>
    /// Returns the gnome into its docile state, hiding in the present.
    /// </summary>
    IEnumerator Down()
    {
        if (animating) yield break;
        animating = true;
        var startTime = Time.time;
        var endTime = startTime + popUpTime;
        while (Time.time < endTime)
        {
            var t = 1 - (Time.time - startTime) / (endTime - startTime);
            var y = popUpCurve.Evaluate(t) * popUpDistance;
            transform.position = new(transform.position.x, startY + y, transform.position.z);
            yield return null;
        }

        animating = false;
    }

    /// <summary>
    /// Detect collisions from player projectiles.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        // Was this gnome hit by a projectile?
        if (!up || other.gameObject.layer != 8) return;
        health--;
        StartCoroutine(TakeDamage());
        Destroy(other.gameObject);

        // Handle death by playing Down animation
        if (health <= 0)
        {
            audioSource.clip = deathSound;
            audioSource.Play();
            player.IncreaseScore();
            StartCoroutine(Down());
        }
    }

    /// <summary>
    /// Shows the damage material for a brief moment when taking damage.
    /// </summary>
    IEnumerator TakeDamage()
    {
        var meshRenderers = GetComponentsInChildren<MeshRenderer>();
        var originalMaterials = meshRenderers.Select(mr => mr.material).ToArray();
        foreach (var mr in meshRenderers) mr.material = damagedMaterial;
        yield return new WaitForSeconds(damageIndicatorDuration);
        for (var i = 0; i < meshRenderers.Length; ++i) meshRenderers[i].material = originalMaterials[i];
    }

    /// <summary>
    /// Gizmo to visualize the trigger radius.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}