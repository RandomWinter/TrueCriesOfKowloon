using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _06_Scripts._05_Boss {
    public class AhKom : MonoBehaviour {
        #region Variable
        [Header("Target Components Field")] 
        public GameObject targetInfo;
        private Transform _targetL;
        private Transform _targetR;
        private PlayerMovement _tMovement;

        [Header("Position Components")] 
        [SerializeField] private GameObject holdRight;
        [SerializeField] private GameObject holdLeft;
        
        [Header("Visual Fields")]
        public HealthBarUI bossHb;
        public Animator bossAnimation;

        [Header("Basic Setup + Line in Sight")] 
        [SerializeField] private Transform castPoint;
        public float castDistance;
        public bool isCheck;
        
        [SerializeField] private float mv = 4.25f;
        public int currentHealth = 40;

        [Header("Condition Event")]
        [SerializeField] private bool faceRight;
        [SerializeField] private bool defeated;
        public int comboHit;
        public bool missAttack; 

        [Header("1st Special: Bull Rush")] 
        private float bRChargeTime = 2f;
        private float bRCoolDown = 1.75f; //! Patch: 2s
        public bool bRActivated; //! Activated when In Position
        private bool _bRFinished; //! Special Finished
        private int _bRNum; //! Repeating Move
        private float _distance1; //!Compare point Distance with Target (L/R)
        private float _distance2;
        private Vector2 _lCharge;
        private Vector2 _rCharge;
        public bool isRCharge; //! Is he right charge?
        public bool recordOnce; //! Record Distance each move
        public bool checkOnce; //! Check for last y-position to charge
        
        [Header("2nd Special: Windmill")] 
        private float wMChaseTime = 5f;
        private float wMCoolDown = 1.25f; //! Patch: 2s ~ 2.5s
        private bool _wMFinished;
        public bool wMActivated;

        [Header("Event Countdown")] 
        [SerializeField] private float countDown1; //! Count for Special Duration
        [SerializeField] private float countDown2; //! Count for cool-down
        #endregion

        #region StateMachine and Animation
        public enum StateMachine{
            Chase, Attack, BullRush, WindMill, Defeated
        } public StateMachine ahKomStage;
        
        //! Animation Boolean
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Attack1 = Animator.StringToHash("Attack");
        private static readonly int Windmill = Animator.StringToHash("Windmill");
        private static readonly int Tired = Animator.StringToHash("tired");
        private static readonly int ChargeWalk = Animator.StringToHash("ChargeWalk");
        private static readonly int Charging = Animator.StringToHash("Charging");
        private static readonly int Dead = Animator.StringToHash("Dead");
        private static readonly int Hit = Animator.StringToHash("Hit");
        #endregion
        
        #region Core
        private void Awake(){
            //! Collect Player's Components
            targetInfo = GameObject.FindGameObjectWithTag("Player");
            _targetL = targetInfo.transform.Find("LeftTrigger");
            _targetR = targetInfo.transform.Find("RightTrigger");
            _tMovement = targetInfo.GetComponent<PlayerMovement>();
            
            bossAnimation = GetComponent<Animator>();
            bossHb.SetMaxHealth(currentHealth);
            holdLeft = GameObject.Find("L_END");
            holdRight = GameObject.Find("R_END");

            wMActivated = bRActivated = defeated = false;
            //ahKomStage = StateMachine.Chase;
        }

        private void Update(){
            if (wMActivated || bRActivated){
                countDown1 += Time.deltaTime; 
            }

            if (_wMFinished || _bRFinished) {
                countDown2 += Time.deltaTime;
            }
            
            switch(ahKomStage){
                case StateMachine.Chase: ChangeDirection(); Chasing(); break;
                case StateMachine.Attack: Attack(); break;
                case StateMachine.BullRush: BullRush(); break;
                case StateMachine.WindMill: WindMill(); break;
                case StateMachine.Defeated: Defeated(); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
        #endregion
        
        #region Chasing, Attack
        private void Chasing(){
            bossAnimation.SetBool(Walk, true);
            if (!InRange(1.5f)){
                transform.position = faceRight 
                    ? Vector2.MoveTowards(transform.position, !_tMovement.facingRight 
                        ? _targetR.position 
                        : _targetL.position, mv * Time.deltaTime) 
                    : Vector2.MoveTowards(transform.position, !_tMovement.facingRight 
                        ? _targetL.position 
                        : _targetR.position, mv * Time.deltaTime);
            }

            if (InRange(1.5f)){
                var selectSpecMov = Random.Range(0, 5);
                bossAnimation.SetBool(Walk, false);
                switch (selectSpecMov){
                    case <= 1 when !wMActivated && currentHealth <= 90: 
                        wMActivated = true;
                        ahKomStage = StateMachine.WindMill;
                        break;
                    case 2 when !bRActivated && currentHealth <= 50:
                        ahKomStage = StateMachine.BullRush;
                        break;
                    default:
                        ahKomStage = StateMachine.Attack;
                        break;
                }
            }
        }
        
        private bool InRange(float distance) {
            var startPosition = castPoint.position;
            isCheck = false;

            castDistance = faceRight switch{
                true => distance, false => -distance
            };
            
            Vector2 endPos = startPosition + Vector3.right * castDistance;
            RaycastHit2D hit = Physics2D.Linecast(startPosition, endPos, 1 << LayerMask.NameToLayer("Players"));
            if (hit.collider != null){
                isCheck = hit.collider.gameObject.CompareTag("Player");
            }
            
            return isCheck;
        }
        #endregion
        
        #region Attack, Windmill
        private void Attack(){
            if (!missAttack && comboHit != 1){
                bossAnimation.SetTrigger(Attack1);
            } else{
                ahKomStage = StateMachine.Chase;
                comboHit = 0; 
                missAttack = false;
            }
        }
        
        //! Windmill Rush Function well
        private void WindMill(){
            var mvWindMill = mv * 0.85f;
            if (countDown1 <= wMChaseTime){
                bossAnimation.SetBool(Windmill, true);
                transform.position = Vector2.MoveTowards(transform.position, targetInfo.transform.position,
                    mvWindMill * Time.deltaTime);
            } else {
                _wMFinished = true;
                bossAnimation.SetBool(Windmill, false);
                bossAnimation.SetTrigger(Tired);
                
                if (!(countDown2 >= wMCoolDown)) return;
                countDown1 = countDown2 = 0;
                wMActivated = _wMFinished = false; //! Reset WindMill variables
                ahKomStage = StateMachine.Chase; //! Return to Chase
            }
        }
        #endregion
        
        #region BullRush
        //! Bull Rush Function well
        private void BullRush(){
            Vector2 target = targetInfo.transform.position;
            Vector2 holdL = holdLeft.transform.position;
            Vector2 holdR = holdRight.transform.position;
            Vector2 bossP = transform.position;

            if (!bRActivated){
                if (!checkOnce){
                    _distance1 = Vector2.Distance(holdL, target);
                    _distance2 = Vector2.Distance(holdR, target);
                    checkOnce = true;
                }

                bossAnimation.SetBool(ChargeWalk, true);
                if (_distance1 > _distance2){
                    transform.position = Vector2.MoveTowards(bossP, holdL, mv * Time.deltaTime);
                    isRCharge = false;
                } else {
                    transform.position = Vector2.MoveTowards(bossP, holdR, mv * Time.deltaTime);
                    isRCharge = true;
                }

                if (bossP.x != holdL.x && bossP.x != holdR.x) return;
                bRActivated = true;
            }

            //! Track Player's Y-Position
            if (countDown1 <= bRChargeTime && _bRNum != 2){
                bossAnimation.SetBool(ChargeWalk, true);
                var xTarget = new Vector2(bossP.x, target.y);
                transform.position = Vector2.MoveTowards(bossP, xTarget, mv * Time.deltaTime);
                Flip();
                return;
            }

            //! Record Player's Final Position
            if (!recordOnce && _bRNum != 2){
                _lCharge = new Vector2(holdL.x, target.y);
                _rCharge = new Vector2(holdR.x, target.y);
                recordOnce = true;
            }

            if (countDown1 >= bRChargeTime && _bRNum != 2){
                bossAnimation.SetBool(ChargeWalk, false);
                bossAnimation.SetTrigger(Charging);
                if (isRCharge) {
                    transform.position = Vector2.MoveTowards(bossP, _lCharge, 6 * mv * Time.deltaTime);
                    if (bossP.x == holdL.x) {
                        _bRNum += 1;
                        countDown1 = 0;
                        recordOnce = false;
                        isRCharge = !isRCharge;
                    }
                } else {
                    transform.position = Vector2.MoveTowards(bossP, _rCharge, 6 * mv * Time.deltaTime);
                    if(bossP.x == holdR.x){
                        _bRNum += 1;
                        countDown1 = 0;
                        recordOnce = false;
                        isRCharge = !isRCharge;
                    }
                }
            } else {
                bossAnimation.ResetTrigger(Charging);
                bossAnimation.SetTrigger(Tired);
                _bRFinished = true;
            }

            //! Used Tired Animation
            if (!(countDown2 >= bRCoolDown) || _bRNum != 2) return;
                bossAnimation.ResetTrigger(Tired);
                countDown1 = countDown2 = 0;
                checkOnce = recordOnce = false;
                ahKomStage = StateMachine.Chase;
        }
        #endregion
        
        #region DamageReceived, Stun, Defeated
        public void DamageReceived(int dmg) {
            if (defeated) return;

            Debug.Log("Ah Kom has taken damage");
            currentHealth -= dmg;
            bossHb.SetMaxHealth(currentHealth);
            
            if (!wMActivated || !bRActivated){
                bossAnimation.SetTrigger(Hit);
            }

            if (currentHealth > 0) return;
            ahKomStage = StateMachine.Defeated;
        }
        
        private void Defeated(){
            //Death Animation
            defeated = true;
            bossAnimation.SetTrigger(Dead);
        }
        #endregion
        
        #region Flip the Character
        private void Flip(){
            faceRight = !faceRight;
            transform.Rotate(0, 180f, 0);
        }

        private void ChangeDirection(){
            var localOffset = transform.position.x - targetInfo.transform.position.x;
            switch(localOffset){
                case < 0 when !faceRight: Flip(); break;
                case > 0 when faceRight: Flip(); break;
            }
        }
        #endregion
    }
}
