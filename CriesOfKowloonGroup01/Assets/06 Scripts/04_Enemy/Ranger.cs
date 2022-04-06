using System;
using System.Collections;
using UnityEngine;

namespace _06_Scripts._04_Enemy {
    public class Ranger : MonoBehaviour {
        #region Components Field
        private GameObject _targetInfo;
        
        [Header("Health & Animator")]
        public HealthBarUI rHealthBar;
        public Animator rAnim;
        public Collider2D c2D;
        #endregion
        
        #region Variables
        //! It's Health and Movement
        [SerializeField] private float searchRadius = 10f;
        [SerializeField] private float movement = 3.25f;
        [HideInInspector] public int currentHealth = 20;
        public float preFire = 1.25f;

        [SerializeField] private Transform path;
        [SerializeField] private Transform[] destArray;
        public int currentDest;
        public bool once;
        public bool fired;
        
        //! Spawn GameObject position
        public Transform launchOffset;
        public RangerBullet bullet;
        
        //! Boolean Event
        [HideInInspector] public bool faceRight;
        public bool isDefeated;
        #endregion
        
        #region StateMachine and Animation
        public enum StateMachine {
            Idle, Chase, Fire, Stun, Dead
        } public StateMachine rangerState;
        #endregion

        //============================================
        private void Start(){
            _targetInfo = GameObject.FindGameObjectWithTag("Player");
            
            //! Setup Animator and Health Bar
            //rHealthBar.SetMaxHealth(currentHealth);
            //rAnim == GetComponent<Animator>();

            var size = path.childCount;
            destArray = new Transform[size];
            for (var i = 0; i < size; i++) {
                destArray[i] = path.transform.GetChild(i);
            }
            
            rangerState = StateMachine.Idle;
        }

        private void Update(){
            switch (rangerState) {
                case StateMachine.Idle: Idle(); break;
                case StateMachine.Chase: ChangeDirection(); ChaseTarget(); break;
                case StateMachine.Fire: StartCoroutine(FireWeapon()); break;
                case StateMachine.Stun: StartCoroutine(StunDown()); break;
                case StateMachine.Dead: StartCoroutine(Vanish()); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        //============================================
        
        #region Idle, Chase Target
        private void Idle() {
            Vector2 targetDistance = transform.position - _targetInfo.transform.position;
            if (targetDistance.sqrMagnitude < searchRadius * searchRadius) {
                rangerState = StateMachine.Chase;
            }
        }

        private void ChaseTarget() {
            if (transform.position != destArray[currentDest].position) {
                transform.position = Vector3.MoveTowards(transform.position, destArray[currentDest].position,
                    movement * Time.deltaTime);
            } else {
                if (!fired) {
                    rangerState = StateMachine.Fire;
                    return;
                }

                if (!once){
                    once = true;
                    StartCoroutine(MoveNextDest());
                }
            }
        }

        private IEnumerator MoveNextDest() {
            yield return new WaitForSeconds(1f);
            if (currentDest + 1 < destArray.Length){
                currentDest += 1;
            } else {
                currentDest = 0;
            }

            once = false;
            fired = false;
        }
        #endregion
        
        #region Fire
        private IEnumerator FireWeapon() {
            yield return new WaitForSeconds(preFire);
            fired = true;
            Instantiate(bullet, launchOffset.position, transform.rotation);
            rangerState = StateMachine.Chase;
        }
        #endregion
        
        //============================================

        #region ReceiveDamage, StunDown, Vanish
        private void ReceivedDamage(int dmg){
            if (isDefeated) return;
            //!anim.ResetTrigger(Attack);
            //!anim.SetBool(IsMoving, false);
            
            currentHealth -= dmg;
            //!minionHb.SetHealth(_currentHealth);
            rangerState = currentHealth <= 0 ? StateMachine.Dead : StateMachine.Stun;
        }
        
        private IEnumerator StunDown(){
            //! anim.SetTrigger(Hit);
            yield return new WaitForSeconds(1f);
            if (!isDefeated){
                rangerState = StateMachine.Chase;
            }
        }

        private IEnumerator Vanish(){
            isDefeated = true;
            //! anim.SetTrigger(Dead);
            yield return new WaitForSeconds(5f);
            gameObject.SetActive(false);
        }
        #endregion
        
        //============================================

        #region Flip the Character
        private void Flip(){
            faceRight = !faceRight;
            transform.Rotate(0, 180f, 0);
        }
        
        private void ChangeDirection() {
            var localOffset = transform.position.x - _targetInfo.transform.position.x;
            switch (localOffset){
                case < 0 when !faceRight: Flip(); break;
                case > 0 when faceRight: Flip(); break;
            }
        }
        #endregion
        
    }
}
