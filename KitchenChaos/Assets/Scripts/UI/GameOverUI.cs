using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    [SerializeField] private TextMeshProUGUI gameScoreText;
    [SerializeField] private Button restartGameButton;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        Hide();
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {
            Show();
            recipesDeliveredText.text = DeliveryManager.Instance.GetSucessfulRecipesAmount().ToString();
            gameScoreText.text = GameManager.Instance.GetGameScore().ToString();
            restartGameButton.onClick.AddListener(() => {
                //Code that runs when play button is clicked
                Loader.Load(Loader.Scene.GameScene);
            });
            mainMenuButton.onClick.AddListener(() => {
                //Code that runs when play button is clicked
                Loader.Load(Loader.Scene.MainMenuScene);
            });
        }
        else
        {
            Hide();
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
