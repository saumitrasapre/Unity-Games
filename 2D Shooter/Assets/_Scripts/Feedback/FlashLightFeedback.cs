using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlashLightFeedback : Feedback
{
    [SerializeField] private Light2D lightTarget = null;
    [SerializeField] private float lightOnDelay = 0.01f;
    [SerializeField] private float lightOffDelay = 0.01f;
    private bool defaultState = false;

    public override void CompletePreviousFeedback()
    {
        StopAllCoroutines();
        lightTarget.enabled = defaultState;
    }

    public override void CreateFeedback()
    {
        StartCoroutine(ToggleLightCoroutine(lightOnDelay, true, () => StartCoroutine(ToggleLightCoroutine(lightOffDelay, false))));
    }

    IEnumerator ToggleLightCoroutine(float time, bool result, Action FinishCallback = null)
    {
        yield return new WaitForSeconds(time);
        lightTarget.enabled = result;
        if (FinishCallback != null)
        {
            FinishCallback.Invoke();
        }
    }
}
