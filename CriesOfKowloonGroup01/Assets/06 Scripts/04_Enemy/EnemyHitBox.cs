using UnityEngine;

namespace _06_Scripts._04_Enemy {
    public class EnemyHitBox : MonoBehaviour{
        private int attackDamage = 15;
        private bool _isAttackOn;
        private bool _targetHit;
        public GameObject enemy;
        
        private EnemyBehavior _checker;

        private void OnEnable() {
            _isAttackOn = true;
            _targetHit = false;
        }

        private void OnDisable() {
            if (_targetHit) return;
            enemy.GetComponent<MeleeCombat>().didIMissAttack = true;
        }

        private void OnTriggerEnter2D(Collider2D col){
            var check = col.GetComponent<PlayerHealth>();

            if (check == null || !_isAttackOn) return;
            enemy.GetComponent<MeleeCombat>().targetHit += 1;
            check.TakeDamage(attackDamage);
            _isAttackOn = false;
            _targetHit = true;
        }
    }
}
