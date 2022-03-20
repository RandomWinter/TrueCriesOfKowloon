using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float baseSpeed;
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public bool facingRight = true;
    public Animator animator;

    public float dashRate = 2f;
    float nextDashTime = 0f;

    public float dashPower;
    public float dashTime;

    bool isDashing = false;

    Vector2 movement;

    void Start()
    {
        moveSpeed = baseSpeed;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Speed", movement.sqrMagnitude);

        if(movement.x > 0 && !facingRight)
        {
            Flip();
            
        }
        else if(movement.x < 0 && facingRight)
        {
            Flip();
            
        }

        if(Time.time >= nextDashTime)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!isDashing)
                {
                    StartCoroutine(Dash());
                    nextDashTime = Time.time + 2f / dashRate;
                    animator.SetTrigger("Dash");
                }
            }
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    IEnumerator Dash()
    {
        isDashing = true;
        moveSpeed *= dashPower;

        yield return new WaitForSeconds(dashTime);

        moveSpeed = baseSpeed;
        isDashing = false;
    }
}
