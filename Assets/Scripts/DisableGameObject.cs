using UnityEngine;

/// <summary>
/// Disables a game object on start.
/// </summary>
public class DisableGameObject : MonoBehaviour
{
    /// <summary>
    /// The game object that will be disabled.
    /// </summary>
    [SerializeField] GameObject gameObjectToDisable;

    void Start()
    {
        // Disable the game object.
        gameObjectToDisable.SetActive(false);
    }
}