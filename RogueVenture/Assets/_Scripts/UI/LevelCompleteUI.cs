using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteUI : MonoBehaviour
{
    [SerializeField] Button nextLevelButton;
    [SerializeField] Button returnToMainMenuButton;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] private Player player;

    private void Start()
    {
        if (player != null)
        { 
            player.OnChestOpened += Player_OnChestOpened;
        }
        this.gameObject.SetActive(false);
    }

    private void Player_OnChestOpened(object sender, System.EventArgs e)
    {
        this.gameObject.SetActive(true);
        nextLevelButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.ProceduralScene);
        });
        returnToMainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        scoreText.text = "Score - " + GameManager.Instance.GetGameScore().ToString();
        player.DisableInput();
        player.GetComponent<AgentMovement>().StopImmediately();
    }
}
