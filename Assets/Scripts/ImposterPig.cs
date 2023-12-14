using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImposterPig : MonoBehaviour
{
    Rigidbody2D enemyRigidbody;
    BoxCollider2D enemyBoxCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyBoxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyBoxCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            enemyRigidbody.velocity = new Vector2(0, Random.Range(1f, 5f));
        }
    }
}
