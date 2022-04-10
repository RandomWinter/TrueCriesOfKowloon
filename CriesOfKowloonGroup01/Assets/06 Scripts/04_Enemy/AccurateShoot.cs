using UnityEngine;

namespace _06_Scripts._04_Enemy {
    public class AccurateShoot : MonoBehaviour{
        public Transform launchOffset;
        public RangerBullet bullet;
        public GameObject personal;
        
        private void OnEnable(){
            Instantiate(bullet, launchOffset.position, transform.rotation);
        }

        private void OnDisable(){
            personal.GetComponent<Ranger>().fired = true;
        }
    }
}
