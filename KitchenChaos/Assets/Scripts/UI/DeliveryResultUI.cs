using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    private const string POPUP = "Popup";
    [SerializeField] Image backgroundImage;
    [SerializeField] Image iconImage;
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] Color successColor;
    [SerializeField] Color failedColor;
    [SerializeField] Sprite successSprite;
    [SerializeField] Sprite failedSprite;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        this.gameObject.SetActive(false);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        this.gameObject.SetActive(true);
        backgroundImage.color = failedColor;
        iconImage.sprite = failedSprite;
        messageText.text = "DELIVERY\nFAILED";
        animator.SetTrigger(POPUP);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        this.gameObject.SetActive(true);
        backgroundImage.color = successColor;
        iconImage.sprite = successSprite;
        messageText.text = "DELIVERY\nSUCCESS";
        animator.SetTrigger(POPUP);
    }
}
