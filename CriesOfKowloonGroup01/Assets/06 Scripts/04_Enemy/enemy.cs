using UnityEngine;

namespace _06_Scripts._04_Enemy {
    public class enemy : MonoBehaviour {
        public int maxHealth = 10;
        public int currentHealth;
        
        public void Start() {
            currentHealth = maxHealth;
        }

        public void takeDmg(int damage){
            currentHealth -= damage;

            if(currentHealth <= 0) {
                Kill();
            }
        }

        void Kill() {
            Debug.Log("enemy dead");
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            this.enabled = false;
        }
    }
}
