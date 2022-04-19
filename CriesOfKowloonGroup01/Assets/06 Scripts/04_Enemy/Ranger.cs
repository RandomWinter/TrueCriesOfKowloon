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

        //! Boolean Event
        [HideInInspector] public bool faceRight;
        public bool isDefeated;
        #endregion
        
        #region StateMachine and Animation
        public enum StateMachine {
            Idle, Chase, Fire, Stun, Dead
        } public StateMachine rangerState;
        private static readonly int IsMoving = Animator.StringToHash("isMoving");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Dead = Animator.StringToHash("dead");
        private static readonly int Hit = Animator.StringToHash("Hit");

        #endregion

        //============================================
        private void Awake(){
            int size = path.childCount;
            destArray = new Transform[size];
            for (var i = 0; i < size; i++) {
                destArray[i] = path.transform.GetChild(i);
            }
        }
        
        private void Start(){
            _targetInfo = GameObject.FindGameObjectWithTag("Player");
            
            //! Setup Animator and Health Bar
            rHealthBar.SetMaxHealth(currentHealth);
            rAnim = GetComponent<Animator>();

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

        private void ChaseTarget(){
            if (isDefeated) return;
            if (transform.position != destArray[currentDest].position) {
                rAnim.SetBool(IsMoving, true);
                transform.position = Vector3.MoveTowards(transform.position, destArray[currentDest].position,
                    movement * Time.deltaTime);
            } else {
                rAnim.SetBool(IsMoving, false);
                if (!fired) {
                    rangerState = StateMachine.Fire;
                    return;
                }
                
                if (!once && fired){
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
            switch (fired){
                case true: rangerState = StateMachine.Chase; break;
                case false: rAnim.SetTrigger(Attack); break;
            }
        }
        #endregion
        
        //============================================

        #region ReceiveDamage, StunDown, Vanish
        public void ReceivedDamage(int dmg){
            if (isDefeated) return;
            rAnim.ResetTrigger(Attack);
            rAnim.SetBool(IsMoving, false);
            
            
            currentHealth -= dmg;
            rHealthBar.SetHealth(currentHealth);
            rangerState = currentHealth <= 0 ? StateMachine.Dead : StateMachine.Stun;
        }

        private bool stunOnce;
        private IEnumerator StunDown(){
            if (!stunOnce){
                rAnim.SetTrigger(Hit);
                stunOnce = true;
            }
            
            yield return new WaitForSeconds(1f);
            if (!isDefeated){
                stunOnce = false;
                rangerState = StateMachine.Chase;
            }
        }

        private bool deadOnce;
        private IEnumerator Vanish(){
            isDefeated = true;
            if (!deadOnce) {
                rAnim.SetTrigger(Dead);
                deadOnce = true;
            }
            
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
