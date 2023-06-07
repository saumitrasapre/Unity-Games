using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    private AudioSource audioSource;
    private float stoveWarningSoundTimer;
    private bool playStoveWarningSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStoveStateChanged += StoveCounter_OnStoveStateChanged;
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnWarningShowProgressAmount = .5f;
        playStoveWarningSound = stoveCounter.IsFried() && e.progressNormalized >= burnWarningShowProgressAmount;
    }

    private void StoveCounter_OnStoveStateChanged(object sender, StoveCounter.OnStoveStateChangedEventArgs e)
    {
        bool playSound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        if (playSound)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }
    private void Update()
    {
        if (playStoveWarningSound)
        {
            stoveWarningSoundTimer -= Time.deltaTime;
            if (stoveWarningSoundTimer <= 0f)
            {
                float warningSoundTimerMax = .2f;
                stoveWarningSoundTimer = warningSoundTimerMax;

                SoundManager.Instance.PlayStoveWarningSound(stoveCounter.transform.position);
            }
        }
       
    }
}
