using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;

    //Awake starts before the "Start()" Method
    private void Awake()
    {
        //Look for any other game sessions and destroy them
        int numberOfGameSessions = FindObjectsOfType<GameSession>().Length;

        if(numberOfGameSessions > 1 )
        {
            Destroy( gameObject );
        }
        else
        {
            DontDestroyOnLoad( gameObject );
        }
    }

    public void ProcessPlayerDeath()
    {
        if(playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGame();
        }
    }

    private void ResetGame()
    {
        //Return to main menu
        SceneManager.LoadScene(0);

        //Remove persistent stats
        Destroy( gameObject );
    }

    private void TakeLife()
    {
        playerLives--;
    }
}
