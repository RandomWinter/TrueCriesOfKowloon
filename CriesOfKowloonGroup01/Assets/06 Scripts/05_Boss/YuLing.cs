using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _06_Scripts._05_Boss {
    public class YuLing : MonoBehaviour {
        #region Components Field
        private GameObject _target;
        private Transform _tFront;
        private Transform _tBehind;
        private PlayerMovement _targetMv;
        
        [SerializeField] private GameObject[] allPoint;
        private Dictionary<float, GameObject> _distList = new Dictionary<float, GameObject>();
        private GameObject _furthestObj;
        
        [Header("Visual Field")] 
        public HealthBarUI bossHb;
        public Animator bossAnim2;
        #endregion

        #region Variables Field
        //! It's Health and Movement
        [SerializeField] private float movement = 4.5f;
        public int currentHealth = 100;
        public bool normalAttack;
        public int chainHit;
        
        //! Raycast for Attack
        [SerializeField] private Transform castPoint;
        [HideInInspector] public float castDistance;
        [HideInInspector] public bool isDetected;
        public float adjDist = 1.5f;
        #endregion
        
        #region Knife Throw
        public GameObject knifePrefab;
        public Transform throwPoint;
        
        private float kTChargeTime = 1.25f; //Knife Throw Timer
        private float _throwKnife = 1;
        
        public int waveKnife;
        public int numKnife = 2;
        public int knifeThrown = 1;
        
        private bool _distanceCheck;
        private bool _kTActivated;
        public bool kTFinish;
        #endregion
        
        #region Spin Death
        private float sDChargeTime = 1f;
        private float sDChaseTime = 5f;
        private float sDCoolDown = 2f;
        private bool _sDPrepared;
        public bool _sDActivated;
        private bool _sDFinished; 
        #endregion
        
        #region Emotional Damage
        [SerializeField] private float eDCoolDown = 1.25f; //Emotional Damage Timer
        public bool eDActivated;
        public bool eDFinish;
        #endregion
        
        #region Countdown and Event
        [Header("Event Countdown / CoolDown")]
        private float _chargeDown; //Specified for Spin Death
        private float _countDown;
        private float _coolDown;
        private float fireRate = 2f;

        [Header("Condition Event")]
        private bool _faceRight; //Ensure it faces Player
        private bool _defeated; //Ensure it has been defeated by Player
        private bool deadOnce;
        private bool stunOnce;
        public bool missAtt; //Check whether it missed
        #endregion
        
        //==========================================================================
        
        #region StateMachine and Animation
        public enum StateMachine {
            Chase, Attack, KnifeThrow, SpinDeath, EmotionalDamage, Dead
        } public StateMachine yuLingState;
        private static readonly int IsWalk = Animator.StringToHash("isWalk");
        private static readonly int Attack1 = Animator.StringToHash("Attack");
        private static readonly int Hold = Animator.StringToHash("hold");
        private static readonly int Throw = Animator.StringToHash("throw");
        private static readonly int IsEd = Animator.StringToHash("isED");
        private static readonly int IsTired = Animator.StringToHash("IsTired");
        private static readonly int Death1 = Animator.StringToHash("death");
        private static readonly int Hit = Animator.StringToHash("hit");
        #endregion
        
        //==========================================================================

        #region Awake, Update
        private void Awake() {
            allPoint = GameObject.FindGameObjectsWithTag("BulletPosition");
            _target = GameObject.FindGameObjectWithTag("Player");
            _tFront = _target.transform.Find("RightTrigger");
            _tBehind = _target.transform.Find("LeftTrigger");
            _targetMv = _target.GetComponent<PlayerMovement>();
            
            bossAnim2 = GetComponent<Animator>();
            bossHb.SetMaxHealth(currentHealth);
            _throwKnife = Time.time;
            
            yuLingState = StateMachine.Chase;
        }
        
        private void Update() {
            if (_sDPrepared){ 
                _chargeDown += Time.deltaTime;
            }

            if (_sDActivated || _kTActivated || eDActivated){
                _countDown += Time.deltaTime;
            }
            
            if (_sDActivated || eDFinish || kTFinish){
                _coolDown += Time.deltaTime;
            }

            switch (yuLingState) {
                case StateMachine.Chase: Flip(); ChasePlayer(); break;
                case StateMachine.Attack: Attack(); break;
                case StateMachine.KnifeThrow: KnifeThrow(); break;
                case StateMachine.SpinDeath: _sDPrepared = true; SpinDeath(); break;
                case StateMachine.EmotionalDamage: EmotionalDamage(); break;
                case StateMachine.Dead: StartCoroutine(Death()); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region Chase, InRange, Attack
        private void ChasePlayer(){
            if (InRange(1.5f)) {
                bossAnim2.SetBool(IsWalk, false);
                var selectSpecMove = Random.Range(0, 6);
                switch (selectSpecMove) {
                    case <= 1 when !_kTActivated && currentHealth <= 80:
                        yuLingState = StateMachine.KnifeThrow;
                        break;
                    case 2 when !_sDActivated && currentHealth <= 50:
                        yuLingState = StateMachine.SpinDeath;
                        break;
                    case 3 when !eDActivated && currentHealth <= 25:
                        yuLingState = StateMachine.EmotionalDamage;
                        break;
                    default:
                        yuLingState = StateMachine.Attack;
                        break;
                }

                return;
            }
            
            if (!InRange(1.5f)){
                bossAnim2.SetBool(IsWalk, true);
                transform.position = _faceRight 
                    ? Vector2.MoveTowards(transform.position, !_targetMv.facingRight 
                        ? _tFront.position 
                        : _tBehind.position, movement * Time.deltaTime) 
                    : Vector2.MoveTowards(transform.position, !_targetMv.facingRight 
                        ? _tBehind.position 
                        : _tFront.position, movement * Time.deltaTime);
            }
        }
        
        private bool InRange(float dist){
            var sPosition = castPoint.position;
            isDetected = false;
            castDistance = _faceRight switch {
                true => dist, false => -dist
            };

            Vector2 endPos = sPosition + Vector3.right * castDistance;
            var check = Physics2D.Linecast(sPosition, endPos, 1 << LayerMask.NameToLayer("Players"));
            if (check.collider != null){
                isDetected = check.collider.gameObject.CompareTag("Player");
            }
        
            return isDetected;
        }

        private void Attack(){
            normalAttack = true;
            if (!missAtt && chainHit != 1) {
                bossAnim2.SetTrigger(Attack1);
            } else {
                yuLingState = StateMachine.Chase;
                normalAttack = false;
                missAtt = false;
                chainHit = 0;
            }
        }
        #endregion
        
        //==========================================================================
        
        #region Furthest Point, Knife Throw
        private void FindFurthestPoint(){
            //Chose the furthest point away from Player, Once Found set true
            if (!_distanceCheck){
                foreach (GameObject currentPoint in allPoint){
                    var dist = (_target.transform.position - currentPoint.transform.position).sqrMagnitude;
                    _distList.Add(dist, currentPoint);
                } 
                var distances = _distList.Keys.ToList();
                distances.Sort();

                _furthestObj = _distList[distances[^1]];
                _distanceCheck = true;
            }

            if (transform.position.x == _furthestObj.transform.position.x) {
                bossAnim2.SetBool(Hold, false);
                _kTActivated = true;
            } else {
                bossAnim2.SetBool(Hold, true);
                transform.position = Vector2.MoveTowards(transform.position, _furthestObj.transform.position,
                    movement * Time.deltaTime * 3);
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void KnifeThrow(){
            if (!_kTActivated){
                FindFurthestPoint();
            }

            //Perform Throw Knife Animation
            if (_countDown > kTChargeTime && waveKnife != 3){
                bossAnim2.SetBool(Hold, true);
                if (Time.time > _throwKnife){
                    bossAnim2.SetBool(Hold, false);
                    bossAnim2.SetTrigger(Throw);
                    
                    Instantiate(knifePrefab, throwPoint.position, Quaternion.identity);
                    _throwKnife = Time.time + (fireRate * 2);
                    knifeThrown++;
                }
                
                if(knifeThrown == numKnife){
                    _countDown = knifeThrown = 0;
                    waveKnife += 1;
                }
            } else if(waveKnife == 6){
                yuLingState = StateMachine.Chase;
                _distanceCheck = _kTActivated = false;
                _countDown = waveKnife = 0;
                _throwKnife = Time.time;
                _distList.Clear();
            }
        }
        #endregion
        
        #region Spin Death, Emotional Damage
        private void SpinDeath(){
            //Speed increase 25%, Damage dealt lesser, but player will have shorter knock-back
            var sDSpeed = movement * 1.25f;
            
            if (_chargeDown <= sDChargeTime) return; //Hold for 2 seconds after charge animation performance.
            _sDActivated = true;
            _sDPrepared = false;
            
            if (_countDown <= sDChaseTime){
                //Enable Attack animation while following:
                
                //Follow Player while attacking 4 ~ 5 seconds
                transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, sDSpeed * Time.deltaTime);
            } else {
                bossAnim2.SetTrigger(IsTired);
                if (!(_coolDown >= sDCoolDown)) return;
                    print("Stop");
                    _sDActivated = false;
                    _coolDown = _countDown = _chargeDown = 0;
                    yuLingState = StateMachine.Chase;
            }
        }

        private void EmotionalDamage(){
            if (!eDFinish){
                bossAnim2.SetTrigger(IsEd);
                eDFinish = true;
                return;
            }
            
            bossAnim2.SetTrigger(IsTired);
            if (!(_coolDown >= eDCoolDown)) return;
            _coolDown = _countDown = 0;
            yuLingState = StateMachine.Chase;
        }
        #endregion 
        
        //==========================================================================
        
        #region Received Damage, Can't Move, and Death
        public void DamageReceived(int damage){
            if (_defeated) return;
            bossAnim2.SetBool(IsWalk, false);
            bossAnim2.ResetTrigger(Attack1);
            
            currentHealth -= damage;
            bossHb.SetHealth(currentHealth);
            if (currentHealth > 0){
                if (!eDActivated && !_kTActivated && !_sDActivated){
                    StartCoroutine(CantMove());
                }
            } else {
                yuLingState = StateMachine.Dead;
            }
        }

        private IEnumerator CantMove(){
            if (!stunOnce){
                bossAnim2.SetTrigger(Hit);
                stunOnce = true;
            }

            yield return new WaitForSeconds(1.75f);
            if (!_defeated){
                stunOnce = false;
                yuLingState = StateMachine.Chase;
            }
        }
        
        private IEnumerator Death(){
            _defeated = true;
            if (!deadOnce){
                bossAnim2.SetTrigger(Death1);
                deadOnce = true;
            }

            yield return new WaitForSeconds(4.5f);
            gameObject.SetActive(false);
        }
        #endregion
        
        #region Flip
        private void Flip(){
            var localOffset = transform.position.x - _target.transform.position.x;
            switch (localOffset){
                case < 0 when !_faceRight:
                    _faceRight = !_faceRight;
                    transform.Rotate(0, 180f, 0);
                    break;
                case > 0 when _faceRight:
                    _faceRight = !_faceRight;
                    transform.Rotate(0, 180f, 0);
                    break;
            }
        }
        
        #endregion
        
        //==========================================================================
    }
}
