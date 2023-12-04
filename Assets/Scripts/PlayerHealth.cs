using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Handles the player having health and potentially dying.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    /// <summary>
    /// Reference to the player control script.
    /// </summary>
    [SerializeField] FollowWaypoint player;

    /// <summary>
    /// Reference to the parent GameObject of the health UI.
    /// </summary>
    [SerializeField] GameObject healthUI;

    /// <summary>
    /// Reference to the GUI element that displays the player's health.
    /// </summary>
    [SerializeField] TextMeshProUGUI healthDisp;

    /// <summary>
    /// Stores the health value (the number of hits that can be taken).
    /// </summary>
    [SerializeField] int maxHealth = 10;

    /// <summary>
    /// The current health of the player.
    /// </summary>
    int health;

    /// <summary>
    /// Has the level been started?
    /// </summary>
    bool started;

    void Start()
    {
        // Initialize health on start
        health = maxHealth;
    }

    /// <summary>
    /// Enable health UI when the level starts.
    /// </summary>
    void Update()
    {
        if (!started && player.started)
        {
            started = true;
            healthUI.SetActive(true);
        }

        if (started)
        {
            healthDisp.text = $"Durability: {health}/{maxHealth}";
        }
    }

    /// <summary>
    /// Handle being hit by a projectile.
    /// </summary>
    void OnCollisionEnter(Collision other)
    {
        // Verify other is an enemy
        if (other.gameObject.layer == 10) health--;
        // Handle death
        if (health > 0) return;
        healthUI.SetActive(false);
        player.ActiveLoss();
    }
}