using System;
using System.Collections;
using UnityEngine;

namespace _06_Scripts._04_Enemy{
    //! This code is an improvement of previous script, simplified, and flexible for changes
    //! 1. Attack Range will be replaced by Radius or Collider, if raycast isn't working properly
    //! 2. Navmesh will Re-Enable back, avoiding the Invisible Wall
    //! 3. Animation will be mixed, WALK is under boolean, others will be a trigger animation (avoiding loop)
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
            Idle, Follow, Attack, Dead
        } public StateMachine minionStates;
        private static readonly int IsMoving = Animator.StringToHash("isMoving");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int IsHit = Animator.StringToHash("IsHit");
        private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
        private static readonly int Dead = Animator.StringToHash("dead");

        private void Awake(){
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
                case StateMachine.Dead: Death(); break;
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
                print("Raycast: Target has been Found");
            } return isDetected;
        }

        private void Follow(){
            if (InRange(1.5f)){
                anim.SetBool(IsMoving, false);
                minionStates = StateMachine.Attack;
            }
            
            //! if readyToAttack = true, chase the target
            // if(readyToAttack){
                if (!InRange(1.5f)){
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

        #region  Attack, Dead, Vanish Timer
        private void AttackPlayer(){
            if (!didIMissAttack && targetHit < 1){
                anim.SetBool(IsAttacking, true);
            } else {
                anim.SetBool(IsAttacking, false);
                targetHit = 0; //! Resetting Target_Hit and Miss Attack, so it can attack again;
                didIMissAttack = false;
                minionStates = StateMachine.Follow;
            }
        }

        private void Death(){
            if (!isDead){
                anim.SetTrigger(Dead);
                isDead = true;
            }
            
            StartCoroutine(Vanish());
        }

        //! Description: It will be disable after its death animation 
        private IEnumerator Vanish(){
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
        public void ReceiveDamage(int dmg) {
            anim.SetBool(IsMoving, false);
            anim.SetBool(IsAttacking, false);
            if (dmg > currentHealth){
                minionStates = StateMachine.Dead;
            }
            
            currentHealth -= dmg;
            anim.SetBool(IsHit, true);
            minionHb.SetHealth(currentHealth);

            if (dmg >= 10){
                StartCoroutine(CantMove());
            } else {
                anim.SetBool(IsHit, false);
                minionStates = StateMachine.Follow;
            }
        }

        private IEnumerator CantMove(){
            anim.SetBool(IsHit, true);
            anim.SetTrigger(Hit);
            yield return new WaitForSeconds(0.5f);
            anim.SetBool(IsHit, false);
            anim.ResetTrigger(Hit);
            minionStates = StateMachine.Follow;
        }
        #endregion
    }
}
