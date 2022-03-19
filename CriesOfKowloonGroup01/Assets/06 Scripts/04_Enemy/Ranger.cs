using System;
using UnityEngine;

namespace _06_Scripts._04_Enemy
{
    public class Ranger : MonoBehaviour
    {
        [Header("Target Components Field")] [SerializeField]
        private GameObject targetInfo;

        [SerializeField] private Transform targetL;
        [SerializeField] private Transform targetR;
        [SerializeField] private PlayerMovement tMovement;

        [Header("Visual Fields")] public HealthBarUI rangerHealthBar;
        public Animator rangerAnimation;

        [Header("Basic Setup + LineInSight")] [SerializeField]
        private float searchRadius = 10f;

        [SerializeField] private int currentHealth = 20;
        [SerializeField] private float mv = 3.25f;
        public Collider2D c2D;

        [SerializeField] private Transform castPoint;
        public float castDistance;
        public bool isCheck;

        [Header("Event Timer & Countdown")] [SerializeField]
        private float perishTimer = 2.5f;

        [SerializeField] private float stunTimer = 1.5f;
        [SerializeField] private float preFire = 1.25f;
        [SerializeField] private float countDown1;
        [SerializeField] private float countDown2;

        [Header("Projectile Components")] public Transform launchOffset;
        public RangerBullet bullet;

        [Header("Condition Event")] [SerializeField]
        private bool faceRight;

        [SerializeField] private bool isStunned;
        public bool isDefeated;
        public bool isShooting;

        //! Future Field for Combat Manager
        // public bool isTargetFound;
        // public bool readyToFire;

        public enum StateMachine {
            Idle, Chase, Fire, Stun, Dead
        } public StateMachine rangerState;

        //! ==================================================
        private void Awake(){
            targetInfo = GameObject.FindGameObjectWithTag("Player");
            targetL = targetInfo.transform.Find("FarLPosition");
            targetR = targetInfo.transform.Find("FarRPosition");
            tMovement = targetInfo.GetComponent<PlayerMovement>();

            isDefeated = isShooting = isStunned = false;
            //rangerState = StateMachine.Idle;
            rangerState = StateMachine.Fire;
        }

        private void Update() {
            if (isDefeated || isStunned) {
                countDown1 += Time.deltaTime;
            }

            if (isShooting) {
                countDown2 += Time.deltaTime;
            }

            switch (rangerState) {
                case StateMachine.Idle:
                    Idle();
                    break;
                case StateMachine.Chase:
                    FacePlayer();
                    ChaseTarget();
                    break;
                case StateMachine.Fire:
                    isShooting = true;
                    Fire();
                    break;
                case StateMachine.Stun:
                    isStunned = true;
                    Stun();
                    break;
                case StateMachine.Dead:
                    c2D.enabled = false;
                    isDefeated = true;
                    Dead();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        //! ==================================================
        //! Method Field: State Event for Behaviour
        private void Idle() {
            Vector2 targetDistance = transform.position - targetInfo.transform.position;
            if (targetDistance.sqrMagnitude < searchRadius * searchRadius) {
                rangerState = StateMachine.Chase;
            }
        }

        private void ChaseTarget() {
            if (InFireRange(10)) {
                rangerState = StateMachine.Fire;
            }
            else {
                transform.position = faceRight switch {
                    true => Vector2.MoveTowards(transform.position,
                        !tMovement.facingRight ? targetL.position : targetR.position, mv * Time.deltaTime),
                    false => Vector2.MoveTowards(transform.position,
                        !tMovement.facingRight ? targetR.position : targetL.position, mv * Time.deltaTime)
                };
            }
        }

        private void Fire() {
            if (countDown2 < preFire) return;
            Instantiate(bullet, launchOffset.position, transform.rotation);
            isShooting = false;
            countDown2 = 0;
            rangerState = StateMachine.Chase;
        }

        private void Stun() {
            //Animation on
            if (countDown1 <= stunTimer) return;
            //Animation Off
            rangerState = StateMachine.Chase;
            isStunned = false;
            countDown1 = 0;
        }

        private void Dead() {
            if (countDown1 <= perishTimer) return;
            Destroy(gameObject);
        }

        //! ==================================================
        //! Method Field 2
        private bool InFireRange(int distance) {
            var startPosition = castPoint.position;
            castDistance = distance;
            isCheck = false;

            if (!faceRight) {
                castDistance = -distance;
            }

            Vector2 endPos = startPosition + Vector3.right * distance;
            RaycastHit2D hit = Physics2D.Linecast(startPosition, endPos, 1 << LayerMask.NameToLayer("Zone"));
            if (hit.collider != null) {
                isCheck = hit.collider.gameObject.CompareTag("Player");
            }

            return isCheck;
        }

        private void FacePlayer() {
            var localOffset = transform.position.x - targetInfo.transform.position.x;
            switch (localOffset) {
                case < 0 when !faceRight:
                    faceRight = !faceRight;
                    transform.Rotate(0f, 180f, 0f);
                    break;
                case > 0 when faceRight:
                    faceRight = !faceRight;
                    transform.Rotate(0f, 180f, 0f);
                    break;
            }
        }

        public void DamageReceived(int damage) {
            currentHealth -= damage;
            //Update UI Health Bar
            rangerState = currentHealth > 0 ? StateMachine.Stun : StateMachine.Dead;
        }
    }
}
