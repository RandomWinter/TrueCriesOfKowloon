using System;
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
        
        //Position Components 1 ~ 6
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
        public int currentHealth = 30;
        public bool normalAttack;
        public int chainHit;
        
        //!Knife Field
        public GameObject knifePrefab;
        public Transform throwPoint;
        
        private float kTChargeTime = 1.25f; //Knife Throw Timer
        private float _throwKnife;
        
        public int waveKnife;
        public int numKnife = 2;
        public int knifeThrown;
        
        private bool _distanceCheck;
        private bool _kTActivated;
        public bool kTFinish;
        
        private Vector2 _direction;
        
        //!Spin Death Field
        private float sDChargeTime = 1f;
        private float sDChaseTime = 5f;
        private float sDCoolDown = 2f;
        private bool _sDPrepared;
        public bool _sDActivated;
        private bool _sDFinished; 
        
        //!Emotional Damage Field
        [SerializeField] private float eDCoolDown = 1.25f; //Emotional Damage Timer
        public bool eDActivated;
        public bool eDFinish;
        
        [Header("Event Countdown / CoolDown")]
        private float _chargeDown; //Specified for Spin Death
        private float _countDown;
        private float _coolDown;
        private float fireRate = 2f;

        [Header("Condition Event")]
        private bool _faceRight; //Ensure it faces Player
        private bool _defeated; //Ensure it has been defeated by Player
        public bool missAtt; //Check whether it missed
        #endregion
        
        //==========================================================================
        
        #region StateMachine and Animation
        public enum StateMachine {
            Chase, Attack, KnifeThrow, SpinDeath, EmotionalDamage, Dead
        } public StateMachine yuLingState;
        #endregion

        private void Awake() {
            allPoint = GameObject.FindGameObjectsWithTag("BulletPosition");
            
            _target = GameObject.FindGameObjectWithTag("Player");
            _tFront = _target.transform.Find("LeftTrigger");
            _tBehind = _target.transform.Find("RightTrigger");
            _targetMv = _target.GetComponent<PlayerMovement>();
            
            //bossAnim = GetComponent<Animator>();
            //bossHB.SetMaxHealth(currentHealth);
            _throwKnife = Time.time;

            _sDActivated = _kTActivated = eDActivated = false;
            //yuLingState = StateMachine.KnifeThrow;
            yuLingState = StateMachine.SpinDeath;
        }
        private void Update() {
            if (_sDPrepared){
                _chargeDown += Time.deltaTime; //Only for Spin Death
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
                case StateMachine.Dead: Death(); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
        
        //==========================================================================

        private void ChasePlayer(){
            //When Player didn't enter the attack Collider
            if (!_target.transform){
                if (_faceRight){
                    transform.position = 
                        Vector2.MoveTowards(transform.position, !_targetMv.facingRight
                            ? _tFront.position
                            : _tBehind.position, movement * Time.deltaTime);
                } else {
                    transform.position = 
                        Vector2.MoveTowards(transform.position, !_targetMv.facingRight
                            ? _tFront.position
                            : _tBehind.position, movement * Time.deltaTime);
                }
            }    

            if (_target.transform) {
                var selectSpecMove = Random.Range(0, 6);
                switch (selectSpecMove) {
                    //Lower 90% Health + 1/3 Chances
                    case <= 1 when !_kTActivated && currentHealth <= 90:
                        yuLingState = StateMachine.KnifeThrow;
                        break;
                    //Lower 60% Health + 1/6 Chances
                    case 2 when !_sDActivated && currentHealth <= 60:
                        yuLingState = StateMachine.SpinDeath;
                        break;
                    //Lower 30% Health + 1/6 Chances
                    case 3 when !eDActivated && currentHealth <= 30:
                        yuLingState = StateMachine.EmotionalDamage;
                        break;
                    default:
                        yuLingState = StateMachine.Attack;
                        break;
                }
            }
        }

        private void Attack(){
            normalAttack = true;
            if (!missAtt && chainHit != 1) {
               //! animation set trigger
            } else {
                yuLingState = StateMachine.Chase;
                normalAttack = false;
                missAtt = false;
                chainHit = 0;
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void KnifeThrow(){
            Vector2 targetPos = _target.transform.position;
            
            if (!_kTActivated){
                FindFurthestPoint();
            }

            //Perform Throw Knife Animation
            if (_countDown > kTChargeTime && waveKnife != 6){
                switch (currentHealth){
                    case <= 30: numKnife = 9; break;
                    case <= 60: numKnife = 6; break;
                    default: numKnife = 3; break;
                }
                
                if (Time.time > _throwKnife){
                    Instantiate(knifePrefab, throwPoint.position, Quaternion.identity);
                    _throwKnife = Time.time + (fireRate * 2);
                    knifeThrown++;
                }
                
                if(knifeThrown == numKnife){
                    _countDown = knifeThrown = 0;
                    waveKnife += 1;
                }
            } else if(waveKnife == 6){
                //Switch to Chase mode, reset all variable
                yuLingState = StateMachine.Chase;
                _distanceCheck = _kTActivated = false;
                _countDown = waveKnife = 0;
                _distList.Clear();
            }
        }
        
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
                //Stop for 2 seconds
                if (!(_coolDown >= sDCoolDown)) return;
                    print("Stop");
                    _sDActivated = false;
                    _coolDown = _countDown = _chargeDown = 0;
                    yuLingState = StateMachine.Chase;
            }
        }

        private void EmotionalDamage(){
            //Activate this animation
            if (!eDFinish){
                //Player will be stun for 2 seconds (base on Animation duration or audio)
                //Player will received de-buff for 5s, Set Player's De_buff to true
                //Attack reduce 12.5%
                //Mobility drop to 5%
                eDFinish = true;
                return;
            }

            if (!(_coolDown >= eDCoolDown)) return;
            _coolDown = _countDown = 0;
            yuLingState = StateMachine.Chase;
        }

        private void Death(){
            //Death Animation
            _defeated = true; //Activate Cutscene from this Lady
            
            //Placeholder, cause Unity annoyed me second time
            if(_defeated){
                
            }
        }
        
        //==========================================================================
        //Method Field 2

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
                //Once in position, enable kTActivated for countdown
                _kTActivated = true;
            } else {
                transform.position = Vector2.MoveTowards(transform.position, _furthestObj.transform.position,
                    movement * Time.deltaTime * 3);
            }
        }
        
        private void Flip(){
            var localOffset = transform.localPosition.x - _target.transform.localPosition.x;
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

        public void DamageReceived(int damage){
            //Enable Hit Animation, Disable other animation, Check condition
            currentHealth -= damage;
            //Update UI Health bar
            
            if (currentHealth > 0) return;
                yuLingState = StateMachine.Dead;
        }
    }
}
