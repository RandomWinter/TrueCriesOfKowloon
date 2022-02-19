using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;
    public GameObject pauseMenuUi;

     void Start()
    {
        Debug.Log("Game is not paused");
        GamePaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            if (GamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (GamePaused)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(0);
            }
        }
        
    }


    void Resume()
    {
            Debug.Log(" Game Is Not Paused");
            pauseMenuUi.SetActive(false);
            Time.timeScale = 1f;
            GamePaused = false;
    }

    void Pause()
    {
            Debug.Log("Game Is Paused");
            pauseMenuUi.SetActive(true);
            Time.timeScale = 0f;
            GamePaused = true;
    }

    
}
