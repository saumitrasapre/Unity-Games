using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class AgentAnimator : MonoBehaviour
{
    protected Animator playerAnimatorComponent;
    private const string IS_WALKING = "IsWalking";
    private const string DIE = "Die";

    private void Awake()
    {
        playerAnimatorComponent = GetComponent<Animator>();
    }
    private void Update()
    {
        playerAnimatorComponent.SetBool(IS_WALKING, AgentMovement.Instance.getPlayerWalkingState());
    }

    public void PlayDeathAnimation()
    {
        playerAnimatorComponent.SetTrigger(DIE);
    }
}
