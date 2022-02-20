using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Components Field")]
    [SerializeField] private Transform playerInfo;
    [SerializeField] private Transform leftPosition;
    [SerializeField] private Transform rightPosition;
    private NavMeshAgent _agent;

    [Header("Visual Component")]
    //[SerializeField] private Rigidbody2D rb;
    [SerializeField] public HealthBarUI hb;
    [SerializeField] public Animator anim;

    //Wander Setup
    //private Transform target;
    [SerializeField] public float wanderRadius;
    [SerializeField] public float wanderTimer;
    [SerializeField] public float coolDownTimer;
    [SerializeField] private float countTimer;
    [SerializeField] private float stunTimer;

    //Fields
    [Header("Classic Stats")]
    [SerializeField] private float searchRadius = 8.5f;
    [SerializeField] private float attRad = 1.25f;

    [SerializeField] private int baseHealth;
    [SerializeField] private int maxHealth = 20;
    // [SerializeField] private int damage = 2;
    // [HideInInspector] 
    public bool sawPlayer;
    public bool prepareForAttack;
    
    public bool missAttack;
    public bool faceRight;
    public bool isDead;

    public enum StateMachine{
        Idle,
        Chase,
        Attack,
        Stun
    }
    
    [Header("StateMachine Setup")]
    [SerializeField] public StateMachine currentState;
    [SerializeField] public GameObject playerReference;
    //randInt = Mathf.RoundToInt(Random.Range(0,5));

    public void Awake(){
        //CombatManager.AddToList(this);
        //One for player position
        playerReference = GameObject.FindGameObjectWithTag("Player");
        playerInfo = playerReference.transform;
        leftPosition = playerInfo.transform.Find("LeftTrigger");
        rightPosition = playerInfo.transform.Find("RightTrigger");
        
        //Get Animator componenet
        anim = GetComponent<Animator>();
        
        //Setup AI Function
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        //Health Setup
        baseHealth = maxHealth;
        hb.SetMaxHealth(maxHealth);
        
        //Wander time
        countTimer = wanderTimer;
        
        //Set initial Boolean
        sawPlayer = false;
        missAttack = false;
        prepareForAttack = false;
        isDead = false;
        
        currentState = StateMachine.Idle;
    }

    public void Update()
    {
        //Wander
        countTimer += Time.deltaTime;
        stunTimer += Time.deltaTime;
        
        switch(currentState){
            case StateMachine.Idle:
                Flip();
                Idle();
                break;
            case StateMachine.Chase:
                Flip();
                Chasing();
                break;
            case StateMachine.Attack:
                AttackPlayer();
                break;
            case StateMachine.Stun:
                Stunning();
                break;
        }
    }

    //Flip based on Player position
    private void Flip(){
        var localOffset = transform.localPosition.x - playerInfo.transform.localPosition.x;
        if(localOffset < 0 && !faceRight){
            faceRight = !faceRight;
            transform.Rotate(0, 180f, 0);
        } else if(localOffset > 0 && faceRight){
            faceRight = !faceRight;
            transform.Rotate(0, 180f, 0);
        }

    }

    protected virtual void Idle(){
        Vector2 radar = transform.position - playerInfo.position;
        if(radar.sqrMagnitude < searchRadius * searchRadius){
            currentState = StateMachine.Chase;
            sawPlayer = true;
        }
    }

    protected virtual void Chasing() {
        anim.SetBool("isMoving", true);
        Vector2 attackRadar = (transform.position - playerInfo.position);
        if (attackRadar.sqrMagnitude < attRad * attRad) {
            currentState = StateMachine.Attack;
        }

        if (!prepareForAttack) {
            if (countTimer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                _agent.SetDestination(newPos);
                countTimer = 0;
            }
        } else {
            if(faceRight) {
                if (!playerReference.GetComponent<PlayerMovement>().facingRight){
                    _agent.SetDestination(rightPosition.position);
                } else {
                    _agent.SetDestination(leftPosition.position);
                }
            } else {
                if (!playerReference.GetComponent<PlayerMovement>().facingRight){
                    _agent.SetDestination(leftPosition.position);
                } else {
                    _agent.SetDestination(rightPosition.position);
                }
            }
        }
    }

    protected virtual void AttackPlayer(){
        anim.SetBool("isMoving", false);
        
        if(!missAttack){
            anim.SetBool("isAttack", true);
        } else {
            anim.SetBool("isAttack", false);
            anim.SetBool("chainAttack", false);
            currentState = StateMachine.Chase;
            
            //Reset Default
            missAttack = false;
        }
        /*
            GetComponent<Player>().TakeDamage(damage);
        */
    }

    protected virtual void DeathReset() {
        isDead = true;
        GetComponent<Collider2D>().enabled = false;
        //Give Points to score
    }

    protected virtual void Stunning(){
        if (stunTimer >= coolDownTimer){
            anim.SetBool("isHit", false);
            currentState = StateMachine.Chase;
            stunTimer = 0;
        }
    }

    //Wander Method
    private static Vector3 RandomNavSphere(Vector3 origin, float dist, int tempMask) {
        Vector3 aa = Random.insideUnitCircle * dist;
        aa += origin;
        
        NavMeshHit navHit;
        NavMesh.SamplePosition(aa, out navHit, dist, tempMask);
        return navHit.position;
    }

    public virtual void ReceiveDamage(int damage) {
        anim.SetBool("isAttack", false);
        anim.SetBool("isMoving", false);
        
        anim.SetBool("isHit", true);
        currentState = StateMachine.Stun;
        baseHealth -= damage;
        hb.SetHealth(baseHealth);

        if(baseHealth <= 0){
            anim.SetBool("isHit", false);
            anim.SetBool("isDead", true);
            //FindObjectOfType<AudioManager>().Play("EnemyDeath");
            DeathReset();
        }
    }
}
