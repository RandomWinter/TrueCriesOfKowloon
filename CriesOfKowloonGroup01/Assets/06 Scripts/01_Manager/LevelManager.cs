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
            case "SampleScene":
            SceneManager.LoadScene(0);
            break;

            case "Level 1":
            SceneManager.LoadScene(0);
            player.GetComponent<PlayerCombat>().lightDamage += 10;
            player.GetComponent<PlayerCombat>().heavyDamage += 10;
            player.GetComponent<PlayerHealth>().maxHealth += 10;
            player.GetComponent<PlayerHealth>().currentHealth = player.GetComponent<PlayerHealth>().maxHealth;
            break;
        }
    }
}
