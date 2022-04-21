using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneTransition : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartGame()
    {
        if (SceneManager.GetActiveScene().buildIndex == 12)
        {
            SceneManager.LoadScene(2);
        }
        
    }
}
