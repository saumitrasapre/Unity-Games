using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeFeedback : Feedback
{
    [SerializeField] private GameObject objectToShake = null;
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private float strength = 1f;
    [SerializeField] private float randomness = 90f;
    [SerializeField] private int vibrato = 10;
    [SerializeField] private bool snapping = false;
    [SerializeField] private bool fadeOut = true;

    public override void CompletePreviousFeedback()
    {
        objectToShake.transform.DOComplete();
    }

    public override void CreateFeedback()
    {
        CompletePreviousFeedback();
        objectToShake.transform.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut);
    }
}
