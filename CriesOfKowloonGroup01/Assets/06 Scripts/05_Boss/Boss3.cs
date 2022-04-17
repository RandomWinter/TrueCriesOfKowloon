using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace _06_Scripts._05_Boss {
    public class Boss3 : MonoBehaviour {
        #region Variable Setup
        //! Search Player's Transform and Script
        private GameObject _target;
        private Transform _pFront;
        private Transform _pBehind;
        private PlayerMovement _targetMv;

        //! Connect with Health Bar and Animator (Visual Effects)
        public HealthBarUI b3Hb;
        public Animator b3Anim;
        public Rigidbody2D rb2d;

        //! Health and Mobility
        [SerializeField] private float mobility = 3f;
        [HideInInspector] public int currentHealth = 100;

        //! Normal Attack
        public bool normalAtt;
        public bool missAttack;
        public int targetHit;
    
        //! Timer
        private float _sMoveTimer = 1f;
        private float _countDown;
    
        //! Straight Kick
        private Vector2 _kickCoordinate;
        public bool xKickActive;
        private bool _readOnce;
        private int _kickNum;
        public float kickForce = 2;

        //! Inch Punch
        public bool inchActivate;
        [FormerlySerializedAs("_inchDash")] public float inchDash = 15;
    
        //! Raycast for Attack
        [SerializeField] private Transform castPoint;
        [HideInInspector] public float castDistance;
        [HideInInspector] public bool isDetected;
        public float adjDist = 1.5f;

        private bool _isFacingRight;
        public bool isTired;
        public bool isDead;
        
        private bool stunOnce;
        private bool deadOnce;
        #endregion
    
        #region StateMachine, Animation
        public enum StateMachine3 {
            Follow, Attack, StraightKick, InchPunch, Defeat, Stun
        } public StateMachine3 boss3States;
        private static readonly int IsMove = Animator.StringToHash("isMove");
        private static readonly int Attack1 = Animator.StringToHash("attack");
        private static readonly int Charge = Animator.StringToHash("charge");
        private static readonly int Kick = Animator.StringToHash("straightKick");
        private static readonly int Punch = Animator.StringToHash("inchPunch");
        private static readonly int Tired1 = Animator.StringToHash("tired");
        private static readonly int Death = Animator.StringToHash("death");

        #endregion
    
        #region Awake, Start, Update
        private void Awake(){
            _target = GameObject.FindGameObjectWithTag("Player");
            _pFront = _target.transform.Find("RightTrigger");
            _pBehind = _target.transform.Find("LeftTrigger");
            _targetMv = _target.GetComponent<PlayerMovement>();

            b3Anim = GetComponent<Animator>();
            b3Hb.SetMaxHealth(currentHealth);

            boss3States = StateMachine3.Follow;
        }

        private void Update(){
            if (xKickActive) {
                _countDown += Time.deltaTime;
            }

            switch (boss3States){
                case StateMachine3.Follow: ChangeDirection(); Follow(); break;
                case StateMachine3.Attack: normalAtt = true; Attack(); break; 
                case StateMachine3.InchPunch: InchPunch(); break;
                case StateMachine3.StraightKick: xKickActive = true; StraightKick(); break;
                case StateMachine3.Defeat: StartCoroutine(Vanish()); break;
                case StateMachine3.Stun: StartCoroutine(Hit()); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region Follow, InRange, Attack
        private void Follow(){
            if (isDead) return;
            if(InRange(adjDist)){
                b3Anim.SetBool(IsMove, false);
                var selectSpecMove = Random.Range(0, 5);
                switch (selectSpecMove){
                    case <= 1 when currentHealth <= 80: boss3States = StateMachine3.StraightKick; break;
                    case 2 when currentHealth <= 50: inchActivate = true; boss3States = StateMachine3.InchPunch; break;
                    default: boss3States = StateMachine3.Attack; break;
                }
                return;
            }
        
            if (!InRange(adjDist) && !isDead){
                b3Anim.SetBool(IsMove, true);
                transform.position = _isFacingRight 
                    ? Vector2.MoveTowards(transform.position, !_targetMv.facingRight 
                        ? _pFront.position 
                        : _pBehind.position, mobility * Time.deltaTime) 
                    : Vector2.MoveTowards(transform.position, !_targetMv.facingRight 
                        ? _pBehind.position 
                        : _pFront.position, mobility * Time.deltaTime);
            }
        }
    
        private bool InRange(float dist){
            var sPosition = castPoint.position;
            isDetected = false;
            
            castDistance = _isFacingRight switch {
                true => dist, false => -dist
            };

            Vector2 endPos = sPosition + Vector3.right * castDistance;
            var check = Physics2D.Linecast(sPosition, endPos, 1 << LayerMask.NameToLayer("Players"));
            if (check.collider != null){
                isDetected = check.collider.gameObject.CompareTag("Player");
                Debug.DrawLine(sPosition, endPos, Color.green);
            }
            else
            {
                Debug.DrawLine(sPosition, endPos, Color.red);
            }
        
            return isDetected;
        }
    
        private void Attack(){
            if (!missAttack && targetHit != 1){
                b3Anim.SetTrigger(Attack1);
            } else {
                boss3States = StateMachine3.Follow;
                missAttack = false;
                normalAtt = false;
                targetHit = 0;
            }
        }
        #endregion

        #region Special Move 1 & 2
        private void StraightKick() {
            Vector2 tar = _target.transform.position;
            Vector2 cur = transform.position;

            b3Anim.SetBool(Charge, true);
            if (_countDown <= _sMoveTimer) return;
            if (!_readOnce && _kickNum != 3){
                _kickCoordinate = new Vector2(tar.x, tar.y);
                ChangeDirection();
                _readOnce = true;
            }
            
            b3Anim.SetBool(Charge, false);
            if (_kickNum != 3){
                b3Anim.SetTrigger(Kick);
                transform.position = Vector2.MoveTowards(cur, _kickCoordinate, kickForce * (mobility * Time.deltaTime));
                
                if (cur != _kickCoordinate) return;
                b3Anim.ResetTrigger(Kick);
                _kickNum += 1;
                _countDown = 0;
                _readOnce = false;
            } else {
                StartCoroutine(Tired());
            }
        }
    
        private void InchPunch(){
            if (inchActivate){
                b3Anim.SetTrigger(Punch);
                rb2d.velocity = _isFacingRight switch{
                    true => Vector2.right * inchDash,
                    false => Vector2.left * inchDash
                };
                inchActivate = false;
            }

            StartCoroutine(Tired());
        }

        private IEnumerator Tired(){
            isTired = true;
            b3Anim.ResetTrigger(Kick);
            b3Anim.ResetTrigger(Punch);
            b3Anim.SetBool(Tired1, true);
            
            yield return new WaitForSeconds(2f);
            _countDown = 0;
            b3Anim.SetBool(Tired1, false);
            _readOnce = xKickActive = isTired = false;
            boss3States = StateMachine3.Follow;
        }

        #endregion
    
        #region Flip Character
        private void Flip(){
            _isFacingRight = !_isFacingRight;
            transform.Rotate(0, 180f, 0);
        }

        private void ChangeDirection(){
            var localOffSet = transform.position.x - _target.transform.position.x;
            switch (localOffSet){
                case < 0 when !_isFacingRight: Flip(); break;
                case > 0 when _isFacingRight: Flip(); break;
            }
        }
        #endregion
        
        #region Receive Damage, Stun Timer, Defeated
        public void ReceiveDamage(int dmg){
            if (isDead) return;
            b3Anim.SetBool(IsMove, false);
            b3Anim.ResetTrigger(Attack1);
            
            currentHealth -= dmg;
            b3Hb.SetHealth(currentHealth);
            if (currentHealth > 0){
                if (!inchActivate && !xKickActive && !isTired){
                    boss3States = StateMachine3.Stun;
                }
            } else {
                boss3States = StateMachine3.Defeat;
            }
        }
        private IEnumerator Hit(){
            if (!stunOnce) {
                stunOnce = true;
            }
            
            yield return new WaitForSeconds(1.5f);
            if (!isDead) {
                stunOnce = false;
                boss3States = StateMachine3.Follow;
            }
        }

        private IEnumerator Vanish(){
            isDead = true;
            if (!deadOnce) {
                b3Anim.SetTrigger(Death);
                deadOnce = true;
            }
            
            yield return new WaitForSeconds(3.5f);
            gameObject.SetActive(false);
        }
        #endregion
    }
}
