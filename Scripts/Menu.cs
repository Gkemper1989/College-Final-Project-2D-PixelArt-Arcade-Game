using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    /*
     * Setting methods to the UI elements
     * Start Game button
     * Main Menu Button on the winning scene
     * Quit Game
     */

   public void StartFirstLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void StartAgainButton()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }
}
