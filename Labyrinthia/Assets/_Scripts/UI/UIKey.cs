using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIKey : MonoBehaviour
{
    [SerializeField] GameObject keyPanel = null;
    [SerializeField] TextMeshProUGUI text = null;
    private const string POSITIVE_KEY_TEXT = "Key Acquired!";
    private const string NEGATIVE_KEY_TEXT = "Key Required!";

    public void UpdateKeyText(bool playerHasKey)
    {

        if (playerHasKey)
        {
            keyPanel.gameObject.SetActive(true);
            text.SetText(POSITIVE_KEY_TEXT);
        }
        else
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
