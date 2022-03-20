using _06_Scripts._04_Enemy;
using UnityEngine;

public class EnemyHitBox : MonoBehaviour{
    private int attackDamage = 15;
    [SerializeField] private bool attackOn;
    [SerializeField] private bool hit;

    [SerializeField] public GameObject enemy;
    private EnemyBehavior _checker;

    private void OnEnable() {
        attackOn = true;
        hit = false;
    }

    private void OnDisable(){
        if (!hit) {
            enemy.GetComponent<EnemyBehavior>().missAttack = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D col){
        var check = col.GetComponent<PlayerHealth>();

        if (check == null || !attackOn) return;
        enemy.GetComponent<EnemyBehavior>().hit += 1;
        check.TakeDamage(attackDamage);
        attackOn = false;
        hit = true;
        
    }
}
