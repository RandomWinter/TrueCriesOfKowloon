using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NewMainMenu : MonoBehaviour
{
    //public GameObject menuUi;
    //public GameObject settingsMenu;
    //public GameObject controlList;
    //public GameObject menuBackground;
    //public GameObject backButton;
    
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        PlayerPrefs.SetInt("Upgraded", 0);
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector2(2f, 7f);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void NextCutScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
