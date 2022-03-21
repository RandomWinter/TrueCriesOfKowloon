using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public string stageName;
    public GameObject player;

    public void Awake()
    {
        var currentStage = SceneManager.GetActiveScene();
        stageName = currentStage.name;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Restart()
    {
        switch(stageName)
        {
            case "Level 1-1":
            SceneManager.LoadScene(2);
            player.GetComponent<PlayerCombat>().lightDamage += 0;
            player.GetComponent<PlayerCombat>().heavyDamage += 0;
            player.GetComponent<PlayerHealth>().maxHealth += 0;
            player.GetComponent<PlayerHealth>().currentHealth = player.GetComponent<PlayerHealth>().maxHealth;
            break;

            case "Level 1-2":
            SceneManager.LoadScene(2);
            player.GetComponent<PlayerCombat>().lightDamage += 5;
            player.GetComponent<PlayerCombat>().heavyDamage += 2;
            player.GetComponent<PlayerHealth>().maxHealth += 5;
            player.GetComponent<PlayerHealth>().currentHealth = player.GetComponent<PlayerHealth>().maxHealth;
            break;

            case "Level 1-3":
            SceneManager.LoadScene(2);
            player.GetComponent<PlayerCombat>().lightDamage += 10;
            player.GetComponent<PlayerCombat>().heavyDamage += 10;
            player.GetComponent<PlayerHealth>().maxHealth += 10;
            player.GetComponent<PlayerHealth>().currentHealth = player.GetComponent<PlayerHealth>().maxHealth;
            break;
        }
    }
}
