using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _06_Scripts._05_Boss {
    public class AhKom : MonoBehaviour {
        //! This extract Player's Components
        [Header("Target Components Field")] 
        [SerializeField] private GameObject targetInfo;
        [SerializeField] private Transform targetL;
        [SerializeField] private Transform targetR;
        [SerializeField] private PlayerMovement tMovement;

        [Header("Position Components")] 
        [SerializeField] private GameObject holdRight;
        [SerializeField] private GameObject holdLeft;
        
        //[Header("Visual Fields")]
        //public HealthBarUI bossHB;
        //public Animator bossAnimation;

        [Header("Basic Setup + Line in Sight")] 
        [SerializeField] private Transform castPoint;
        public float castDistance;
        public bool isCheck;
        [SerializeField] private float mv = 4.25f;
        public int currentHealth = 100;

        [Header("Condition Event")]
        [SerializeField] private bool faceRight;
        [SerializeField] private bool defeated;
        public int comboHit;
        public bool missAttack; 

        [Header("1st Special: Bull Rush")] 
        [SerializeField] private float bRChargeTime = 2f;
        [SerializeField] private float bRCoolDown = 1.75f; //! Patch: 2s
        [SerializeField] private bool bRActivated; //! Activated when In Position
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
        [SerializeField] private bool wMActivated;
        [SerializeField] private bool wMFinished;

        [Header("Event Countdown")] 
        [SerializeField] private float countDown1; //! Count for Special Duration
        [SerializeField] private float countDown2; //! Count for cool-down

        public enum StateMachine{
            Chase, Attack, BullRush, WindMill, Defeated
        } public StateMachine ahKomStage;

        private void Awake(){
            //! Collect Player's Components
            targetInfo = GameObject.FindGameObjectWithTag("Player");
            //targetL = targetInfo.transform.Find("LPosition");
            //targetR = targetInfo.transform.Find("RPosition");
            tMovement = targetInfo.GetComponent<PlayerMovement>();
            
            //bossAnimation = GetComponent<Animator>();
            //bossHB = SetMaxHealth(currentHealth);
            //holdLeft = GameObject.Find("L_END");
            //holdRight = GameObject.Find("R_END");

            wMActivated = bRActivated = defeated = false;
            ahKomStage = StateMachine.Chase;
            //ahKomStage = StateMachine.BullCharge; //! Special Testing
            //ahKomStage = StateMachine.WindMill;
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
            //! Target didn't enter Attack Collider
            if (!InRange(6)){
                transform.position = faceRight switch {
                    true => Vector2.MoveTowards(transform.position,
                        !tMovement.facingRight ? targetL.position : targetR.position, mv * Time.deltaTime),
                    false => Vector2.MoveTowards(transform.position,
                        !tMovement.facingRight ? targetL.position : targetR.position, mv * Time.deltaTime)
                };
            }

            if (InRange(6)){
                var selectSpecMov = Random.Range(0, 5);
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
            if (!missAttack && comboHit < 2){
                switch (comboHit){
                    case 0:
                        break;
                    case 1:
                        break;
                }
            } else{
                comboHit = 0;
                ahKomStage = StateMachine.Chase;
            }
        }
        
        private void WindMill(){
            var mvWindMill = mv * 0.85f;
            if (countDown1 <= wMChaseTime){
                //! Enable WindMill Animation
                transform.position = Vector2.MoveTowards(transform.position, targetInfo.transform.position,
                    mvWindMill * Time.deltaTime);
            } else {
                wMFinished = true;
                if (!(countDown2 >= wMCoolDown)) return;
                    wMActivated = wMFinished = false; //! Reset WindMill variables
                    countDown1 = countDown2 = 0;
                    ahKomStage = StateMachine.Chase; //! Return to Chase
            }
        }

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

            if (countDown1 <= bRChargeTime && bRNum != 2){
                var xTarget = new Vector2(bossP.x, bossP.y);
                transform.position = Vector2.MoveTowards(bossP, xTarget, mv * Time.deltaTime);
                return;
            }

            if (!recordOnce && bRNum != 2){
                lCharge = new Vector2(holdL.x, target.y);
                rCharge = new Vector2(holdR.x, target.y);
                recordOnce = true;
            }

            if (countDown1 >= bRChargeTime && bRNum != 2){
                switch (isRCharge){
                    case true:
                        transform.position = Vector2.MoveTowards(bossP, lCharge, 6 * mv * Time.deltaTime);
                        if(bossP.x == holdL.x){
                            bRNum += 1;
                            countDown1 = 0;
                            recordOnce = false;
                            isRCharge = !isRCharge;
                        }
                        break;
                    case false:
                        transform.position = Vector2.MoveTowards(bossP, rCharge, 6 * mv * Time.deltaTime);
                        if(bossP.x == holdL.x){
                            bRNum += 1;
                            countDown1 = 0;
                            recordOnce = false;
                            isRCharge = !isRCharge;
                        }
                        break;
                }
            } else {
                bRFinished = true;
            }

            //! Used Tired Animation
            if (!(countDown2 >= bRCoolDown) || bRNum != 2) return;
                countDown1 = countDown2 = 0;
                checkOnce = recordOnce = false;
                ahKomStage = StateMachine.Chase;
        }

        private void Defeated(){
            //Death Animation
            defeated = true;

            if (defeated){
                
            }
        }
        
        //! Method Field 2
        private bool InRange(float distance) {
            var startPosition = castPoint.position;
            castDistance = distance; 
            isCheck = false;
            
            if(!faceRight){
                castDistance = -distance;
            }
            
            Vector2 endPos = startPosition + Vector3.right * distance;
            RaycastHit2D hit = Physics2D.Linecast(startPosition, endPos, 1 << LayerMask.NameToLayer("Zone"));
            if (hit.collider != null){
                isCheck = hit.collider.gameObject.CompareTag("Player");
            } return isCheck;
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
            //! Update Health Bar UI
            
            if (!wMActivated || !bRActivated){
                //! Enable Hit Animation in non-special move
            }
            
            if (currentHealth > 0) return;
            ahKomStage = StateMachine.Defeated;
        }
    }
}
