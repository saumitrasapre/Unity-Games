using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public event EventHandler OnGameStateChanged;
    private enum GameState
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    private GameState gameState;
    private float waitingToStartTimer = 1f;
    private float countdownToStartTimer = 3f;
    private float gamePLayingTimer;
    private float gamePLayingTimerMax = 10f;

    private void Awake()
    {
        Instance = this;
        gameState = GameState.WaitingToStart;
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer < 0f)
                {
                    gameState = GameState.CountdownToStart;
                    if (OnGameStateChanged != null)
                    {
                        OnGameStateChanged(this, EventArgs.Empty);
                    }
                }
                break;

            case GameState.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0f)
                {
                    gamePLayingTimer = gamePLayingTimerMax;
                    gameState = GameState.GamePlaying;
                    if (OnGameStateChanged != null)
                    {
                        OnGameStateChanged(this, EventArgs.Empty);
                    }
                }
                break;

            case GameState.GamePlaying:
                gamePLayingTimer -= Time.deltaTime;
                if (gamePLayingTimer < 0f)
                {
                    gameState = GameState.GameOver;
                    if (OnGameStateChanged != null)
                    {
                        OnGameStateChanged(this, EventArgs.Empty);
                    }
                }
                break;

            case GameState.GameOver:
                break;
        }
        Debug.Log(gameState);
    }

    public bool IsGamePlaying()
    {
        return gameState == GameState.GamePlaying;  
    }

    public bool IsCountdownToStartActive()
    {
        return gameState == GameState.CountdownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return this.countdownToStartTimer;
    }

    public bool IsGameOver()
    {
        return gameState == GameState.GameOver;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return (gamePLayingTimer / gamePLayingTimerMax);
    }
}
