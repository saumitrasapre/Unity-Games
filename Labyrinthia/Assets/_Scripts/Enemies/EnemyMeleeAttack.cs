using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : EnemyAttack
{
    public override void Attack(int damage)
    {
        if (waitBeforeNextAttack == false)
        {
            IHittable hittable = GetTarget().GetComponent<IHittable>();
            if (hittable != null)
            {
                hittable.GetHit(damage, this.gameObject);
            }
            StartCoroutine(WaitBeforeAttackCoroutine());
            
        }
    }
}
