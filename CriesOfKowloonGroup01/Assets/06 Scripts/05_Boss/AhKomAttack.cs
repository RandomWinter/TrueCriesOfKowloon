using System;
using UnityEngine;

namespace _06_Scripts._05_Boss {
    public class AhKomBehaviour : MonoBehaviour{
        [Header("Attack Stage")] 
        [SerializeField] private bool isAttack;
        [SerializeField] private bool isTargetHit;

        [Header("Connector")] 
        public GameObject ahKom;

        private void OnEnable(){
            isAttack = true;
            isTargetHit = false;
        }

        private void OnDisable() {
            if (!isTargetHit)
                ahKom.GetComponent<AhKom>().missAttack = true;
        }

        private void OnTriggerEnter2D(Collider2D c)  {
            var check = c.GetComponent<PlayerHealth>();

            if (check == null || !isAttack) return;
            switch (ahKom.GetComponent<AhKom>().comboHit){
                case 0:
                    check.TakeDamage(10);
                    break;
                case 1:
                    check.TakeDamage(15);
                    break;
            } ahKom.GetComponent<AhKom>().comboHit += 1;
            isAttack = false; isTargetHit = true;
        }
    }
}
