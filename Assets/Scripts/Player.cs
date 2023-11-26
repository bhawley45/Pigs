using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField] float runSpeed = 5f;

    Rigidbody2D playerRigidBody;
    Animator playerAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        //Get reference to player rigidbody and animator
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");

        Vector2 velocity = new Vector2(controlThrow * runSpeed, playerRigidBody.velocity.y);
        playerRigidBody.velocity = velocity;

        //.Epsilon is used because the velocity can never be truly 0 from the controller?
        bool runningHorizontally = Mathf.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon;

        FlipSprite(runningHorizontally);
        UpdateRunningState(runningHorizontally);
    }

    private void UpdateRunningState(bool isRunning)
    {
        //Run animation if running, not if not
        playerAnimator.SetBool("Running", isRunning);
    }

    private void FlipSprite(bool isRunning)
    {
        if(isRunning)
        {
            // Left -> -1 | Right -> 1 (Mathf.Sign(playerRigidBody.velocity.x))
            transform.localScale = new Vector2(Mathf.Sign(playerRigidBody.velocity.x) ,1f);
        }
    }
}
