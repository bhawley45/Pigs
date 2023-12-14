using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField] float runSpeed = 5.5f;
    [SerializeField] float jumpSpeed = 16f;
    [SerializeField] float climbSpeed = 4.5f;
    [SerializeField] Vector2 hitVelocity = new Vector2(20f, 20f);

    [SerializeField] float attackRadius = .86f;
    [SerializeField] Transform hurtBox;
 
    Rigidbody2D playerRigidBody;
    BoxCollider2D playerBoxCollider;
    PolygonCollider2D playerFeetCollider;
    Animator playerAnimator;

    //Original gravity from playerRigidbody
    float startingGravity;
    //Track if hit, used to momentarily disable movement
    bool isHurting;
    //Set delay from hit
    float hurtDelay = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        //Get reference to player rigidbody and animator
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerBoxCollider = GetComponent<BoxCollider2D>();
        playerFeetCollider = GetComponent<PolygonCollider2D>();

        startingGravity = playerRigidBody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHurting)
        {
            Run();
            Jump();
            Climb();
            Atack();

            if (playerBoxCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
            {
                PlayerHit();
            }

            ExitLevel();
        }
    }

    private void ExitLevel()
    {
        if (!playerBoxCollider.IsTouchingLayers(LayerMask.GetMask("Interactable"))) { return; }
        
        var exitDoor = FindObjectOfType<ExitDoor>();

        if (CrossPlatformInputManager.GetButtonDown("Vertical"))
        {
            exitDoor.StartLoadingNextLevel();
        }
    }

    //Coroutine used to delay player movement after taking damage
    IEnumerator StopHurting()
    {
        yield return new WaitForSeconds(hurtDelay);
        isHurting = false;
    }

    private void Atack()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            //Play Attack Animation
            playerAnimator.SetTrigger("Attacking");

            Collider2D[] enemiesWithinRange = Physics2D.OverlapCircleAll(hurtBox.position, attackRadius, LayerMask.GetMask("Enemy"));

            //Hit enemies within the attachRadius
            foreach (var enemy in enemiesWithinRange)
            {
                enemy.GetComponent<Enemy>().Dying();
            }

        }
    }

    public void PlayerHit()
    {
        //Remove a life/Reset Game
        FindAnyObjectByType<GameSession>().ProcessPlayerDeath();
        
        //Shoot the player the opposite direction they were facing at time of collision
        playerRigidBody.velocity = hitVelocity * new Vector2(-transform.localScale.x, 1f);

        //Play hit animation
        playerAnimator.SetTrigger("Hitting");
        isHurting = true;
        StartCoroutine(StopHurting());
    }

    private void Climb()
    {
        if (playerBoxCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
            Vector2 velocity = new Vector2(playerRigidBody.velocity.x, controlThrow * climbSpeed);
            playerRigidBody.velocity = velocity;

            bool isClimbing = Mathf.Abs(playerRigidBody.velocity.y) > Mathf.Epsilon;

            //Remove gravity from player rigidbody to prevent falling
            playerRigidBody.gravityScale = 0f;

            //Play climbing animation if true...
            playerAnimator.SetBool("Climbing", isClimbing);
        }
        else
        {
            //Reset player gravity and animation...
            playerRigidBody.gravityScale = startingGravity;
            playerAnimator.SetBool("Climbing", false);
        }
    }

    private void Jump()
    {
        //Quick return if feet not touching ground layer
        if(!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        bool isJumping = CrossPlatformInputManager.GetButtonDown("Jump");
        if (isJumping)
        {
            Vector2 jumpVelocity = new Vector2(playerRigidBody.velocity.x, jumpSpeed);
            playerRigidBody.velocity = jumpVelocity;
        }
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 velocity = new Vector2(controlThrow * runSpeed, playerRigidBody.velocity.y);
        playerRigidBody.velocity = velocity;

        //.Epsilon is used because the velocity can never be truly 0 from the controller?
        bool runningHorizontally = Mathf.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon;

        //Play running animation if true...
        playerAnimator.SetBool("Running", runningHorizontally);
        FlipSprite(runningHorizontally);
    }

    private void FlipSprite(bool isRunning)
    {
        if(isRunning)
        {
            // Left -> -1 | Right -> 1 (Mathf.Sign(playerRigidBody.velocity.x))
            transform.localScale = new Vector2(Mathf.Sign(playerRigidBody.velocity.x) ,1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Test!");
    }


    //DEBUG for player attack
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(hurtBox.position, attackRadius);
    }
}
