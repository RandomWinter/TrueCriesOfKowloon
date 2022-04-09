using System;
using System.Collections;
using UnityEngine;

namespace _06_Scripts._04_Enemy{
    //! This code is an improvement of previous script, simplified, and flexible for changes
    //! 2. Navmesh will Re-Enable back, avoiding the Invisible Wall
    public class MeleeCombat : MonoBehaviour{
        #region Variable Setup
        //! Search player's transform and script
        private GameObject _target;
        private Transform _pFront;
        private Transform _pBehind;
        private PlayerMovement _targetMv;
        
        //! Connect with Health Bar and Animator
        public HealthBarUI minionHb;
        public Animator anim;
        
        //! It's Health and Movement (28.03.2022 - Apply Navmesh Agent back)
        [SerializeField] private float searchRange = 10f;
        [SerializeField] private float movementS = 2.85f; //! Update: 3 ~ 3.25f
        [HideInInspector] public int currentHealth = 20;
        
        //! Raycast for Attack (28.03.2022 - Exchange to Radius or Collider Box)
        [SerializeField] private Transform castPoint;
        [HideInInspector] public float castDistance;
        [HideInInspector] public bool isDetected;
        
        //! Boolean Event, used to inform Manager about current situation
        public bool targetFound; //! Description: Check Target in its radius
        public bool readyToAttack; //! Description: when other is attacking, it will wait
        public bool isDead; //! Description: when it died, manager will check and asked other to take over its job

        [HideInInspector] public bool didIMissAttack; //! Description: Received from Animation feedback
        [HideInInspector] public int targetHit;
        [HideInInspector] public bool isFacingRight; //! Visual Effect: Facing Player according to their position offset
        #endregion

        #region StateMachine, Awake, Update
        public enum StateMachine{
            Idle, Follow, Attack, Dead, Stun
        } public StateMachine minionStates;
        private static readonly int IsMoving = Animator.StringToHash("isMoving");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int Dead = Animator.StringToHash("dead");

        private void Start(){
            _target = GameObject.FindGameObjectWithTag("Player");
            _pFront = _target.transform.Find("RightTrigger");
            _pBehind = _target.transform.Find("LeftTrigger");
            _targetMv = _target.GetComponent<PlayerMovement>();
            
            //! Setup Animator and Health Bar
            minionHb.SetMaxHealth(currentHealth);
            anim = GetComponent<Animator>();

            minionStates = StateMachine.Idle;
        }

        private void Update(){
            switch (minionStates){
                case StateMachine.Idle: Idle(); break;
                case StateMachine.Follow: ChangeDirection(); Follow(); break;
                case StateMachine.Attack: AttackPlayer(); break;
                case StateMachine.Dead: StartCoroutine(Vanish()); break;
                case StateMachine.Stun: StartCoroutine(CantMove()); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region Idle, InRange, Follow
        private void Idle(){
            Vector2 radar = transform.position - _target.transform.position;
            if (!(radar.sqrMagnitude < searchRange * searchRange)) return;
            minionStates = StateMachine.Follow;
            targetFound = true;
        }
        
        private bool InRange(float dist) {
            var sPosition = castPoint.position;
            isDetected = false;

            castDistance = isFacingRight switch {
                true => dist, false => -dist
            };

            Vector2 endPos = sPosition + Vector3.right * castDistance;
            var check = Physics2D.Linecast(sPosition, endPos, 1 << LayerMask.NameToLayer("Players"));
            if(check.collider != null){
                isDetected = check.collider.gameObject.CompareTag("Player");
                Debug.DrawLine(sPosition, endPos, Color.green);
            }
            else {
                Debug.DrawLine(sPosition, endPos, Color.red);
            }
            return isDetected;
        }

        private void Follow(){
            if (InRange(1f) && !isDead){
                anim.SetBool(IsMoving, false);
                minionStates = StateMachine.Attack;
            }
            
            //! if readyToAttack = true, chase the target
            // if(readyToAttack){
                if (!InRange(1f) && !isDead){
                    anim.SetBool(IsMoving, true);
                    transform.position = isFacingRight 
                        ? Vector2.MoveTowards(transform.position, !_targetMv.facingRight 
                            ? _pFront.position 
                            : _pBehind.position, movementS * Time.deltaTime) 
                        : Vector2.MoveTowards(transform.position, !_targetMv.facingRight 
                            ? _pBehind.position 
                            : _pFront.position, movementS * Time.deltaTime);
                }
            // } else {
            //     //! Don't Move, disable walk animation
            // }
        }
        #endregion

        #region  Attack, Vanish Timer
        private void AttackPlayer(){
            if (!didIMissAttack && targetHit != 1){
                anim.SetTrigger(Attack);
            } else {
                minionStates = StateMachine.Follow;
                targetHit = 0; //! Resetting Target_Hit and Miss Attack, so it can attack again;
                didIMissAttack = false;
            }
        }

        //! Description: It will be disable after its death animation 
        private IEnumerator Vanish(){
            isDead = true;
            anim.SetTrigger(Dead);
            yield return new WaitForSeconds(2f);
            gameObject.SetActive(false);
        }
        #endregion
        
        #region Flip the Character
        private void Flip(){
            isFacingRight = !isFacingRight;
            transform.Rotate(0, 180f, 0);
        }

        private void ChangeDirection() {
            //! transform.localPosition.x - transform.localPosition.x
            var localOffset = transform.position.x - _target.transform.position.x;
            switch (localOffset){
                case < 0 when !isFacingRight: Flip(); break;
                case > 0 when isFacingRight: Flip(); break;
            }
        }
        #endregion
        
        #region Receive Damage and Stun Timer
        public void ReceiveDamage(int dmg){
            if (isDead) return;
            anim.ResetTrigger(Attack);
            anim.SetBool(IsMoving, false);
            
            currentHealth -= dmg;
            minionHb.SetHealth(currentHealth);
            minionStates = currentHealth <= 0 ? StateMachine.Dead : StateMachine.Stun;
        }

        private IEnumerator CantMove(){
            anim.SetTrigger(Hit);
            yield return new WaitForSeconds(1f);
            if (!isDead) {
                minionStates = StateMachine.Follow;
            }
        }
        #endregion
    }
}
