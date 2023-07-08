using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIDecision : MonoBehaviour
{
    protected AIActionData aiActionData;
    protected AIMovementData aiMovementData;
    protected EnemyAIBrain enemyBrain;

    private void Awake()
    {
        aiActionData = this.transform.root.GetComponentInChildren<AIActionData>();
        aiMovementData = this.transform.root.GetComponentInChildren<AIMovementData>();
        enemyBrain = this.transform.root.GetComponent<EnemyAIBrain>();
    }

    public abstract bool MakeADecision();
}
