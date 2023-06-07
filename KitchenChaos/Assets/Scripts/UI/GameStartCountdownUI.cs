using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    private const string NUMBER_POPUP = "NumberPopup";
    [SerializeField] private TextMeshProUGUI countdownText;
    private Animator animator;
    private int previousCountdownNumber;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        animator = GetComponent<Animator>();
        Hide();
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStartActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Update()
    {
        int countdownNumber = Mathf.CeilToInt(GameManager.Instance.GetCountdownToStartTimer()); 
        countdownText.text = countdownNumber.ToString();

        if(countdownNumber != previousCountdownNumber )
        {
            previousCountdownNumber = countdownNumber;
            animator.SetTrigger(NUMBER_POPUP);
            SoundManager.Instance.PlayCountdownSound();
        }
    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
    }

    private void Show()
    {
        this.gameObject.SetActive(true);
    }
}
