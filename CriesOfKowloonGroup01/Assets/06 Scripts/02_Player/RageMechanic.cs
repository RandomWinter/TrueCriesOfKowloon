using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageMechanic : MonoBehaviour
{
    public Transform AttackPoint;
    public LayerMask enemyLayer;
    public LayerMask bossLayer;

    public float attackRange = 0.5f;

    public float attackRate = 2f;
    float nextAttackTime = 0f;

    public float currentRageXP;
    public float maxRageXP = 100;
    public bool IsRaging;
    public RageBarUI ragebar;

    private void Start()
    {
        ragebar.SetStartRage();
    }

    void Update()
    {
        CheckForRaging();

        if(Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Attack1();
                nextAttackTime = Time.time + 1f / attackRate;
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                Attack2();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        if(currentRageXP > maxRageXP)
        {
            currentRageXP = maxRageXP;
            ragebar.SetRage(currentRageXP);
        }
    }

    void Attack1()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemyLayer);
        Collider2D[] hitBoss = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, bossLayer);

        foreach(Collider2D enemy in hitEnemies)
        {
            if(!IsRaging)
            {
                currentRageXP += 5;
                ragebar.SetRage(currentRageXP);
            }
        }

        foreach (Collider2D boss in hitBoss)
        {
            if(!IsRaging)
            {
                currentRageXP += 5;
                ragebar.SetRage(currentRageXP);
            }
        }
    }

    void Attack2()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemyLayer);
        Collider2D[] hitBoss = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, bossLayer);

        foreach(Collider2D enemy in hitEnemies)
        {
            if(!IsRaging)
            {
                currentRageXP += 5;
                ragebar.SetRage(currentRageXP);
            }
        }

        foreach (Collider2D boss in hitBoss)
        {
            if(!IsRaging)
            {
                currentRageXP += 5;
                ragebar.SetRage(currentRageXP);
            }
        }
    }

    public void CheckForRaging()
    {
        if(IsRaging)
        {
            Raging();
        }
        
        if(currentRageXP >= 100)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                GetComponent<PlayerCombat>().lightDamage += 5;
                GetComponent<PlayerCombat>().heavyDamage += 5;
                GetComponent<PlayerMovement>().moveSpeed += 5;
                IsRaging = !IsRaging;

                if(!IsRaging)
                {
                    GetComponent<PlayerCombat>().lightDamage -= 5;
                    GetComponent<PlayerCombat>().heavyDamage -= 5;
                    GetComponent<PlayerMovement>().moveSpeed -= 5;
                }
            }
        }

        if(IsRaging && currentRageXP == 0)
        {
            IsRaging = false;
            GetComponent<PlayerCombat>().lightDamage -= 5;
            GetComponent<PlayerCombat>().heavyDamage -= 5;
            GetComponent<PlayerMovement>().moveSpeed -= 5;
        }
    }

    public void Raging()
    {
        currentRageXP -= Time.deltaTime * 8;
        ragebar.SetRage(currentRageXP);

        if(currentRageXP < 0)
        {
            currentRageXP = 0;
            ragebar.SetStartRage();
        }
    }
}
