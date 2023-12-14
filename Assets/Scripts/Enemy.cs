using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float enemyRunSpeed = 5f;

    [SerializeField] AudioClip deathSFX;

    Rigidbody2D enemyRigidBody;
    Animator enemyAnimator;

    //Destroy object after death
    float deathDelay = 1f;

    // Start is called before the first frame update
    void Start()
    {
        enemyRigidBody = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Initial run to the left
        enemyRigidBody.velocity = new Vector2(-enemyRunSpeed, 0f);
    }

    public void Dying()
    {
        //Play death animation
        enemyAnimator.SetTrigger("Die");

        //Play death SFX
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, .5f);

        //Turn off colliders to prevent hitting player and movement
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        enemyRigidBody.bodyType = RigidbodyType2D.Static;

        //NOTE: want to make enemy sink into floor upon death

        StartCoroutine(DestroyEnemy());
    }

    //Enemy Movement
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Flip velocity on x axis
        enemyRunSpeed *= -1;

        //Flip Sprite
        transform.localScale = new Vector2(Mathf.Sign(enemyRigidBody.velocity.x), 1f);
    }

    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(deathDelay);
        Destroy(gameObject);
    }
}
