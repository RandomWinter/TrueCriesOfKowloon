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
    public  float knockBack;
    public  float knockForce;
    public float cancelCombo;
    public float coolDown = 3f;

    public int attackDamage;
    public int lightDamage = 1;
    public int newLightDamage = 0;
    public int heavyDamage = 2;
    public int newHeavyDamage = 0;
    public int lightCount;
    public int heavyCount;

    public Animator animator;

    public Transform AttackPoint;
    public LayerMask enemyLayer;
    public LayerMask enemy2Layer;
    public LayerMask propLayer;
    public LayerMask bossLayer;
    public LayerMask boss2Layer;
    public LayerMask boss3Layer;
    public GameObject Player;

    

    void Start()
    {
        
        lightCount = heavyCount = 0;
    }

    void Update()
    {
        if (cancelCombo <= Time.time)
        {
            ResetAttackCount();
            //print(lightCount);
            //print(heavyCount);
        }

        // Light attack and combos
        if (Input.GetKeyDown(KeyCode.K) && Time.time > NextAttack)
        {

            Debug.Log("light attack");
            NextAttack = Time.time + AttackL;
            Attack1();
            Debug.Log(NextAttack);
            lightCount++;
            cancelCombo = Time.time + coolDown;

            switch (lightCount)
            {
                case 1:
                    if (heavyCount == 2)
                    {
                        animator.SetTrigger("DragonSweep");
                        Debug.Log("Dragon Sweep");
                        //NextAttack = Time.time + 0.85f;
                        DragonSweep();
                        ResetAttackCount();
                    }
                    break;

                case 2:
                    animator.SetTrigger("Attack2");
                    Debug.Log("light attack");
                    NextAttack = Time.time + AttackL;
                    Attack1();
                    cancelCombo = Time.time + coolDown;
                    break;
                case 3:
                    animator.SetTrigger("FlashFist");
                    Debug.Log("Flash Fist");
                    NextAttack = Time.time + 0.75f;
                    FlashFist();
                    //Debug.Log(NextAttack);
                    ResetAttackCount();
                    break;
                case 4:
                    lightCount = 0;
                    break;

                default:
                    cancelCombo = Time.time + coolDown;
                    break;

            }
        }
        //Heavy attack and combos
        if (Input.GetKeyDown(KeyCode.L) && Time.time > NextAttack)
        {
            Debug.Log("heavy attack");
            NextAttack = Time.time + AttackH;
            Attack2();
            Debug.Log(NextAttack);
            heavyCount++;
            cancelCombo = Time.time + coolDown;

            switch (heavyCount)
            {
                case 1:
                    if (lightCount == 2)
                    {
                        animator.SetTrigger("UpstreamPunch");
                        Debug.Log("Upstream Punch");
                        NextAttack = Time.time + 1.5f;
                        UpstreamPunch();
                        ResetAttackCount();
                    }
                    break;
                case 2:
                    if (lightCount == 1)
                    {
                        animator.SetTrigger("RockBreaker");
                        Debug.Log("Rock Breaker");
                        NextAttack = Time.time + 1.5f;
                        RockBreaker();
                        ResetAttackCount();

                    }
                    else if (lightCount == 2)
                    {
                        animator.SetTrigger("EmotionalDamage");
                        Debug.Log("Emotional Damage");
                        NextAttack = Time.time + 1.5f;
                        EmotionalDamage();
                        ResetAttackCount();
                    }
                    else
                    {
                        animator.SetTrigger("HeavyAttack2");
                        NextAttack = Time.time + AttackH;
                        Attack2();
                        cancelCombo = Time.time + coolDown;

                    }

                    break;
                case 3:
                    heavyCount = 1;
                    break;
                default:
                    cancelCombo = Time.time + coolDown;
                    break;

            }

        }
        
    }

    void Attack1()
    {
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemyLayer);
        Collider2D[] hitEnemies2 = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemy2Layer);
        Collider2D[] hitProps = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, propLayer);
        Collider2D[] hitBoss = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, bossLayer);
        Collider2D[] hitBoss2 = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, boss2Layer);
        Collider2D[] hitBoss3 = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, boss3Layer);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (Player.GetComponent<PlayerMovement>().facingRight)
            {
                Debug.Log("Enemy hit");
                //enemy.GetComponent<EnemyBehavior>().ReceiveDamage(lightDamage);
                enemy.GetComponent<MeleeCombat>().ReceiveDamage(lightDamage);
                enemy.attachedRigidbody.AddForce(new Vector2(50 * 5, 0 * 5));
                Debug.Log(enemy.gameObject.name);
                print("launch right");
                
            }
            else
            {
                Debug.Log("Enemy hit");
                //enemy.GetComponent<EnemyBehavior>().ReceiveDamage(lightDamage);
                enemy.GetComponent<MeleeCombat>().ReceiveDamage(lightDamage);
                enemy.attachedRigidbody.AddForce(new Vector2(50 * -5, 0 * -5));
                enemy.GetComponent<Ranger>().ReceivedDamage(lightDamage);
                print("launch left");
            }

        }

        foreach (Collider2D enemy in hitEnemies2)
        {
            if (Player.GetComponent<PlayerMovement>().facingRight)
            {
                Debug.Log("Enemy hit");
                enemy.GetComponent<Ranger>().ReceivedDamage(lightDamage);
                enemy.attachedRigidbody.AddForce(new Vector2(50 * 5, 0 * 5));
                Debug.Log(enemy.gameObject.name);
                print("launch right");

            }
            else
            {
                Debug.Log("Enemy hit");
                //enemy.GetComponent<EnemyBehavior>().ReceiveDamage(lightDamage);
                enemy.GetComponent<Ranger>().ReceivedDamage(lightDamage);
                enemy.attachedRigidbody.AddForce(new Vector2(50 * -5, 0 * -5));
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
            Debug.Log(boss.gameObject.name);
            //boss.GetComponent<AhKom>().DamageReceived(lightDamage);
            boss.gameObject.GetComponent<AhKom>().DamageReceived(lightDamage);
        }

        foreach (Collider2D boss in hitBoss2)
        {
            print("Boss hit");
            Debug.Log(boss.gameObject.name);
            //boss.GetComponent<AhKom>().DamageReceived(lightDamage);
            boss.gameObject.GetComponent<YuLing>().DamageReceived(lightDamage);
        }

        foreach (Collider2D boss in hitBoss3)
        {
            print("Boss hit");
            Debug.Log(boss.gameObject.name);
            //boss.GetComponent<AhKom>().DamageReceived(lightDamage);
            boss.gameObject.GetComponent<Boss3>().ReceiveDamage(lightDamage);
        }

    }

    void Attack2()
    {
        animator.SetTrigger("HeavyAttack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemyLayer);
        Collider2D[] hitProps = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, propLayer);
        Collider2D[] hitBoss = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, bossLayer);


        foreach (Collider2D enemy in hitEnemies)
        {
            if (Player.GetComponent<PlayerMovement>().facingRight)
            {
                // Debug.Log("Enemy hit");
                //enemy.GetComponent<EnemyBehavior>().ReceiveDamage(heavyDamage);
                enemy.GetComponent<MeleeCombat>().ReceiveDamage(heavyDamage);
                enemy.attachedRigidbody.AddForce(new Vector2(75 * 5, 0 * 5));
                
            }
            else
            {
                // Debug.Log("Enemy hit");
                //enemy.GetComponent<EnemyBehavior>().ReceiveDamage(heavyDamage);
                enemy.GetComponent<MeleeCombat>().ReceiveDamage(heavyDamage);
                enemy.attachedRigidbody.AddForce(new Vector2(75 * -5, 0 * -5));
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
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemyLayer);
        Collider2D[] hitProps = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, propLayer);
        Collider2D[] hitBoss = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, bossLayer);


        foreach (Collider2D enemy in hitEnemies)
        {
            if (Player.GetComponent<PlayerMovement>().facingRight)
            {
                // Debug.Log("Enemy hit");
                //enemy.GetComponent<EnemyBehavior>().ReceiveDamage(attackDamage + 5);
                enemy.GetComponent<MeleeCombat>().ReceiveDamage(attackDamage + 2);
                enemy.attachedRigidbody.AddForce(new Vector2(90 * 5, 0 * 5));

            }
            else
            {
                // Debug.Log("Enemy hit");
                //enemy.GetComponent<EnemyBehavior>().ReceiveDamage(attackDamage + 5);
                enemy.GetComponent<MeleeCombat>().ReceiveDamage(attackDamage + 2);
                enemy.attachedRigidbody.AddForce(new Vector2(90 * -5, 0 * -5));
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
        animator.SetTrigger("UpstreamPunch");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemyLayer);
        Collider2D[] hitProps = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, propLayer);
        Collider2D[] hitBoss = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, bossLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (Player.GetComponent<PlayerMovement>().facingRight)
            {
                // Debug.Log("Enemy hit");
                //enemy.GetComponent<EnemyBehavior>().ReceiveDamage(attackDamage + 5);
                enemy.GetComponent<MeleeCombat>().ReceiveDamage(attackDamage + 5);
                enemy.attachedRigidbody.AddForce(new Vector2(50 * 5, 100 * 5));

            }
            else
            {
                // Debug.Log("Enemy hit");
                //enemy.GetComponent<EnemyBehavior>().ReceiveDamage(attackDamage + 5);
                enemy.GetComponent<MeleeCombat>().ReceiveDamage(attackDamage + 5);
                enemy.attachedRigidbody.AddForce(new Vector2(0 * 5, -100 * 5));
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

    void RockBreaker()
    {
        animator.SetTrigger("RockBreaker");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemyLayer);
        Collider2D[] hitProps = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, propLayer);
        Collider2D[] hitBoss = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, bossLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (Player.GetComponent<PlayerMovement>().facingRight)
            {
                // Debug.Log("Enemy hit");
                //enemy.GetComponent<EnemyBehavior>().ReceiveDamage(attackDamage + 5);
                enemy.GetComponent<MeleeCombat>().ReceiveDamage(attackDamage + 5);
                enemy.attachedRigidbody.AddForce(new Vector2(50 * 5, 100 * 5));

            }
            else
            {
                // Debug.Log("Enemy hit");
                //enemy.GetComponent<EnemyBehavior>().ReceiveDamage(attackDamage + 5);
                enemy.GetComponent<MeleeCombat>().ReceiveDamage(attackDamage + 5);
                enemy.attachedRigidbody.AddForce(new Vector2(0 * 5, -100 * 5));
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

    void DragonSweep()
    {
        animator.SetTrigger("DragonSweep");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemyLayer);
        Collider2D[] hitProps = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, propLayer);
        Collider2D[] hitBoss = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, bossLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (Player.GetComponent<PlayerMovement>().facingRight)
            {
                Debug.Log("Enemy hit");
                //enemy.GetComponent<EnemyBehavior>().ReceiveDamage(attackDamage + 5);
                enemy.GetComponent<MeleeCombat>().ReceiveDamage(attackDamage + 2);
                enemy.attachedRigidbody.AddForce(new Vector2(200 * 5, 0 * 5));

            }
            else
            {
                // Debug.Log("Enemy hit");
                //enemy.GetComponent<EnemyBehavior>().ReceiveDamage(attackDamage + 5);
                enemy.GetComponent<MeleeCombat>().ReceiveDamage(attackDamage + 2);
                enemy.attachedRigidbody.AddForce(new Vector2(200 * -5, 0 * 5));
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

    void EmotionalDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemyLayer);
        Collider2D[] hitProps = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, propLayer);
        Collider2D[] hitBoss = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, bossLayer);


        foreach (Collider2D enemy in hitEnemies)
        {
            if (Player.GetComponent<PlayerMovement>().facingRight)
            {
                Debug.Log("Enemy hit");
                //enemy.GetComponent<EnemyBehavior>().ReceiveDamage(attackDamage + 5);
                enemy.GetComponent<MeleeCombat>().ReceiveDamage(attackDamage + 4);
                enemy.attachedRigidbody.AddForce(new Vector2(90 * 5, 0 * 5));

            }
            else
            {
                // Debug.Log("Enemy hit");
                //enemy.GetComponent<EnemyBehavior>().ReceiveDamage(attackDamage + 5);
                enemy.GetComponent<MeleeCombat>().ReceiveDamage(attackDamage + 4);
                enemy.attachedRigidbody.AddForce(new Vector2(90 * -5, 0 * -5));
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPoint.position, attackRange);
    }

    void ResetAttackCount()
    {
        lightCount = 0;
        heavyCount = 0;
    }


}