using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    Animator bombAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        bombAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bombAnimator.SetTrigger("Burn");
    }

    void DestroyBomb()
    {
        //gameObject represents the object the script is attached to
        Destroy(gameObject);
    }
}
