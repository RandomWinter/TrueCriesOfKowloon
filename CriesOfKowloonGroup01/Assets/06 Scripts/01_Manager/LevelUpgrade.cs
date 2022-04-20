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
            case "level 1-1":
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

            case "level 1-2":
            if(other.CompareTag("Player"))
            {
                if(PlayerPrefs.GetInt("Upgraded", 1) == 1)
                {
                    player.GetComponent<PlayerCombat>().newLightDamage += 5;
                    player.GetComponent<PlayerCombat>().newHeavyDamage += 5;
                    player.GetComponent<PlayerHealth>().newMaxHealth += 5;
                    PlayerPrefs.SetInt("Upgraded", 2);
                }
            }
            break;

            case "lvl 1-3":
            if(other.CompareTag("Player"))
            {
                if(PlayerPrefs.GetInt("Upgraded", 2) == 2)
                {
                    player.GetComponent<PlayerCombat>().newLightDamage += 5;
                    player.GetComponent<PlayerCombat>().newHeavyDamage += 5;
                    player.GetComponent<PlayerHealth>().newMaxHealth += 5;
                    PlayerPrefs.SetInt("Upgraded", 3);
                }
            }
            break;

            case "lvl 2 - 1":
            if(other.CompareTag("Player"))
            {
                if(PlayerPrefs.GetInt("Upgraded", 3) == 3)
                {
                    player.GetComponent<PlayerCombat>().newLightDamage += 5;
                    player.GetComponent<PlayerCombat>().newHeavyDamage += 5;
                    player.GetComponent<PlayerHealth>().newMaxHealth += 5;
                    PlayerPrefs.SetInt("Upgraded", 4);
                }
            }
            break;

            case "lvl 2 - 2":
            if(other.CompareTag("Player"))
            {
                if(PlayerPrefs.GetInt("Upgraded", 4) == 4)
                {
                    player.GetComponent<PlayerCombat>().newLightDamage += 5;
                    player.GetComponent<PlayerCombat>().newHeavyDamage += 5;
                    player.GetComponent<PlayerHealth>().newMaxHealth += 5;
                    PlayerPrefs.SetInt("Upgraded", 5);
                }
            }
            break;

            case "lvl 2 - 3":
            if(other.CompareTag("Player"))
            {
                if(PlayerPrefs.GetInt("Upgraded", 5) == 5)
                {
                    player.GetComponent<PlayerCombat>().newLightDamage += 5;
                    player.GetComponent<PlayerCombat>().newHeavyDamage += 5;
                    player.GetComponent<PlayerHealth>().newMaxHealth += 5;
                    PlayerPrefs.SetInt("Upgraded", 6);
                }
            }
            break;

            case "lvl 3 - 1":
            if(other.CompareTag("Player"))
            {
                if(PlayerPrefs.GetInt("Upgraded", 6) == 6)
                {
                    player.GetComponent<PlayerCombat>().newLightDamage += 5;
                    player.GetComponent<PlayerCombat>().newHeavyDamage += 5;
                    player.GetComponent<PlayerHealth>().newMaxHealth += 5;
                    PlayerPrefs.SetInt("Upgraded", 7);
                }
            }
            break;

            case "lvl 3 - 2":
            if(other.CompareTag("Player"))
            {
                if(PlayerPrefs.GetInt("Upgraded", 7) == 7)
                {
                    player.GetComponent<PlayerCombat>().newLightDamage += 5;
                    player.GetComponent<PlayerCombat>().newHeavyDamage += 5;
                    player.GetComponent<PlayerHealth>().newMaxHealth += 5;
                    PlayerPrefs.SetInt("Upgraded", 8);
                }
            }
            break;

            case "lvl 3 - 3":
            if(other.CompareTag("Player"))
            {
                if(PlayerPrefs.GetInt("Upgraded", 8) == 8)
                {
                    player.GetComponent<PlayerCombat>().newLightDamage += 5;
                    player.GetComponent<PlayerCombat>().newHeavyDamage += 5;
                    player.GetComponent<PlayerHealth>().newMaxHealth += 5;
                    PlayerPrefs.SetInt("Upgraded", 9);
                }
            }
            break;
        }
    }
}
