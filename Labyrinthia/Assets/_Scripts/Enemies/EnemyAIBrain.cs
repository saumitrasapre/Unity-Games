using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIBrain : MonoBehaviour
{
    [field: SerializeField]
    public GameObject Target { get; set; }

    [field:SerializeField]
    private AIState CurrentState { get; set; }

    [SerializeField]
    EnemyRenderer enemyRenderer;

    [SerializeField]
    EnemyMovement enemyMovement;


    private void Update()
    {
        if (Target == null)
        {
            enemyMovement.MoveAgent(Vector2.zero);
        }
        else
        {
            CurrentState.UpdateState();
        }
        
    }

    public void Attack()
    {
        Enemy.Instance.PerformAttack();
    }

    public void Move(Vector2 movementDirection, Vector2 targetPosition)
    {
        enemyMovement.MoveAgent(movementDirection);
        enemyRenderer.FaceDirection(targetPosition);
    }

    internal void ChangeToState(AIState state)
    {
        CurrentState = state;
    }

    private void Awake()
    {
        enemyRenderer = GetComponentInChildren<EnemyRenderer>();
        enemyMovement = GetComponent<EnemyMovement>();
        Target = FindObjectOfType<Player>().gameObject;
    }
}
