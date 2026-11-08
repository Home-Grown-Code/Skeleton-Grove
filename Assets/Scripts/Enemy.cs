using UnityEditor.Experimental.GraphView;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Windows;

public class Enemy : Entity
{
    private bool PlayerDetected = false;
    protected override void Update()
    {
        HandleCollision();
        HandleMovement();
        HandleAnimations();
        HandleFlip();
        HandleAttack();
    }

    protected override void HandleMovement()
    {
        if (canMove == true)
        {
            rb.linearVelocity = new Vector2(isFacingDirection * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    protected override void HandleFlip()
    {
        if (!canMove)
        {
            direction = 0;
            return;
        }
        else if (rb.linearVelocity.x < 0)
        {
            isFacingDirection = -1;
            direction = -1;

        }
        else if (rb.linearVelocity.x > 0)
        {
            isFacingDirection = 1;
            direction = 1;
        }
        else
        {
            isFacingDirection = 0;
            direction = 0;
        }
    }

    protected override void HandleAnimations()
    {
        anim.SetFloat("xDirection", direction);
    }

    protected override void HandleAttack()
    {
        if (PlayerDetected)
            anim.SetTrigger("attack");
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();
        PlayerDetected = Physics2D.OverlapCircle(attackPoint.position, attackRadius, WhatIsTarget);
    }

    protected override void Die()
    {
        anim.enabled = false;
        col.enabled = false;

        rb.gravityScale = 12;
        rb.linearVelocity = new Vector2(rb.linearVelocityX, 15);

        UI.instance.AddKillCount();
    }
}