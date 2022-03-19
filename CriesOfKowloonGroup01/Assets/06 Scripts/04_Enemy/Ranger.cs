using System;
using Unity.VisualScripting;
using UnityEngine;

namespace _06_Scripts._04_Enemy{
    public class Ranger : MonoBehaviour{
        private Transform _castPoint;
        private Transform _player;

        private void Update(){
            
        }

        private bool InRange(float distance){
            var position = _castPoint.position;
            var castDist = distance;
            var val = false;
            
            /*
            if(faceRight){
                castDist = -distance;    
            }
            */
            
            
            Vector2 endPos = position + Vector3.right * distance;
            RaycastHit2D hit = Physics2D.Linecast(position, endPos, 1 << LayerMask.NameToLayer("Eyes"));
            if (hit.collider != null){
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    val = true;
                } else {
                    val = false;
                }
            }

            return val;
        }
    }
}
