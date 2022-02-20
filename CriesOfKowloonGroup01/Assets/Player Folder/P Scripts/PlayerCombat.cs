using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public float AttackL;
    public float AttackH;
    private float NextAttack;
    public float attackRange = 0.5f;
    public Transform AttackPoint;
    public LayerMask enemyLayer;
    public int attackDamage = 2;

    void Update()
    {

        
        if (Input.GetKeyDown(KeyCode.K) && Time.time > NextAttack)
        {
            Debug.Log("light attack");
            NextAttack = Time.time + AttackL;
            Attack1();
            Debug.Log(NextAttack);

        }
        else if (Input.GetKeyDown(KeyCode.L) && Time.time > NextAttack)
        {
            Debug.Log("heavy attack");
            NextAttack = Time.time + AttackH;
            Attack2();
            Debug.Log(NextAttack);

        }
        else
        {
            return;
        }

    }

    void Attack1()
    {
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position,attackRange,enemyLayer);

        foreach(Collider2D enemy in hitEnemies)
        {
            // Debug.Log("Enemy hit");
            enemy.GetComponent<EnemyBehavior>().ReceiveDamage(2);
        }
    }

    void Attack2()
    {
        animator.SetTrigger("Attack2");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            // Debug.Log("Enemy hit");
            enemy.GetComponent<EnemyBehavior>().ReceiveDamage(2);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(AttackPoint.position, attackRange);
    }
}
