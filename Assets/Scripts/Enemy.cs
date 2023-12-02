using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float enemyRunSpeed = 5f;

    Rigidbody2D enemyRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        enemyRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Run to the left
        enemyRigidBody.velocity = new Vector2(-enemyRunSpeed, 0f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Flip velocity on x axis
        enemyRunSpeed *= -1;

        //Flip Sprite
        transform.localScale = new Vector2(Mathf.Sign(enemyRigidBody.velocity.x), 1f);
    }
}
