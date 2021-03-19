using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    //calling a singleton method
    void Awake()
    {
        SetUpSingleton();
    }

    //singleton method to play the background music with no stops or pauses when changeing scenes.
    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
