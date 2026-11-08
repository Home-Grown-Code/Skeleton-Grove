using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    protected Collider2D col;
    protected SpriteRenderer sr;

    [Header("Health")]
    [SerializeField] private int maxHeath = 1;
    [SerializeField] private int currentHealth;
    [SerializeField] private Material damageMaterial;
    [SerializeField] private float damageFeedbackDuration = 0.2f;
    private Coroutine damageFeedbackCoroutine;

    [Header("Attack Detaiols")]
    [SerializeField] protected float attackRadius;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask WhatIsTarget;


    [SerializeField] protected float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 12;
    protected float direction = 0f;
    protected int isFacingDirection = 1;

    private float xInput;
    protected bool facingRight = true;
    protected bool canMove = true;
    private bool canJump = true;
    protected bool isAttacking = false;

    [Header("Collision details")]
    [SerializeField] private float groundCheckDisance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();

        currentHealth = maxHeath;
    }

    protected virtual void Update()
    {
        HandleCollision();
        HandleInput();
        HandleMovement();
        HandleAnimations();
        HandleFlip();
    }

    public void DamageTargets()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, WhatIsTarget);

        foreach (Collider2D enemy in enemyColliders)
        {
            Entity entityTarget = enemy.GetComponent<Entity>();
            entityTarget.TakeDamage();
        }
    }

    private void TakeDamage()
    {
        currentHealth = currentHealth - 1;

        RunDamageFeedback();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void RunDamageFeedback()
    {
        if (damageFeedbackCoroutine != null)
            StopCoroutine(DamageFeedbackCor());

        StartCoroutine(DamageFeedbackCor());
    }

    private IEnumerator DamageFeedbackCor()
    {
        Material origenalMat = sr.material;

        sr.material = damageMaterial;

        yield return new WaitForSeconds(damageFeedbackDuration);

        sr.material = origenalMat;
    }

    protected virtual void Die()
    {
        anim.enabled = false;
        col.enabled = false;

        rb.gravityScale = 12;
        rb.linearVelocity = new Vector2(rb.linearVelocityX, 15);

        UI.instance.EnableGameOverUI();
    }

    public void EnableJumpAndMovement(bool enable)
    {
        canJump = enable;
        canMove = enable;
    }

    protected virtual void HandleAnimations()
    {
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("xDirection", direction);
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetMouseButtonDown(0))
        {
            HandleAttack();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            TryToJump();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            TryToJump();
        }

        else if (Input.GetKeyDown(KeyCode.Space))
        {
            TryToJump();
        }
    }

    protected virtual void HandleMovement()
    {
        if (canMove == true)
        {
            rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
        }
    }

    protected virtual void HandleFlip()
    {
        if (!canMove)
        {
            direction = 0;
            return;
        }

        bool leftPressed =
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.LeftArrow);

        bool rightPressed =
            Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.RightArrow);

        if (leftPressed && rightPressed)
        {
            direction = 0;
        }
        else if (leftPressed)
        {
            direction = -1;
        }
        else if (rightPressed)
        {
            direction = 1;
        }
        else
        {
            direction = 0;
        }

        if (direction < 0 && facingRight)
        {
            flip();
        }
        else if (direction > 0 && !facingRight)
        {
            flip();
        }
    }

    protected virtual void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDisance, whatIsGround);
    }

    protected virtual void HandleAttack()
    {
        if (isGrounded && !isAttacking)
        {
            isAttacking = true;
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            EnableJumpAndMovement(false);
            anim.SetTrigger("attack");
        }
    }

    public void IsAttackingStop()
    {
        isAttacking = false;
    }

    private void TryToJump()
    {
        if (isGrounded && canJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    public void flip()
    {
        if (!isAttacking)
        {
            transform.Rotate(0, 180, 0);
            facingRight = !facingRight;
            isFacingDirection = isFacingDirection * -1;

        }

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDisance));
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}