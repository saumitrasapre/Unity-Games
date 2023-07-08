using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LookDecision : AIDecision
{
    [SerializeField]
    [Range(1,15)]
    private float distance = 15f;
    [SerializeField] private LayerMask raycastMask = new LayerMask();
    [field:SerializeField]
    public UnityEvent OnPlayerSpotted { get; set; }

    public override bool MakeADecision()
    {
        Vector2 direction = enemyBrain.Target.transform.position - this.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, distance, raycastMask);
        if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (OnPlayerSpotted != null)
            {
                OnPlayerSpotted.Invoke();
            }
            return true;
        }
        return false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(UnityEditor.Selection.activeObject == this.gameObject && enemyBrain!=null && enemyBrain.Target != null)
        {
            Gizmos.color = Color.red;
            Vector2 direction = enemyBrain.Target.transform.position - this.transform.position;
            Gizmos.DrawRay(this.transform.position, direction.normalized * distance);
        }
    }
#endif
}
