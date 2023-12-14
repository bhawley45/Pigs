using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondPickup : MonoBehaviour
{
    [SerializeField] AudioClip diamondPickupSFX;
    [SerializeField] int diamondValue = 100; //customize value in engine

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Play audio clip at the position of main camera
        AudioSource.PlayClipAtPoint(diamondPickupSFX, Camera.main.transform.position);

        FindObjectOfType<GameSession>().AddToScore(diamondValue);

        Destroy(gameObject);
    }
}
