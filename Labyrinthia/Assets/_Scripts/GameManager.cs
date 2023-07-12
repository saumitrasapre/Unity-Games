using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture = null;
    [SerializeField] private GameObject backgroundMusic = null;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;    
    public event EventHandler OnMusicMuted;
    public event EventHandler OnMusicUnmuted;
    public static GameManager Instance { get; private set; }
    private int gameScore = 0;

    public enum GameState
    {
        /*WaitingToStart,*/
        GamePlaying,
        GameOver
    }
    private GameState gameState;
    private bool isGamePaused = false;
    private bool isMusicMuted = false;
    private void Awake()
    {
        Time.timeScale = 1f;
        this.gameScore = 0;
        Instance = this;
        gameState = GameState.GamePlaying;
    }
    private void Start()
    {
        SetCursorIcon();
        GameInput.Instance.OnPausePerformed += GameInput_OnPausePerformed;
    }

    private void GameInput_OnPausePerformed(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void SetCursorIcon()
    {
        Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width / 2f, cursorTexture.height / 2f), CursorMode.Auto);
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            GameInput.Instance.DisableGameInput();
            if (OnGamePaused != null)
            {
                OnGamePaused(this, EventArgs.Empty);
            }
        }
        else
        {
            Time.timeScale = 1f;
            GameInput.Instance.EnableGameInput();
            if (OnGameUnpaused != null)
            {
                OnGameUnpaused(this, EventArgs.Empty);
            }
        }
    }

    public void ToggleMusic()
    {
        isMusicMuted = !isMusicMuted;
        if (isMusicMuted)
        {
            if (backgroundMusic != null)
            {
                backgroundMusic.GetComponent<AudioSource>().volume = 0;
                if (OnMusicMuted != null)
                {
                    OnMusicMuted(this, EventArgs.Empty);
                }
            }
        }
        else
        {
            if (backgroundMusic != null)
            {
                backgroundMusic.GetComponent<AudioSource>().volume = 0.5f;
                if (OnMusicUnmuted != null)
                {
                    OnMusicUnmuted(this, EventArgs.Empty);
                }
            }
        }
    }

    public bool IsGamePlaying()
    {
        return gameState == GameState.GamePlaying;
    }

    public bool IsGameOver()
    {
        return gameState == GameState.GameOver;
    }

    public int GetGameScore()
    {
        return this.gameScore;
    }
    public void SetGameScore(int gameScore)
    {
        this.gameScore = gameScore;
    }

    public void SetGameState(GameState state)
    {
        this.gameState = state;
    }

    public bool IsGameMuted()
    {
        return isMusicMuted;
    }

}
