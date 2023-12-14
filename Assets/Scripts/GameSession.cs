using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3, score = 0;
    [SerializeField] TextMeshProUGUI scoreText, livesText; 

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

    private void Start()
    {
        //Assign values
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }

    public void AddToScore(int value)
    {
        //Add to score and update UI element
        score += value;
        scoreText.text = score.ToString();
    }

    public void AddToLives()
    {
        //Add life and update UI element
        playerLives++;
        livesText.text = playerLives.ToString();
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
        //Remove player life and update UI element
        playerLives--;
        livesText.text = playerLives.ToString();
    }
}
