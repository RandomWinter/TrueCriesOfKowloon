using System;
using UnityEngine;

namespace _06_Scripts {
    public class WaveTrigger : MonoBehaviour{
        private void OnTriggerEnter2D(Collider2D col){
             var target = col.GetComponent<PlayerHealth>();
             if (target != null){
                
             }
        }
    }
}
