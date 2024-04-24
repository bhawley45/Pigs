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
    [SerializeField] float secToWait = .5f;

    [SerializeField] AudioClip jumpingSFX, attackingSFX, deathSFX, gettingHitSFX, walkingSFX;

    [SerializeField] float attackRadius = .86f;
    [SerializeField] Transform hurtBox;
 
    Rigidbody2D playerRigidBody;
    BoxCollider2D playerBoxCollider;
    PolygonCollider2D playerFeetCollider;
    Animator playerAnimator;
    AudioSource playerAudioSource;

    float startingGravity; //Original gravity from playerRigidbody
    bool isHurting; //Track if hit, used to momentarily disable movement
    float hurtDelay = 1f; //Set delay from hit

    // Start is called before the first frame update
    void Start()
    {
        //Get reference to player rigidbody and animator
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerBoxCollider = GetComponent<BoxCollider2D>();
        playerFeetCollider = GetComponent<PolygonCollider2D>();
        playerAudioSource = GetComponent<AudioSource>();

        startingGravity = playerRigidBody.gravityScale;

        //Initial animation when entering level
        playerAnimator.SetTrigger("DoorExit");
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

        if (CrossPlatformInputManager.GetButtonDown("Vertical"))
        {
            playerAnimator.SetTrigger("DoorEnter");
            //Check events on player animator on "DoorIn"
        }
    }

    //
    public void LoadNextLevel()
    {
        FindObjectOfType<ExitDoor>().StartLoadingNextLevel();
        TurnOffRenderer();
    }

    public void TurnOffRenderer()
    {
        GetComponent<SpriteRenderer>().enabled = false;
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
            playerAudioSource.PlayOneShot(attackingSFX, .45f);

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
        playerAudioSource.PlayOneShot(gettingHitSFX, .3f);
        isHurting = true;
        StartCoroutine(StopHurting());
    }

    private void Climb()
    {
        if (playerBoxCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
            //Vector2 velocity = new Vector2(playerRigidBody.velocity.x, (controlThrow * climbSpeed) + playerRigidBody.velocity.y);
            Vector2 velocity = new Vector2(playerRigidBody.velocity.x, (controlThrow * climbSpeed));
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
        //Quick return if feet not touching ground/climbing layer
        bool isTouchingGround = playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        bool isTouchingBanner = playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"));

        if (isTouchingGround || isTouchingBanner)
        {
            bool isJumping = CrossPlatformInputManager.GetButtonDown("Jump");
            if (isJumping)
            {
                Vector2 jumpVelocity = new Vector2(playerRigidBody.velocity.x, jumpSpeed);
                playerRigidBody.velocity = jumpVelocity;
                playerAudioSource.PlayOneShot(jumpingSFX, .7f);
            }
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

    public void PlayWalkingSFX()
    {
        bool playerMovingHorizontally = Mathf.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon;

        if (playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            playerAudioSource.PlayOneShot(walkingSFX, .2f);
        }
        else
        {
            //Stop if player stops moving (sounds weird otherwise)
            playerAudioSource.Stop();
        }
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
