using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIKey : MonoBehaviour
{
    [SerializeField] GameObject keyPanel = null;
    [SerializeField] TextMeshProUGUI text = null;
    private const string KEY_ACQUIRED_TEXT = "Key Acquired!";
    private const string PLAYER_NEAR_CHEST_AND_HAS_KEY = "Press E!";
    private const string NEGATIVE_KEY_TEXT = "Key Required!";

    public void UpdateKeyText(bool targetChestnearPlayer, bool playerHasKey)
    {

        if (playerHasKey)
        {
            if (targetChestnearPlayer)
            {
                keyPanel.gameObject.SetActive(true);
                text.SetText(PLAYER_NEAR_CHEST_AND_HAS_KEY);
            }
            else
            {
                keyPanel.gameObject.SetActive(true);
                text.SetText(KEY_ACQUIRED_TEXT);
            }
        }
        else if(playerHasKey==false && targetChestnearPlayer)
        {
            keyPanel.gameObject.SetActive(true);
            text.SetText(NEGATIVE_KEY_TEXT);
            StartCoroutine(WaitTimer());
        }

    }
    IEnumerator WaitTimer()
    {
        yield return new WaitForSeconds(2);
        keyPanel.gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}
