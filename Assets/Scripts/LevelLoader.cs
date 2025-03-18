using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 0f;

    public Sound sound;
    public GameObject uiButtons;

    public void LoadNextLevel(string scene)
    {
        StartCoroutine(LoadLevel(scene));
    }

    IEnumerator LoadLevel(string level)
    {
        uiButtons.SetActive(false);

        transition.SetBool("StartTransition", true);

        yield return new WaitForSeconds(transitionTime);

        // THIS IS SO JANK, mute all audio in main scene
        foreach (AudioSource audio in FindObjectsOfType<AudioSource>())
        {
            audio.mute = true;
        }

        SceneManager.LoadScene(level, LoadSceneMode.Additive);

        transition.SetBool("StartTransition", false);
    }

    public IEnumerator UnloadAdditiveScene() {
        transition.SetBool("StartTransition", true);

        yield return new WaitForSeconds(transitionTime);

        // Put sound back in, this is horrible code
        foreach (AudioSource audio in FindObjectsOfType<AudioSource>())
        {
            audio.mute = false;
        }

        sound.SwitchSound();
        uiButtons.SetActive(true);

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.name != "RestaurantGame")
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }
        
        transition.SetBool("StartTransition", false);
    }
}
