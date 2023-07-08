using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICoins : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text = null;

    public void UpdateCoinText(int coinCount)
    {
        text.SetText(coinCount.ToString());
    }
}
