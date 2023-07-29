using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMapUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Camera mainMapCamera;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.ToggleShowMap();
        });

    }
    private void Start()
    {
        GameManager.Instance.OnMapShown += GameManager_OnMapShown;
        GameManager.Instance.OnMapHidden += GameManager_OnMapHidden;
        Hide();
    }

    private void GameManager_OnMapHidden(object sender, EventArgs e)
    {
        Hide();
    }

    private void GameManager_OnMapShown(object sender, EventArgs e)
    {
        Show();
    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
        mainMapCamera.gameObject.SetActive(false);
    }

    private void Show()
    {
        this.gameObject.SetActive(true);
        mainMapCamera.gameObject.SetActive(true);
        resumeButton.Select();
    }
}
