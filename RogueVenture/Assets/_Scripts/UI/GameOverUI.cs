using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] Button restartGameButton;
    [SerializeField] Button returnToMainMenuButton;

    private void Start()
    {
        restartGameButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.ProceduralScene);
        });
        returnToMainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }
}
