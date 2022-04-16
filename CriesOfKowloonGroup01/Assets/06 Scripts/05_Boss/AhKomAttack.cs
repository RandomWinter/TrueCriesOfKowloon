using System;
using UnityEngine;

namespace _06_Scripts._05_Boss {
    public class AhKomAttack : MonoBehaviour{
        [Header("Attack Stage")] 
        [SerializeField] private bool isAttack;
        [SerializeField] private bool isTargetHit;

        [Header("Connector")] 
        public GameObject ahKom;

        private void OnEnable(){
            isAttack = true;
            isTargetHit = false;
        }

        private void OnDisable(){
            if (!isTargetHit)
                ahKom.GetComponent<AhKom>().missAttack = true;
        }


        private void OnTriggerEnter2D(Collider2D c)  {
            var check = c.GetComponent<PlayerHealth>();

            if (check == null || !isAttack) return;
            isAttack = false; 
            isTargetHit = true;
            
            if (ahKom.GetComponent<AhKom>().normalAttack){
                ahKom.GetComponent<AhKom>().comboHit += 1;
                check.TakeDamage(20);
                return;
            }
            
            if (ahKom.GetComponent<AhKom>().wMActivated){
                check.TakeDamage(15);
                return;
            }
            
            if (ahKom.GetComponent<AhKom>().bRActivated){
                check.TakeDamage(35);
            }
        }
    }
}
