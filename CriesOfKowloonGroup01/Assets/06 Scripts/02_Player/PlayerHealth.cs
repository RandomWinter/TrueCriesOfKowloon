using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
    public float currentHealth;
    public float maxHealth = 100;
    public int newMaxHealth;
    public HealthBarUI hb;
    public Image HPBar;

    private void Start(){
        currentHealth = maxHealth;
        //hb.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        UpdateHealthUI();
    }
    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collisionInfo){
        if(collisionInfo.CompareTag("HealthPotion")){
            Destroy(collisionInfo.gameObject, .5f);
            if(currentHealth >= 90){
                currentHealth = maxHealth;
                //hb.SetHealth(currentHealth);
                return;
            }
            
            currentHealth += 10;
            //hb.SetHealth(currentHealth);
        }

        if (collisionInfo.CompareTag("Projectile")){
            Destroy(collisionInfo.gameObject);
            TakeDamage(3);
        }
    }

    public void TakeDamage(float damage){
        currentHealth -= damage;
        //hb.SetHealth(currentHealth);

        if (currentHealth <= 0){
            FindObjectOfType<LevelManager>().Restart();
        }
    }

    public void UpdateHealthUI()
    {
        float fillF = HPBar.fillAmount;
        HPBar.fillAmount = currentHealth / maxHealth;
    }
}
