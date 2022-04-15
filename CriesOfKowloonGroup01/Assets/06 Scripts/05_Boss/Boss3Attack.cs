using System;
using UnityEngine;

namespace _06_Scripts._05_Boss {
    public class Boss3Attack : MonoBehaviour{
        public int attack = 20;
        private bool _isAttackOn;
        private bool _targetHit;
        private GameObject boss3;

        private void OnEnable(){
            _isAttackOn = true;
            _targetHit = false;
        }

        private void OnDisable(){
            if (!_targetHit)
                boss3.GetComponent<Boss3>().missAttack = true;
        }

        private void OnTriggerEnter2D(Collider2D col){
            var det = col.GetComponent<PlayerHealth>();

            if (det == null || !_isAttackOn) return;
            _isAttackOn = false;
            _targetHit = true;

            if(boss3.GetComponent<Boss3>().normalAtt){
                boss3.GetComponent<Boss3>().targetHit += 1;
                det.TakeDamage(attack);
                return;
            }
            
            if(boss3.GetComponent<Boss3>().xKickActive) {
                det.TakeDamage(45);
                return;
            }
            
            if(boss3.GetComponent<Boss3>().inchActivate) {
                det.TakeDamage(50);
            }
        }
    }
}
