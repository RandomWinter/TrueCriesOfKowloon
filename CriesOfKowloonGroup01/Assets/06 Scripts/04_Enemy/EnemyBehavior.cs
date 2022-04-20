using System;
using UnityEngine;

namespace _06_Scripts._04_Enemy{
    public class EnemyBehavior : MonoBehaviour{
        #region Variables
        [Header("Components Field")]
        [SerializeField] private GameObject playerReference;
        [SerializeField] private Transform leftPosition;
        [SerializeField] private Transform rightPosition;
        [SerializeField] private PlayerMovement playerMovement;

        [Header("Visual Component")]
        public HealthBarUI hb;
        public Animator anim;

        [Header("Event Timer")] 
        [SerializeField] private bool stunActivated;
        [SerializeField] private float stunTimer;
        public float coolDownTimer = 0.5f;
        public int hit;
        
        [Header("Classic Stats")]
        [SerializeField] private float searchRadius = 8.5f;
        [SerializeField] private float mv = 3.15f;
        
        [SerializeField] private int maxHealth = 20;
        [SerializeField] private int baseHealth;
        
        [SerializeField] private Transform castPoint;
        public float castDistance;
        public bool isCheck;

        [Header("Condition Event")]
        public bool prepareForAttack;
        public bool missAttack;
        public bool faceRight;
        public bool sawPlayer;
        public bool isDead;
        
        public enum StateMachine{
            Idle, Chase, Attack, Stun
        } public StateMachine currentState;
        private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
        private static readonly int IsMoving = Animator.StringToHash("isMoving");
        private static readonly int IsHit = Animator.StringToHash("isHit");
        private static readonly int IsDead = Animator.StringToHash("isDead");
        #endregion

        #region Awake and Update
        private void Awake(){
            playerReference = GameObject.FindGameObjectWithTag("Player");
            leftPosition = playerReference.transform.Find("LeftTrigger");
            rightPosition = playerReference.transform.Find("RightTrigger");
            playerMovement = playerReference.GetComponent<PlayerMovement>();
            
            anim = GetComponent<Animator>();
            baseHealth = maxHealth; 
            hb.SetMaxHealth(maxHealth);

            sawPlayer = false;
            currentState = StateMachine.Idle;
        }

        private void Update() {
            if (stunActivated){
                stunTimer += Time.deltaTime;
            }

            switch(currentState){
                case StateMachine.Idle:
                    Idle();
                    break;
                case StateMachine.Chase:
                    Flip();
                    Chasing();
                    SoundManager.PlaySound("Footstep");
                    break;
                case StateMachine.Attack:
                    AttackPlayer();
                    break;
                case StateMachine.Stun:
                    stunActivated = true;
                    Stunning();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region States Field
        private void Idle(){
            Vector2 radar = transform.position - playerReference.transform.position;
            
            if (!(radar.sqrMagnitude < searchRadius * searchRadius)) return;
            currentState = StateMachine.Chase;
            sawPlayer = true;
        }

        private void Chasing() {
            if (InRange(1.5f)){
                anim.SetBool(IsMoving, false);
                currentState = StateMachine.Attack;   
            }

            if (prepareForAttack){
                if (!InRange(1.5f)){
                    anim.SetBool(IsMoving, true);
                    transform.position = faceRight 
                        ? Vector2.MoveTowards(transform.position, !playerMovement.facingRight 
                            ? rightPosition.position 
                            : leftPosition.position, mv * Time.deltaTime) 
                        : Vector2.MoveTowards(transform.position, !playerMovement.facingRight 
                            ? leftPosition.position 
                            : rightPosition.position, mv * Time.deltaTime);
                }
            } else {
                anim.SetBool(IsMoving, false);
            }
        }

        private void AttackPlayer(){
            if(!missAttack){
                anim.SetBool(IsAttacking, true);
            } else {
                hit = 0; missAttack = false;
                anim.SetBool(IsAttacking, false);
                currentState = StateMachine.Chase;
            }
        }

        private void DeathReset() {
            isDead = true;
            anim.SetBool(IsDead, true);
            gameObject.SetActive(false);
        }
        
        private void Stunning(){
            anim.SetBool(IsHit, true);
            if(stunTimer >= coolDownTimer){
                anim.SetBool(IsHit, false);
                currentState = StateMachine.Chase;
                stunActivated = false;
                stunTimer = 0;
            }
        }
        #endregion

        #region Flip, InRange, ReceiveDamage
        private void Flip() {
            var localOffset = transform.localPosition.x - playerReference.transform.localPosition.x;
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
        
        private bool InRange(float distance) {
            var startPosition = castPoint.position;
            castDistance = distance; 
            isCheck = false;
            
            if(!faceRight){
                castDistance = -distance;
            }
            
            Vector2 endPos = startPosition + Vector3.right * castDistance;
            RaycastHit2D at = Physics2D.Linecast(startPosition, endPos, 1 << LayerMask.NameToLayer("Players"));
            if (at.collider != null){
                isCheck = at.collider.gameObject.CompareTag("Player");
                Debug.DrawLine(startPosition, endPos, Color.green);
            } else {
                Debug.DrawLine(startPosition, endPos, Color.yellow);
            }
            return isCheck;
        }

        public void ReceiveDamage(int damage) {
            anim.SetBool(IsAttacking, false);
            anim.SetBool(IsMoving, false);
            
            baseHealth -= damage;
            hb.SetHealth(baseHealth);
            
            if(baseHealth <= 0){
                SoundManager.PlaySound("EnemyDeath");
                DeathReset();
            } currentState = StateMachine.Stun;
        }
        #endregion
    }
}
