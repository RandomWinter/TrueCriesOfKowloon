using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menuUi;
    public GameObject settingsMenu;
    public GameObject controlList;
    public GameObject menuBackground;
    public GameObject backButton;

    public bool menuOpen;

    void Start()
    {
        menuOpen = false;
    }

    void Update()
    {
        //plays game 
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerPrefs.SetInt("Upgraded", 0);
            SceneManager.LoadScene(1);
        }
        //opens settings menu
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            menuUi.SetActive(false);
            settingsMenu.SetActive(true);
            controlList.SetActive(false);
            menuOpen = true;
        }
        //opens controls list
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            menuUi.SetActive(false);
            settingsMenu.SetActive(false);
            menuBackground.SetActive(false);
            controlList.SetActive(true);
            backButton.SetActive(true);
            menuOpen = true;
        }
        //closes the game
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Application.Quit();
        }

        if (menuOpen)
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                Debug.Log("Menu is closed");
                menuUi.SetActive(true);
                menuBackground.SetActive(true);
                settingsMenu.SetActive(false);
                controlList.SetActive(false);
                backButton.SetActive(false);
                menuOpen = false;
                
            }
        }
    }

    

    
}
