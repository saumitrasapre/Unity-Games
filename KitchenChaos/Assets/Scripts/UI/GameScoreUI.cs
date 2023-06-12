using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameScoreText;

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        gameScoreText.text = GameManager.Instance.GetGameScore().ToString();
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        gameScoreText.text = GameManager.Instance.GetGameScore().ToString();
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        gameScoreText.text = GameManager.Instance.GetGameScore().ToString();
    }
}
