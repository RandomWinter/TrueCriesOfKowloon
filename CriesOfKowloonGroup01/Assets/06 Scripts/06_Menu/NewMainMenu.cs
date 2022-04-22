using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NewMainMenu : MonoBehaviour
{
    
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        PlayerPrefs.SetInt("Upgraded", 0);
        GetComponent<PlayerCombat>().lightDamage = 1;
        GetComponent<PlayerCombat>().heavyDamage = 2;
        GetComponent<PlayerHealth>().maxHealth = 100;
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

    public void Intro()
    {
        
    }
}
