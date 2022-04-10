using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NewMainMenu : MonoBehaviour
{
    public GameObject menuUi;
    public GameObject settingsMenu;
    public GameObject controlList;
    public GameObject menuBackground;
    public GameObject backButton;
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
