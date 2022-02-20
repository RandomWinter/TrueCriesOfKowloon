using UnityEngine;

public class EnemyHitBox : MonoBehaviour{
    [SerializeField] private int attackDamage = 2;
    [SerializeField] private bool attackOn;
    [SerializeField] private bool hit;
    
    [SerializeField] public GameObject enemy;
    [SerializeField] public int thrust = 5;
    private EnemyBehavior _checker;

    private void OnEnable() {
        attackOn = true;
        hit = false;
    }

    private void OnDisable()
    {
        if (!hit) {
            enemy.GetComponent<EnemyBehavior>().missAttack = true;
            //enemy.GetComponent<Testing2>().chainAttack = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D col){
        var check = col.GetComponent<PlayerHealth>();
        var force = col.GetComponent<Rigidbody2D>();

        if (check == null || !attackOn) return;
        check.takeDamage(attackDamage);
        attackOn = false;
        hit = true;
    }
}
