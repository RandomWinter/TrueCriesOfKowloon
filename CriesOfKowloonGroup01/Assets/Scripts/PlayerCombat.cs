using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public float AttackL;
    public float AttackH;
    private float NextAttack;
    

    void Update()
    {

        
        if (Input.GetKeyDown(KeyCode.K) && Time.time > NextAttack)
        {
            Debug.Log("light attack");
            NextAttack = Time.time + AttackL;
            Attack1();
        }
        else if (Input.GetKeyDown(KeyCode.L) && Time.time > NextAttack)
        {
            Debug.Log("heavy attack");
            NextAttack = Time.time + AttackH;
            Attack2();

        }
        else
        {
            return;
        }

        



    }

    void Attack1()
    {
        animator.SetTrigger("Attack");
    }

    void Attack2()
    {
        animator.SetTrigger("Attack2");
    }
}
