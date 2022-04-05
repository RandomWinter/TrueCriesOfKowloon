using System;
using UnityEngine;

namespace _06_Scripts {
    public class WaveTrigger : MonoBehaviour{
        public event EventHandler OnTargetEnter;
        
        private void OnTriggerEnter2D(Collider2D col){
            if (col.CompareTag("Player")){
                OnTargetEnter?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
