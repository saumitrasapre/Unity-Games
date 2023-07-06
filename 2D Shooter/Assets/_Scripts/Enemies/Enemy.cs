using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IHittable, IAgent, IKnockBackable
{
    [SerializeField] private EnemyDataSO enemyData;

    [field: SerializeField]
    public EnemyAttack enemyAttack { get; set; }
    public static Enemy Instance { get; private set; }

    private bool isDead = false;

    private EnemyMovement enemyMovement;

    public int Health { get; private set; } = 2;

    [field: SerializeField]
    public UnityEvent OnGetHit { get; set; }
    [field: SerializeField]
    public UnityEvent OnDie { get; set; }

    private void Awake()
    {
        Instance = this;
        if (enemyAttack == null)
        {
            enemyAttack = GetComponent<EnemyAttack>();
        }
        enemyMovement = GetComponent<EnemyMovement>();
    }

    private void Start()
    {
        Health = enemyData.MaxHealth;
    }

    public void GetHit(int damage, GameObject damageDealer)
    {
        if (isDead == false)
        {
            Health -= damage;
            if (OnGetHit != null)
            {
                OnGetHit.Invoke();
            }
            if (Health <= 0)
            {
                isDead = true;
                if (OnDie != null)
                {
                    OnDie.Invoke();
                }
            }
        }

    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    public void PerformAttack()
    {
        if (isDead == false)
        {
            enemyAttack.Attack(enemyData.Damage);
        }
    }

    public void KnockBack(Vector2 direction, float power, float duration)
    {
        enemyMovement.KnockBack(direction, power, duration);
    }
}
