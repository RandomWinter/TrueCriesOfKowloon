using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Attack1();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            Attack2();
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
