using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _06_Scripts._05_Boss {
    public class AhKom : MonoBehaviour {
        //! This extract Player's Components
        [Header("Target Components Field")] 
        [SerializeField] public GameObject targetInfo;
        [SerializeField] private Transform targetPosition;
        [SerializeField] private Transform targetL;
        [SerializeField] private Transform targetR;
        [SerializeField] private PlayerMovement tMovement;

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
        [SerializeField] private float bRChargeTime = 2f;
        [SerializeField] private float bRCoolDown = 1.75f; //! Patch: 2s
        [SerializeField] public bool bRActivated; //! Activated when In Position
        [SerializeField] private bool bRFinished; //! Special Finished
        [SerializeField] private int bRNum; //! Repeating Move
        [SerializeField] private float distance1; //!Compare point Distance with Target (L/R)
        [SerializeField] private float distance2;
        [SerializeField] private Vector2 lCharge;
        [SerializeField] private Vector2 rCharge;
        public bool isRCharge; //! Is he right charge?
        public bool recordOnce; //! Record Distance each move
        public bool checkOnce; //! Check for last y-position to charge
        
        [Header("2nd Special: Windmill")] 
        [SerializeField] private float wMChaseTime = 5f;
        [SerializeField] private float wMCoolDown = 1.25f; //! Patch: 2s ~ 2.5s
        [SerializeField] public bool wMActivated;
        [SerializeField] private bool wMFinished;

        [Header("Event Countdown")] 
        [SerializeField] private float countDown1; //! Count for Special Duration
        [SerializeField] private float countDown2; //! Count for cool-down

        public enum StateMachine{
            Chase, Attack, BullRush, WindMill, Defeated
        } public StateMachine ahKomStage;
        
        //! Animation Boolean
        private static readonly int IsHit = Animator.StringToHash("isHit");
        private static readonly int IsDead = Animator.StringToHash("isDead");
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
        private static readonly int IsWindMill = Animator.StringToHash("isWindMill");
        private static readonly int IsTired = Animator.StringToHash("isTired");
        private static readonly int IsChargeMove = Animator.StringToHash("isChargeMove");
        private static readonly int IsCharging = Animator.StringToHash("isCharging");

        private void Awake(){
            //! Collect Player's Components
            targetInfo = GameObject.FindGameObjectWithTag("Player");
            targetL = targetInfo.transform.Find("LeftTrigger");
            targetR = targetInfo.transform.Find("RightTrigger");
            tMovement = targetInfo.GetComponent<PlayerMovement>();
            
            bossAnimation = GetComponent<Animator>();
            bossHb.SetMaxHealth(currentHealth);
            holdLeft = GameObject.Find("L_END");
            holdRight = GameObject.Find("R_END");

            wMActivated = bRActivated = defeated = false;
            //ahKomStage = StateMachine.Chase;
        }

        private void Update(){
            if (wMActivated || bRActivated) //! Count when Special is triggered
                countDown1 += Time.deltaTime;

            if (wMFinished || bRFinished) //! Count when Special is finished
                countDown2 += Time.deltaTime;

            switch(ahKomStage){
                case StateMachine.Chase:
                    Flip();
                    Chasing();
                    break;
                case StateMachine.Attack:
                    Attack();
                    break;
                case StateMachine.BullRush:
                    BullRush();
                    break;
                case StateMachine.WindMill:
                    WindMill();
                    break;
                case StateMachine.Defeated:
                    Defeated();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        //! Method Field: State Event for Behaviour
        private void Chasing(){
            bossAnimation.SetBool(IsWalking, true);
            if (!InRange(1.5f)) {
                transform.position = faceRight 
                    ? Vector2.MoveTowards(transform.position, !tMovement.facingRight 
                        ? targetR.position 
                        : targetL.position, mv * Time.deltaTime) 
                    : Vector2.MoveTowards(transform.position, !tMovement.facingRight 
                        ? targetL.position 
                        : targetR.position, mv * Time.deltaTime);
            }

            if (InRange(1.5f)){
                var selectSpecMov = Random.Range(0, 5);
                bossAnimation.SetBool(IsWalking, false);
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
        
        private void Attack(){
            if (!missAttack && comboHit < 1){
                bossAnimation.SetBool(IsAttacking, true);
            } else{
                comboHit = 0;
                bossAnimation.SetBool(IsAttacking, false);
                ahKomStage = StateMachine.Chase;
            }
        }
        
        //! Windmill Rush Function well
        private void WindMill(){
            var mvWindMill = mv * 0.85f;
            if (countDown1 <= wMChaseTime){
                bossAnimation.SetBool(IsWindMill, true);
                transform.position = Vector2.MoveTowards(transform.position, targetInfo.transform.position,
                    mvWindMill * Time.deltaTime);
            } else {
                wMFinished = true;
                bossAnimation.SetBool(IsWindMill, false);
                bossAnimation.SetBool(IsTired, true);
                
                if (!(countDown2 >= wMCoolDown)) return;
                bossAnimation.SetBool(IsTired, false);
                countDown1 = countDown2 = 0;
                wMActivated = wMFinished = false; //! Reset WindMill variables
                ahKomStage = StateMachine.Chase; //! Return to Chase
            }
        }
        
        //! Bull Rush Function well
        private void BullRush(){
            Vector2 target = targetInfo.transform.position;
            Vector2 holdL = holdLeft.transform.position;
            Vector2 holdR = holdRight.transform.position;
            Vector2 bossP = transform.position;

            if (!bRActivated){
                if (!checkOnce){
                    distance1 = Vector2.Distance(holdL, target);
                    distance2 = Vector2.Distance(holdR, target);
                    checkOnce = true;
                }

                bossAnimation.SetBool(IsChargeMove, true);
                if (distance1 > distance2){
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
            if (countDown1 <= bRChargeTime && bRNum != 2){
                bossAnimation.SetBool(IsChargeMove, true);
                var xTarget = new Vector2(bossP.x, target.y);
                transform.position = Vector2.MoveTowards(bossP, xTarget, mv * Time.deltaTime);
                Flip();
                return;
            }

            //! Record Player's Final Position
            if (!recordOnce && bRNum != 2){
                lCharge = new Vector2(holdL.x, target.y);
                rCharge = new Vector2(holdR.x, target.y);
                recordOnce = true;
            }

            if (countDown1 >= bRChargeTime && bRNum != 2){
                bossAnimation.SetBool(IsChargeMove, false);
                bossAnimation.SetBool(IsCharging, true);
                if (isRCharge) {
                    transform.position = Vector2.MoveTowards(bossP, lCharge, 6 * mv * Time.deltaTime);
                    if (bossP.x == holdL.x) {
                        bossAnimation.SetBool(IsCharging, false);
                        bRNum += 1;
                        countDown1 = 0;
                        recordOnce = false;
                        isRCharge = !isRCharge;
                    }
                } else {
                    transform.position = Vector2.MoveTowards(bossP, rCharge, 6 * mv * Time.deltaTime);
                    if(bossP.x == holdR.x){
                        bossAnimation.SetBool(IsCharging, false);
                        bRNum += 1;
                        countDown1 = 0;
                        recordOnce = false;
                        isRCharge = !isRCharge;
                    }
                }
            } else {
                bossAnimation.SetBool(IsTired, true);
                bRFinished = true;
            }

            //! Used Tired Animation
            if (!(countDown2 >= bRCoolDown) || bRNum != 2) return;
                bossAnimation.SetBool(IsTired, false);
                countDown1 = countDown2 = 0;
                checkOnce = recordOnce = false;
                ahKomStage = StateMachine.Chase;
        }
        
        private void Defeated(){
            //Death Animation
            bossAnimation.SetBool(IsDead, true);
            defeated = true;
        }
        
        //! Method Field 2
        private bool InRange(float distance) {
            var startPosition = castPoint.position;
            castDistance = distance; 
            isCheck = false;
            
            if(!faceRight){
                castDistance = -distance;
            }
            
            Vector2 endPos = startPosition + Vector3.right * castDistance;
            RaycastHit2D hit = Physics2D.Linecast(startPosition, endPos, 1 << LayerMask.NameToLayer("Players"));
            if (hit.collider != null){
                isCheck = hit.collider.gameObject.CompareTag("Player");
                Debug.DrawLine(startPosition, endPos, Color.green);
            } else {
                Debug.DrawLine(startPosition, endPos, Color.yellow);
            }
            return isCheck;
        }
        
        private void Flip(){
            var localOffset = transform.localPosition.x - targetInfo.transform.localPosition.x;
            switch(localOffset){
                case < 0 when !faceRight:
                    faceRight = !faceRight;
                    transform.Rotate(0, 180f, 0);
                    break;
                case > 0 when faceRight:
                    faceRight = !faceRight;
                    transform.Rotate(0, 180f, 0);
                    break;
            }
        }

        public void DamageReceived(int dmg){
            currentHealth -= dmg;
            bossHb.SetMaxHealth(currentHealth);
            
            if (!wMActivated || !bRActivated){
                //! Enable Hit Animation in non-special move
                bossAnimation.SetBool(IsHit, true);
            } bossAnimation.SetBool(IsHit, false);
            
            if (currentHealth > 0) return;
            ahKomStage = StateMachine.Defeated;
        }
    }
}
