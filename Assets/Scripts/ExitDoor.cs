using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] float secondsToLoad = .5f;

    [SerializeField] AudioClip doorOpenSFX, doorCloseSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<Animator>().SetTrigger("Open");
    }

    public void StartLoadingNextLevel()
    {
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(secondsToLoad);

        GetComponent<Animator>().SetTrigger("Close");
        AudioSource.PlayClipAtPoint(doorCloseSFX, Camera.main.transform.position);

        yield return new WaitForSeconds(secondsToLoad/2f);

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    void PlayOpeningDoorSFX()
    {
        AudioSource.PlayClipAtPoint(doorOpenSFX, Camera.main.transform.position);
    }
}
