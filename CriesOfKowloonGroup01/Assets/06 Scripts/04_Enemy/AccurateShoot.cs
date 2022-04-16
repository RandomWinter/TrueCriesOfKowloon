using UnityEngine;

namespace _06_Scripts._04_Enemy {
    public class AccurateShoot : MonoBehaviour{
        public GameObject target;
        public Transform launchOffset;
        public RangerBullet bullet;
        public GameObject personal;

        private void Awake() {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        
        private void OnEnable(){
            Instantiate(bullet, launchOffset.position, target.transform.rotation);
        }

        private void OnDisable(){
            personal.GetComponent<Ranger>().fired = true;
        }
    }
}
