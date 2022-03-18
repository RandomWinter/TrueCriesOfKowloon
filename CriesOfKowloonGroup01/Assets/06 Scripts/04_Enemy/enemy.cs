using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public int maxHealth = 10;
    int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void takeDmg(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Kill();
        }
    }

    void Kill()
    {
        Debug.Log("enemy dead");
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        this.enabled = false;
    }
}
