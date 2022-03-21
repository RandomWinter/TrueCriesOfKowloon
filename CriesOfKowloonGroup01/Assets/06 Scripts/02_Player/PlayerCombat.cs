using System.Collections;
using System.Collections.Generic;
using _06_Scripts._03_Props;
using _06_Scripts._04_Enemy;
using _06_Scripts._05_Boss;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    //public variables
    public float AttackL;
    public float AttackH;
    public float attackCD;
    private float NextAttack;
    public float attackRange = 0.5f;
    public int attackDamage;
    public  float knockBack;
    public  float knockForce;
    public float cancelCombo = 3f;

    public int lightDamage = 2;
    public int heavyDamage = 5;
    public int lightCount;
    public int heavyCount;

    public Animator animator;

    public Transform AttackPoint;
    public LayerMask enemyLayer;
    public LayerMask propLayer;
    public GameObject Player;

    void Start()
    {
        lightCount = heavyCount = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && Time.time > NextAttack)
        {
            
            Debug.Log("light attack");
            NextAttack = Time.time + AttackL;
            Attack1();
            Debug.Log(NextAttack);
            lightCount++;

            switch (lightCount)
            {
                case 1:
                    if (heavyCount == 2)
                    {
                        Debug.Log("Dragon Sweep");
                        NextAttack = Time.time + AttackH;
                        ResetAttackCount();
                    }
                    break;
                case 2:
                    animator.SetTrigger("Attack2");
                    Debug.Log("light attack");
                    NextAttack = Time.time + AttackL;
                    Attack1();
                    break;
                case 3:
                    animator.SetTrigger("FlashFist");
                    Debug.Log("Flash Fist");
                    NextAttack = Time.time + 2;
                    FlashFist();
                    Debug.Log(NextAttack);
                    ResetAttackCount();
                    lightCount = 0;
                    heavyCount = 0;
                    break;

                default:
                    ResetAttackCount();
                    break;

            }
        }
        else if (Input.GetKeyDown(KeyCode.L) && Time.time > NextAttack)
        {
            Debug.Log("heavy attack");
            NextAttack = Time.time + AttackH;
            Attack2();
            Debug.Log(NextAttack);
            heavyCount++;

            switch (heavyCount)
            {
                case 1:
                    if (lightCount == 2)
                    {
                        animator.SetTrigger("UpstreamPunch");
                        Debug.Log("Upstream Punch");
                        NextAttack = Time.time + 2;
                        UpstreamPunch();
                        ResetAttackCount();
                        lightCount = 0;
                        heavyCount = 0;
                    }
                    break;
                case 2:
                    if (lightCount == 1)
                    {
                        Debug.Log("Rock Breaker");
                        ResetAttackCount();

                    }
                    else if (lightCount == 2)
                    {
                        Debug.Log("Emotional Damage");
                        ResetAttackCount();
                    }
                    else
                    {
                        animator.SetTrigger("HeavyAttack2");
                        NextAttack = Time.time + AttackH;
                        Attack2();
                        ResetAttackCount();
                    }

                    break;
                default:
                    ResetAttackCount();
                    break;

            }

        }
        else
        {
            return;
        }
    }

    void Attack1()
    {
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemyLayer);
        Collider2D[] hitProps = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, propLayer);
        Collider2D[] hitBoss = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, propLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (Player.GetComponent<PlayerMovement>().facingRight)
            {
                Debug.Log("Enemy hit");
                enemy.GetComponent<EnemyBehavior>().ReceiveDamage(lightDamage);
                enemy.attachedRigidbody.AddForce(new Vector2(knockBack * 5, knockForce * 5));
                print("launch right");
            }
            else
            {
                    Debug.Log("Enemy hit");
                    enemy.GetComponent<EnemyBehavior>().ReceiveDamage(lightDamage);
                    enemy.attachedRigidbody.AddForce(new Vector2(knockBack * -5, knockForce * -5));
                    print("launch left");
            }
            
        }

        foreach (Collider2D Props in hitProps)
        {
            print("Prop hit");
            Props.GetComponent<BreakableProps>().ReceivingDamage(lightDamage);
        }

        foreach (Collider2D boss in hitBoss)
        {
            print("Boss hit");
            boss.GetComponent<AhKom>().DamageReceived(lightDamage);
        }

    }

    void Attack2()
    {
        animator.SetTrigger("HeavyAttack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemyLayer);
        Collider2D[] hitProps = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, propLayer);
        Collider2D[] hitBoss = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, propLayer);


        foreach (Collider2D enemy in hitEnemies)
        {
            if (Player.GetComponent<PlayerMovement>().facingRight)
            {
                // Debug.Log("Enemy hit");
                enemy.GetComponent<EnemyBehavior>().ReceiveDamage(heavyDamage);
                enemy.attachedRigidbody.AddForce(new Vector2(knockBack * 5, knockForce * 5));
                
            }
            else
            {
                // Debug.Log("Enemy hit");
                enemy.GetComponent<EnemyBehavior>().ReceiveDamage(heavyDamage);
                enemy.attachedRigidbody.AddForce(new Vector2(knockBack * -5, knockForce * -5));
            }
        }

        foreach (Collider2D Props in hitProps)
        {
            print("Prop hit");
            Props.GetComponent<BreakableProps>().ReceivingDamage(heavyDamage);
        }

        foreach (Collider2D boss in hitBoss)
        {
            print("Boss hit");
            boss.GetComponent<AhKom>().DamageReceived(lightDamage);
        }

    }

    void FlashFist()
    {
        print("Working");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemyLayer);
        Collider2D[] hitProps = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, propLayer);
        Collider2D[] hitBoss = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, propLayer);


        foreach (Collider2D enemy in hitEnemies)
        {
            if (Player.GetComponent<PlayerMovement>().facingRight)
            {
                // Debug.Log("Enemy hit");
                enemy.GetComponent<EnemyBehavior>().ReceiveDamage(attackDamage + 5);
                enemy.attachedRigidbody.AddForce(new Vector2(knockBack * 5, knockForce * 5));

            }
            else
            {
                // Debug.Log("Enemy hit");
                enemy.GetComponent<EnemyBehavior>().ReceiveDamage(attackDamage + 5);
                enemy.attachedRigidbody.AddForce(new Vector2(knockBack * -5, knockForce * -5));
            }
        }

        foreach (Collider2D Props in hitProps)
        {
            print("Prop hit");
            Props.GetComponent<BreakableProps>().ReceivingDamage(heavyDamage);
        }

        foreach (Collider2D boss in hitBoss)
        {
            print("Boss hit");
            boss.GetComponent<AhKom>().DamageReceived(lightDamage);
        }
        ResetAttackCount();


    }

    void UpstreamPunch()
    {
        animator.SetTrigger("Upstream Punch");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemyLayer);
        Collider2D[] hitProps = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, propLayer);
        Collider2D[] hitBoss = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, propLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (Player.GetComponent<PlayerMovement>().facingRight)
            {
                // Debug.Log("Enemy hit");
                enemy.GetComponent<EnemyBehavior>().ReceiveDamage(attackDamage + 5);
                enemy.attachedRigidbody.AddForce(new Vector2(knockBack * 5, knockForce * 5));

            }
            else
            {
                // Debug.Log("Enemy hit");
                enemy.GetComponent<EnemyBehavior>().ReceiveDamage(attackDamage + 5);
                enemy.attachedRigidbody.AddForce(new Vector2(knockBack * -5, knockForce * -5));
            }
        }

        foreach (Collider2D Props in hitProps)
        {
            print("Prop hit");
            Props.GetComponent<BreakableProps>().ReceivingDamage(heavyDamage);
        }

        foreach (Collider2D boss in hitBoss)
        {
            print("Boss hit");
            boss.GetComponent<AhKom>().DamageReceived(lightDamage);
        }
        ResetAttackCount();
    }

    void ResetAttackCount()
    {
        lightCount = 0;
        heavyCount = 0;
    }
}