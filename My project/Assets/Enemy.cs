using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator animator;
    public float health = 6;
    Rigidbody2D rb;

    public ContactFilter2D movementFilter;
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;

    public float maxSpeed = 5f;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    SpriteRenderer spriteRenderer;

    bool canMove = true;
    private float timeLeft;
    private Vector2 movement;
    public float accelerationTime = 2f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            timeLeft += accelerationTime;
        }
    }

    private void FixedUpdate()
    {

        // If movement input is not 0, try to move
        if (movement != Vector2.zero)
        {

            bool success = TryMove(Vector2.zero);

            if (!success)
            {
                success = TryMove(new Vector2(movement.x, 0));
            }

            if (!success)
            {
                success = TryMove(new Vector2(0, movement.y));
            }

            animator.SetBool("isMoving", success);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        // Set direction of sprite to movement direction
        if (movement.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movement.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private Vector2 RandomUnitVector()
    {
        float random1 = Random.Range(-1f, 1f);
        float random2 = Random.Range(-1f, 1f);
        return new Vector2(0f, random2);
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            // Check for potential collisions
            int count = rb.Cast(
                direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
                movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
                castCollisions, // List of collisions to store the found collisions into after the Cast is finished
                moveSpeed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset

            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            // Can't move if there's no direction to move in
            return false;
        }

    }
    public void TakeDamage(float damage)
    {
        print("ow");
        Defeated();
        health -= damage;

        if (health <= 0)
        {
            Defeated();
        }

    }
    public void Defeated()
    {
        animator.SetTrigger("Defeated");
        //Destroy(gameObject);
    }

    public void RemoveEnemy()
    {
        Destroy(gameObject);
    }
}
