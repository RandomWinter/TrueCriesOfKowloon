using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RageMechanic : MonoBehaviour
{
    public Transform AttackPoint;
    public LayerMask enemyLayer;
    public LayerMask enemy2Layer;
    public LayerMask bossLayer;
    public LayerMask bossLayer2;

    public float attackRange = 0.5f;

    public float attackRate = 2f;
    float nextAttackTime = 0f;

    public float currentRageXP;
    public float maxRageXP = 100;
    public bool IsRaging;
    public RageBarUI ragebar;
    public Image rageFill;
    public Image rageFill2;
    public Image rageFill3;
    public GameObject rageMode;

    public Animator animator;

    private void Start()
    {
        ragebar.SetStartRage();
        rageFill.fillAmount = 0;
        rageFill2.fillAmount = 0;
        rageFill3.fillAmount = 0;
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
           //ragebar.SetRage(currentRageXP);
            rageFill.fillAmount = currentRageXP/maxRageXP;
            rageFill2.fillAmount = currentRageXP/maxRageXP;
            rageFill3.fillAmount = currentRageXP/maxRageXP;
        }
    }

    void Attack1()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemyLayer);
        Collider2D[] hitEnemies2 = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemy2Layer);
        Collider2D[] hitBoss = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, bossLayer);
        Collider2D[] hitBoss2 = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, bossLayer2);

        foreach(Collider2D enemy in hitEnemies)
        {
            if(!IsRaging)
            {
                currentRageXP += 5;
                //ragebar.SetRage(currentRageXP);
                rageFill.fillAmount = currentRageXP / maxRageXP;
                rageFill2.fillAmount = currentRageXP / maxRageXP;
                rageFill3.fillAmount = currentRageXP / maxRageXP;
            }
        }

        foreach (Collider2D enemy in hitEnemies2)
        {
            if (!IsRaging)
            {
                currentRageXP += 5;
               // ragebar.SetRage(currentRageXP);
                rageFill.fillAmount = currentRageXP / maxRageXP;
                //rageFill2.fillAmount = currentRageXP / maxRageXP;
                //rageFill3.fillAmount = currentRageXP / maxRageXP;
            }
        }

        foreach (Collider2D boss in hitBoss)
        {
            if(!IsRaging)
            {
                currentRageXP += 5;
               // ragebar.SetRage(currentRageXP);
                rageFill.fillAmount = currentRageXP / maxRageXP;
                rageFill2.fillAmount = currentRageXP / maxRageXP;
                rageFill3.fillAmount = currentRageXP / maxRageXP;
            }
        }

        foreach (Collider2D boss in hitBoss2)
        {
            if (!IsRaging)
            {
                currentRageXP += 5;
               // ragebar.SetRage(currentRageXP);
                rageFill.fillAmount = currentRageXP / maxRageXP;
                rageFill2.fillAmount = currentRageXP / maxRageXP;
                rageFill3.fillAmount = currentRageXP / maxRageXP;
            }
        }
    }

    void Attack2()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemyLayer);
        Collider2D[] hitEnemies2 = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemy2Layer);
        Collider2D[] hitBoss = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, bossLayer);

        foreach(Collider2D enemy in hitEnemies)
        {
            if(!IsRaging)
            {
                currentRageXP += 5;
                //ragebar.SetRage(currentRageXP);
                rageFill.fillAmount = currentRageXP / maxRageXP;
                rageFill2.fillAmount = currentRageXP / maxRageXP;
                rageFill3.fillAmount = currentRageXP / maxRageXP;
            }
        }

        foreach (Collider2D enemy in hitEnemies2)
        {
            if (!IsRaging)
            {
                currentRageXP += 5;
                //ragebar.SetRage(currentRageXP);
                rageFill.fillAmount = currentRageXP / maxRageXP;
                rageFill2.fillAmount = currentRageXP / maxRageXP;
                rageFill3.fillAmount = currentRageXP / maxRageXP;
            }
        }

        foreach (Collider2D boss in hitBoss)
        {
            if(!IsRaging)
            {
                currentRageXP += 5;
                //ragebar.SetRage(currentRageXP);
                rageFill.fillAmount = currentRageXP / maxRageXP;
                rageFill2.fillAmount = currentRageXP / maxRageXP;
                rageFill3.fillAmount = currentRageXP / maxRageXP;
            }
        }
    }

    public void CheckForRaging()
    {
        if(IsRaging)
        {
            Raging();
            rageMode.SetActive(true);
        }
        
        if(currentRageXP >= 100)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                
                animator.SetTrigger("Rage");
                SoundManager.PlaySound("Rage");
                GetComponent<PlayerCombat>().lightDamage += 5;
                GetComponent<PlayerCombat>().heavyDamage += 5;
                GetComponent<PlayerMovement>().moveSpeed += 5;
                IsRaging = !IsRaging;

                if(!IsRaging)
                {
                    GetComponent<PlayerCombat>().lightDamage -= 5;
                    GetComponent<PlayerCombat>().heavyDamage -= 5;
                    GetComponent<PlayerMovement>().moveSpeed = 5;
                }
            }
        }

        if(IsRaging && currentRageXP == 0)
        {
            rageMode.SetActive(false);
            IsRaging = false;
            GetComponent<PlayerCombat>().lightDamage -= 5;
            GetComponent<PlayerCombat>().heavyDamage -= 5;
            GetComponent<PlayerMovement>().moveSpeed = 5;
        }
    }

    public void Raging()
    {
        currentRageXP -= Time.deltaTime * 8;
        ragebar.SetRage(currentRageXP);
        rageFill.fillAmount = currentRageXP / maxRageXP;
        rageFill2.fillAmount = currentRageXP / maxRageXP;
        rageFill3.fillAmount = currentRageXP / maxRageXP;

        if (currentRageXP < 0)
        {
            currentRageXP = 0;
            ragebar.SetStartRage();
            rageFill.fillAmount =  0;
            rageFill2.fillAmount =  0;
            rageFill3.fillAmount =  0;
        }
    }
}
