using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(() => {
            //Code that runs when play button is clicked
            Loader.Load(Loader.Scene.ProceduralScene);
        });
        
        quitButton.onClick.AddListener(() => {
            //Code that runs when quit button is clicked
            Application.Quit();
        });

        Time.timeScale = 1f;
    }
}
