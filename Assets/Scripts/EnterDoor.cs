using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDoor : MonoBehaviour
{
    [SerializeField] AudioClip doorOpenSFX, doorCloseSFX;

    private void Start()
    {
        GetComponent<Animator>().SetTrigger("Open");
    }

    void PlayOpeningDoorSFX()
    {
        AudioSource.PlayClipAtPoint(doorOpenSFX, Camera.main.transform.position);
    }

    void PlayClosingDoorSFX()
    {
        AudioSource.PlayClipAtPoint(doorOpenSFX, Camera.main.transform.position);
    }
}
