using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss3 : MonoBehaviour {
    #region Variable Setup
    //! Search Player's Transform and Script
    private GameObject _target;
    private Transform _pFront;
    private Transform _pBehind;
    private PlayerMovement _targetMv;

    //! Connect with Health Bar and Animator (Visual Effects)
    public HealthBarUI boss3Hb;
    public Animator boss3Animation;
    public Rigidbody2D rb2d;

    //! Health and Mobility
    [SerializeField] private float mobility = 3f;
    [HideInInspector] public int currentHealth = 100;

    //! Normal Attack
    public bool missAttack;
    public int targetHit;
    
    //! Timer
    private float _sMoveTimer = 1f;
    private float _countDown;
    
    //! Straight Kick
    private Vector2 _kickCoordinate;
    private bool _xKickActive;
    private bool _readOnce;
    private int _kickNum;

    //! Inch Punch
    private bool _inchActivate;
    private float _inchDash = 30;
    
    //! Raycast for Attack
    [SerializeField] private Transform castPoint;
    [HideInInspector] public float castDistance;
    [HideInInspector] public bool isDetected;
    public float adjDist = 1.5f;

    private bool _isFacingRight;
    public bool isDead;
    
    #endregion
    
    #region StateMachine, Animation
    public enum StateMachine3 {
        Follow, Attack, StraightKick, InchPunch, Defeat, Stun
    } public StateMachine3 boss3States;
    
    #endregion
    
    #region Awake, Start, Update
    private void Awake(){
        _target = GameObject.FindGameObjectWithTag("Player");
        _pFront = _target.transform.Find("RightTrigger");
        _pBehind = _target.transform.Find("LeftTrigger");
        _targetMv = _target.GetComponent<PlayerMovement>();
    }

    private void Update(){
        if (_xKickActive) {
            _countDown += Time.deltaTime;
        }

        switch (boss3States){
            case StateMachine3.Follow: ChangeDirection(); Follow(); break;
            case StateMachine3.Attack: Attack(); break; 
            case StateMachine3.InchPunch: _inchActivate = true; InchPunch(); break;
            case StateMachine3.StraightKick: _xKickActive = true; StraightKick(); break;
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
            var selectSpecMove = Random.Range(0, 5);
            switch (selectSpecMove){
                case <= 1 when currentHealth <= 80: boss3States = StateMachine3.StraightKick; break;
                case 2 when currentHealth <= 50: boss3States = StateMachine3.InchPunch; break;
                default: boss3States = StateMachine3.Attack; break;
            }
            return;
        }
        
        if (!InRange(adjDist)){
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
            true => dist, false => dist
        };

        Vector2 endPos = sPosition + Vector3.right * castDistance;
        var check = Physics2D.Linecast(sPosition, endPos, 1 << LayerMask.NameToLayer("Players"));
        if (check.collider != null){
            isDetected = check.collider.gameObject.CompareTag("Player");
        }
        
        return isDetected;
    }
    
    private void Attack(){
        if (!missAttack && targetHit != 1){
            print("Punch");
            //! anim.SetTrigger(Attack);
        } else {
            boss3States = StateMachine3.Follow;
            missAttack = false;
            targetHit = 0;
        }
    }
    #endregion

    #region Special Move 1 & 2
    private void StraightKick() {
        Vector2 tar = _target.transform.position;
        Vector2 cur = transform.position;

        if (_countDown <= _sMoveTimer) return;
        if (!_readOnce && _kickNum != 3){
            _kickCoordinate = new Vector2(tar.x, tar.y);
            ChangeDirection();
            _readOnce = true;
        }
        
        if (_kickNum != 3){
            print("Oh Boi");
            transform.position = Vector2.MoveTowards(cur, _kickCoordinate, 6 * (mobility * Time.deltaTime));
            if (cur != _kickCoordinate) return;
            _kickNum += 1;
            _countDown = 0;
            _readOnce = false;
        } else {
            StartCoroutine(Tired());
        }
    }
    
    private void InchPunch(){
        if (_inchActivate){
            //! Play Animation
            print("InchPunch");
            rb2d.velocity = _isFacingRight switch{
                true => Vector2.right * _inchDash,
                false => Vector2.left * _inchDash
            };
            _inchActivate = false;
        }

        StartCoroutine(Tired());
    }

    private IEnumerator Tired(){
        //! anim.ResetTrigger(kick);
        //!anim.ResetTrigger(Tired);
        yield return new WaitForSeconds(2f);
        _countDown = 0;
        _readOnce = _xKickActive = false;
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
        //ResetTrigger(Attack);

        currentHealth -= dmg;
        //SetHealth
        boss3States = currentHealth <= 0 ? StateMachine3.Defeat : StateMachine3.Stun;
    }

    private IEnumerator Hit(){
        yield return new WaitForSeconds(1.5f);
        boss3States = StateMachine3.Follow;
    }

    private IEnumerator Vanish(){
        isDead = true;
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
    #endregion
}
