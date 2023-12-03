using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManageScene : MonoBehaviour
{
    [SerializeField] SceneAsset nextScene;
    [SerializeField] float fadeTime = 1;

    IEnumerator Start()
    {
        Alpha = 1;
        yield return FadeFromBlack();
    }

    public void ResetTheGame()
    {
        //Resets the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    public void NextScene() => StartCoroutine(FadeToBlack());

    // Coroutine to fade out the scene
    IEnumerator FadeToBlack()
    {
        var startTime = Time.time;
        var endTime = startTime + fadeTime;
        while (Alpha < 1)
        {
            Alpha = (Time.time - startTime) / (endTime - startTime);
            yield return null;
        }
        SceneManager.LoadScene(nextScene.name);
    }

    //Coroutine to fade in the scene
    IEnumerator FadeFromBlack()
    {
        var startTime = Time.time;
        var endTime = startTime + fadeTime;
        while (Alpha > 0)
        {
            Alpha = 1 - (Time.time - startTime) / (endTime - startTime);
            yield return null;
        }
    }

    /// <summary>
    /// Property for transparency of black color.
    /// </summary>
    float Alpha
    {
        get => GetComponent<Image>().color.a;
        set
        {
            var image = GetComponent<Image>();
            var color = image.color;
            color.a = value;
            image.color = color;
        }
    }
}