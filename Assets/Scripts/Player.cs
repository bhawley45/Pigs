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
    }
}
