using UnityEngine;

namespace _06_Scripts._05_Boss {
    public class YuLingAttack : MonoBehaviour{
        public int attack = 20;
        private bool _isAttackOn;
        private bool _targetHit;
        public GameObject boss2;

        private void OnEnable(){
            _isAttackOn = true;
            _targetHit = false;
        }

        private void OnDisable(){
            if (!_targetHit)
                boss2.GetComponent<YuLing>().missAtt = true;
        }

        private void OnTriggerEnter2D(Collider2D col){
            var det = col.GetComponent<PlayerHealth>();

            if (det == null || !_isAttackOn) return;
            _isAttackOn = false;
            _targetHit = true;

            if (boss2.GetComponent<YuLing>().normalAttack) {
                boss2.GetComponent<YuLing>().chainHit += 1;
                det.TakeDamage(attack);
                return;
            }

            if (boss2.GetComponent<YuLing>()._sDActivated){
                det.TakeDamage(15);
                return;
            }

            if (boss2.GetComponent<YuLing>().eDActivated){
                det.TakeDamage(30);
            }
        }
    }
}
