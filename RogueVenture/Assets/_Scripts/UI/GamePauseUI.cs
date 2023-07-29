using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button muteMusicButton;
    private const string MUSIC_UNMUTED_TEXT = "Mute music";
    private const string MUSIC_MUTED_TEXT = "Unmute music";

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.TogglePauseGame();
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        muteMusicButton.onClick.AddListener(() =>
        {
            GameManager.Instance.ToggleMusic();
        });
    }
    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
        GameManager.Instance.OnMusicMuted += GameManager_OnMusicMuted;
        GameManager.Instance.OnMusicUnmuted += GameManager_OnMusicUnmuted;
        Hide();
    }

    private void GameManager_OnMusicUnmuted(object sender, EventArgs e)
    {
        muteMusicButton.GetComponentInChildren<TextMeshProUGUI>().text = MUSIC_UNMUTED_TEXT;
    }

    private void GameManager_OnMusicMuted(object sender, EventArgs e)
    {
        muteMusicButton.GetComponentInChildren<TextMeshProUGUI>().text = MUSIC_MUTED_TEXT;
    }

    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
        if (GameManager.Instance.IsGameMuted())
        {
            muteMusicButton.GetComponentInChildren<TextMeshProUGUI>().text = MUSIC_MUTED_TEXT;
        }
        else
        {
            muteMusicButton.GetComponentInChildren<TextMeshProUGUI>().text = MUSIC_UNMUTED_TEXT;
        }
    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
    }

    private void Show()
    {
        this.gameObject.SetActive(true);
        resumeButton.Select();
    }
}
