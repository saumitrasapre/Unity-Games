using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/WeaponData")]
public class WeaponDataSO : ScriptableObject
{
    [field:SerializeField]
    public BulletDataSO BulletData { get; set; }
    [field: SerializeField]
    [field: Range(0, 100)]
    public int AmmoCapacity { get; set; } = 100;
    [field: SerializeField]
    public bool AutomaticFire { get; set; } = false;
    [field: SerializeField]
    [field: Range(0.1f, 2f)]
    public float WeaponDelay { get; set; } = 0.1f;
    [field: SerializeField]
    [field: Range(0f, 10f)]
    public float SpreadAngle { get; set; } = 5f;
    [SerializeField]
    private bool multiBulletShoot = false;
    [SerializeField]
    [Range(1,10)]
    private int bulletCount = 1;

    internal int GetBulletCountToSpawn()
    {
        if (multiBulletShoot == true)
        {
            return bulletCount;
        }
        else
        {
            return 1;
        }
    }
}
