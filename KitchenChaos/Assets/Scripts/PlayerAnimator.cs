using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private Animator playerAnimatorComponent;

    [SerializeField] private Player player;

    private void Awake()
    {
        playerAnimatorComponent = GetComponent<Animator>();
    }

    private void Update()
    {
        playerAnimatorComponent.SetBool(IS_WALKING, player.getPlayerWalkingState());
    }
}
