using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeFreezeFeedback : Feedback
{
    [SerializeField] private float freezeTimeDelay = 0.01f;//Time to wait before freezing time
    [SerializeField] private float unfreezeTimeDelay = 0.02f; //Time to wait before unfreezing time
    [SerializeField]
    [Range(0, 1)]
    private float timeFreezeValue = 0.2f;

    public override void CompletePreviousFeedback()
    {
        if (TimeController.Instance != null)
        {
            TimeController.Instance.ResetTimeScale();
        }

    }

    public override void CreateFeedback()
    {
        TimeController.Instance.ModifyTimescale(timeFreezeValue, freezeTimeDelay, () => TimeController.Instance.ModifyTimescale(1, unfreezeTimeDelay));
    }
}
