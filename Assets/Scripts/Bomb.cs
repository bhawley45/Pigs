using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] float radius = 3f;
    [SerializeField] Vector2 explosionForce = new Vector2(35f, 35f);

    [SerializeField] AudioClip fuseBurnSFX;
    [SerializeField] AudioClip explosionSFX;
    
    Animator bombAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        bombAnimator = GetComponent<Animator>();
    }

    void ExplodeBomb()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, radius, LayerMask.GetMask("Player"));

        if (playerCollider)
        {
            //Kick back player on explosion
            playerCollider.GetComponent<Rigidbody2D>().AddForce(explosionForce);
            playerCollider.GetComponent<Player>().PlayerHit();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Start burning animation
        bombAnimator.SetTrigger("Burn");
        AudioSource.PlayClipAtPoint(fuseBurnSFX, Camera.main.transform.position);
    }

    void DestroyBomb()
    {
        //gameObject represents the object the script is attached to
        Destroy(gameObject);
    }

    void PlayExplosionSFX()
    {
        AudioSource.PlayClipAtPoint(explosionSFX, Camera.main.transform.position);
    }

    //Debug Visualized for AOE of bomb
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
