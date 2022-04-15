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
        PlayerPrefs.SetInt("Upgraded", 0);
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector2(2f, 7f);
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
