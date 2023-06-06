using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public event EventHandler OnGameStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
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
    private bool isGamePaused = false;

    private void Awake()
    {
        Instance = this;
        gameState = GameState.WaitingToStart;
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            if (OnGamePaused != null)
            {
                OnGamePaused(this, EventArgs.Empty);
            }
        }
        else
        {
            Time.timeScale = 1f;
            if (OnGameUnpaused != null)
            {
                OnGameUnpaused(this, EventArgs.Empty);
            }
        }
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
