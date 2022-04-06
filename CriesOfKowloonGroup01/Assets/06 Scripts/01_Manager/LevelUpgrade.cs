using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUpgrade : MonoBehaviour
{
    public string stageName;
    public GameObject player;

    public void Awake()
    {
        var currentStage = SceneManager.GetActiveScene();
        stageName = currentStage.name;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch(stageName)
        {
            case "level 1-2":
            if(other.CompareTag("Player"))
            {
                if(PlayerPrefs.GetInt("Upgraded", 0) == 0)
                {
                    player.GetComponent<PlayerCombat>().newLightDamage += 5;
                    player.GetComponent<PlayerCombat>().newHeavyDamage += 5;
                    player.GetComponent<PlayerHealth>().newMaxHealth += 5;
                    PlayerPrefs.SetInt("Upgraded", 1);
                }
            }
            break;

            case "lvl 1-3":
            if(other.CompareTag("Player"))
            {
                if(PlayerPrefs.GetInt("Upgraded", 1) == 1)
                {
                    player.GetComponent<PlayerCombat>().newLightDamage += 10;
                    player.GetComponent<PlayerCombat>().newHeavyDamage += 10;
                    player.GetComponent<PlayerHealth>().newMaxHealth += 10;
                    PlayerPrefs.SetInt("Upgraded", 2);
                }
            }
            break;
        }
    }
}
