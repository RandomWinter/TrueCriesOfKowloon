using System;
using UnityEngine;

namespace _06_Scripts._04_Enemy{
    public class RangerBullet : MonoBehaviour{
        [Header("Basic Bullet")] 
        public Rigidbody2D rb2d;
        private GameObject _target;
        private Vector2 _direction;
        public float speed = 6f;


        private void Start(){
            rb2d = GetComponent<Rigidbody2D>();
            _target = GameObject.FindGameObjectWithTag("Player");
            _direction = (_target.transform.position - transform.position).normalized * speed;
            rb2d.velocity = new Vector2(_direction.x, _direction.y);
        }

        private void OnTriggerEnter2D(Collider2D col){
            throw new NotImplementedException();
        }

        private void OnBecameInvisible(){
            Destroy(gameObject);
        }
    }
}
