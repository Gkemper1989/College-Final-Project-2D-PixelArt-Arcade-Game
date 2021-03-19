using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    //variables
    [SerializeField] float levelLoadDelay = 2f;

  void OnTriggerEnter2D (Collider2D other)
    {
        StartCoroutine(LoadNextLevel());//courotine method
    }

    //setting the courotine to wait an amount of time before load the next scene of the game
    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
