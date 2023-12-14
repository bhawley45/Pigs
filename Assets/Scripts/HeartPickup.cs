using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] AudioClip heartPickupSFX;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Play audio clip at the position of main camera
        AudioSource.PlayClipAtPoint(heartPickupSFX, Camera.main.transform.position);

        FindObjectOfType<GameSession>().AddToLives();

        Destroy(gameObject);
    }
}
