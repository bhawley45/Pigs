using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField] float runSpeed = 5f;

    private Rigidbody2D playerRigidBody;
    
    // Start is called before the first frame update
    void Start()
    {
        //Get reference to player rigidbody
        playerRigidBody = GetComponent<Rigidbody2D>();
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
        FlipSprite();
    }

    private void FlipSprite()
    {
        //.Epsilon is used because the velocity can never be truly 0 from the controller?
        bool runningHorizontally = Mathf.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon;

        if(runningHorizontally)
        {
            // Left -> -1 | Right -> 1 (Mathf.Sign(playerRigidBody.velocity.x))
            transform.localScale = new Vector2(Mathf.Sign(playerRigidBody.velocity.x) ,1f);
        }
    }
}
