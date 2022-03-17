using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    //public variables
    public Animator animator;
    public float AttackL;
    public float AttackH;
    public float attackCD;
    private float NextAttack;
    public float attackRange = 0.5f;
    public Transform AttackPoint;
    public LayerMask enemyLayer;
    public int lightDamage = 2;
    public int heavyDamage = 5;
    public int lightCount;
    public int heavyCount;
    public float knockBack;
    public float knockForce;
    public float cancelCombo;

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
                        ResetAttackCount();
                    }

                    break;
                case 3:
                    Debug.Log("Flash Fist");
                    NextAttack = Time.time + AttackL;
                    Attack2();
                    Debug.Log(NextAttack);
                    ResetAttackCount();
                    break;

                default:
                    //nothing
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
                    if(lightCount == 2)
                    {
                        Debug.Log("Upstream Punch");
                        ResetAttackCount();
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

        foreach (Collider2D enemy in hitEnemies)
        {
            // Debug.Log("Enemy hit");
            enemy.GetComponent<EnemyBehavior>().ReceiveDamage(lightDamage);
            enemy.attachedRigidbody.AddForce(new Vector2(knockBack * 5, knockForce * 5));
        }

        
    }

    void Attack2()
    {
        animator.SetTrigger("Attack2");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            // Debug.Log("Enemy hit");
            enemy.GetComponent<EnemyBehavior>().ReceiveDamage(heavyDamage);
            enemy.attachedRigidbody.AddForce(new Vector2(knockBack * 10, knockForce * 10));
        }

    }

    void comboAttack()
    {
        if(lightCount == 3)
        {
            //Flash fist
            Debug.Log("FLash Fist");
            lightCount = 0;
            heavyCount = 0;
        }
        else if (lightCount == 2 && heavyCount == 1)
        {
            //Upstream punch
            Debug.Log("Upstream Punch");
            lightCount = 0;
            heavyCount = 0;
        }
        else if (lightCount == 1 && heavyCount == 2)
        {
            //Rock Breaker
            Debug.Log("Rock Breaker");
            lightCount = 0;
            heavyCount = 0;
        }
        else if (heavyCount == 2 && lightCount == 1)
        {
            //Dragon Sweep
            Debug.Log("Dragon Sweep");
            lightCount = 0;
            heavyCount = 0;
        }
        else if(lightCount == 2 && heavyCount == 2)
        {
            //Emotional Damage
            Debug.Log("Emotional Damage");
            lightCount = 0;
            heavyCount = 0;
        }
        else
        {
            //Nothing
            //lightCount = 0;
            //heavyCount = 0;
        }
    }

    void ResetAttackCount()
    {
        lightCount = 0;
        heavyCount = 0;
    }
}
