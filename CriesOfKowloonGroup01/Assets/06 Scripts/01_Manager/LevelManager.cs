using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject player;
    Vector2 playerInitPosition;
    public string stageName;

    public GameObject playerBorder;
    public GameObject healthUI;
    public GameObject rageUI;

    public GameObject playerBorder2;
    public GameObject healthUI2;
    public GameObject rageUI2;

    public GameObject playerBorder3;
    public GameObject healthUI3;
    public GameObject rageUI3;

    public void Awake()
    {
        var currentStage = SceneManager.GetActiveScene();
        stageName = currentStage.name;

        player = GameObject.FindGameObjectWithTag("Player");
        playerInitPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    public void Restart()
    {
        switch(stageName)
        {
            case "level 1-1": case "level 1-2": case "lvl 1-3": case "lvl 2 - 1": case "lvl 2 - 2": case "lvl 2 - 3": case "lvl 3 - 1": case "lvl 3 - 2": case "lvl 3 - 3":
            SceneManager.LoadScene(12);
            player.GetComponent<PlayerCombat>().lightDamage = player.GetComponent<PlayerCombat>().lightDamage + player.GetComponent<PlayerCombat>().newLightDamage;
            player.GetComponent<PlayerCombat>().heavyDamage = player.GetComponent<PlayerCombat>().heavyDamage + player.GetComponent<PlayerCombat>().newHeavyDamage;
            player.GetComponent<PlayerHealth>().maxHealth = player.GetComponent<PlayerHealth>().maxHealth + player.GetComponent<PlayerHealth>().newMaxHealth;
            player.GetComponent<PlayerHealth>().currentHealth = player.GetComponent<PlayerHealth>().maxHealth;

            player.GetComponent<PlayerCombat>().newLightDamage = 0;
            player.GetComponent<PlayerCombat>().newHeavyDamage = 0;
            player.GetComponent<PlayerHealth>().newMaxHealth = 0;

            player.transform.position = new Vector2(2f, 7f);
            break;

            case "lvl 3 - 4 (Boss)":
            SceneManager.LoadScene(13);
            player.GetComponent<PlayerCombat>().lightDamage = player.GetComponent<PlayerCombat>().lightDamage + player.GetComponent<PlayerCombat>().newLightDamage;
            player.GetComponent<PlayerCombat>().heavyDamage = player.GetComponent<PlayerCombat>().heavyDamage + player.GetComponent<PlayerCombat>().newHeavyDamage;
            player.GetComponent<PlayerHealth>().maxHealth = player.GetComponent<PlayerHealth>().maxHealth + player.GetComponent<PlayerHealth>().newMaxHealth;
            player.GetComponent<PlayerHealth>().currentHealth = player.GetComponent<PlayerHealth>().maxHealth;

            player.GetComponent<PlayerCombat>().newLightDamage = 0;
            player.GetComponent<PlayerCombat>().newHeavyDamage = 0;
            player.GetComponent<PlayerHealth>().newMaxHealth = 0;

            player.transform.position = new Vector2(2f, 7f);
            break;
        }
    }

    public void ChangeUI()
    {
        switch (stageName)
        {
            case "lvl 1-3":
                playerBorder.SetActive(false);
                healthUI.SetActive(false);
                rageUI.SetActive(false);

                playerBorder2.SetActive(true);
                healthUI2.SetActive(true);
                rageUI2.SetActive(true);
                break;

            case "lvl 2 - 3":
            case "lvl 3 - 1":
            case "lvl 3 - 2":
            case "lvl 3 - 3":
            case "lvl 3 - 4 (Boss)":
                playerBorder.SetActive(false);
                healthUI.SetActive(false);
                rageUI.SetActive(false);

                playerBorder2.SetActive(false);
                healthUI2.SetActive(false);
                rageUI2.SetActive(false);
                
                playerBorder3.SetActive(true);
                healthUI3.SetActive(true);
                rageUI3.SetActive(true);


                break;
        }
    }
}
