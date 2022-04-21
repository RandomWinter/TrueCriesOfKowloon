using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelUpgrade : MonoBehaviour
{
    public string stageName;
    public GameObject player;
    public GameObject playerBorder;
    public GameObject playerBorder2;
    public GameObject playerBorder3;
    public GameObject healthUI;
    public GameObject healthUI2;
    public GameObject healthUI3;
    public GameObject rageUI;
    public GameObject rageUI2;
    public GameObject rageUI3;

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

                    playerBorder.SetActive(false);
                    healthUI.SetActive(false);
                    rageUI.SetActive(false);

                    playerBorder2.SetActive(true);
                    healthUI2.SetActive(true);
                    rageUI2.SetActive(true);
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

                    playerBorder.SetActive(false);
                    healthUI.SetActive(false);
                    rageUI.SetActive(false);

                    playerBorder2.SetActive(false);
                    healthUI2.SetActive(false);
                    rageUI2.SetActive(false);

                    playerBorder3.SetActive(true);
                    healthUI3.SetActive(true);
                    rageUI3.SetActive(true);
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
