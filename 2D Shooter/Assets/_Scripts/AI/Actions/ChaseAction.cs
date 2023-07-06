using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAction : AIAction
{
    public override void TakeAction()
    {
        Vector3 direction = enemyBrain.Target.transform.position - this.transform.position;
        aiMovementData.Direction = direction.normalized;
        aiMovementData.PointOfInterest = enemyBrain.Target.transform.position;
        enemyBrain.Move(aiMovementData.Direction, aiMovementData.PointOfInterest);
    }
}
