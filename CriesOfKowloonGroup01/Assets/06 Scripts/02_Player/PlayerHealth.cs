using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {
    public int currentHealth;
    public int maxHealth = 100;
    public int newMaxHealth;
    public HealthBarUI hb;

    private void Start(){
        currentHealth = maxHealth;
        hb.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collisionInfo){
        if(collisionInfo.CompareTag("HealthPotion")){
            Destroy(collisionInfo.gameObject, .5f);
            if(currentHealth >= 90){
                currentHealth = maxHealth;
                hb.SetHealth(currentHealth);
                return;
            }
            
            currentHealth += 10;
            hb.SetHealth(currentHealth);
        }

        if (collisionInfo.CompareTag("Projectile")){
            Destroy(collisionInfo.gameObject);
            TakeDamage(3);
        }
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;
        hb.SetHealth(currentHealth);

        if (currentHealth <= 0){
            //SceneManager.LoadScene(0);
            FindObjectOfType<LevelManager>().Restart();
        }
    }
}
