using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moveUpKeyText;
    [SerializeField] private TextMeshProUGUI moveLeftKeyText;
    [SerializeField] private TextMeshProUGUI moveDownKeyText;
    [SerializeField] private TextMeshProUGUI moveRightKeyText;
    [SerializeField] private TextMeshProUGUI interactKeyText;
    [SerializeField] private TextMeshProUGUI interactAlternateKeyText;
    [SerializeField] private TextMeshProUGUI pauseKeyText;
    [SerializeField] private TextMeshProUGUI interactGamepadKeyText;
    [SerializeField] private TextMeshProUGUI interactGamepadAlternateKeyText;
    [SerializeField] private TextMeshProUGUI pauseGamepadKeyText;


    private void Start()
    {
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        UpdateVisual();
        Show();
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void GameInput_OnBindingRebind(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        moveUpKeyText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownKeyText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveLeftKeyText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        moveRightKeyText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        interactKeyText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAlternateKeyText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact_Alternate);
        pauseKeyText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        interactGamepadKeyText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
        interactGamepadAlternateKeyText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact_Alternate);
        pauseGamepadKeyText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);
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
