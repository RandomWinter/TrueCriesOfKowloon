using UnityEngine;

namespace _06_Scripts._04_Enemy{
    public class RangerBullet : MonoBehaviour {
        [Header("Basic Bullet")]
        public float speed = 6f;

        private void FixedUpdate(){
            var transform1 = transform;
            transform1.position += -transform1.right * (Time.deltaTime * speed);
        }

        private void OnBecameInvisible(){
            Destroy(gameObject);
        }
    }
}
