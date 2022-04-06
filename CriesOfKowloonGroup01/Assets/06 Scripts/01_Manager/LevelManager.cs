using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject player;
    Vector2 playerInitPosition;

    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInitPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    public void Restart()
    {
        SceneManager.LoadScene(2);
        GameObject.FindGameObjectWithTag("Player").transform.position = playerInitPosition;
        player.GetComponent<PlayerCombat>().lightDamage = player.GetComponent<PlayerCombat>().lightDamage + player.GetComponent<PlayerCombat>().newLightDamage;
        player.GetComponent<PlayerCombat>().heavyDamage = player.GetComponent<PlayerCombat>().heavyDamage + player.GetComponent<PlayerCombat>().newHeavyDamage;
        player.GetComponent<PlayerHealth>().maxHealth = player.GetComponent<PlayerHealth>().maxHealth + player.GetComponent<PlayerHealth>().newMaxHealth;
        player.GetComponent<PlayerHealth>().currentHealth = player.GetComponent<PlayerHealth>().maxHealth;

        player.GetComponent<PlayerCombat>().newLightDamage = 0;
        player.GetComponent<PlayerCombat>().newHeavyDamage = 0;
        player.GetComponent<PlayerHealth>().newMaxHealth = 0;
    }
}
