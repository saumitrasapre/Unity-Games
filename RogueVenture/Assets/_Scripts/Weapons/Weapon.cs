using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    public static Weapon Instance { get; private set; }
    [SerializeField] protected GameObject muzzle;
    [SerializeField] protected int ammo = 10;
    [SerializeField] protected WeaponDataSO weaponData;
    public event EventHandler OnShoot;
    public event EventHandler OnShootNoAmmo;
    protected bool isShooting = false;
    [SerializeField] protected bool reloadCoroutine = false;
    public bool AmmoFull { get => Ammo >= weaponData.AmmoCapacity; }

    [field: SerializeField]
    public UnityEvent<int> OnAmmoChange { get; set; }
    public int Ammo
    {
        get { return ammo; }
        set
        {
            ammo = Mathf.Clamp(value, 0, weaponData.AmmoCapacity);
            if (OnAmmoChange != null)
            {
                OnAmmoChange.Invoke(ammo);
            }
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ammo = weaponData.AmmoCapacity;
    }

    private void Update()
    {
        UseWeapon();
    }

    private void UseWeapon()
    {
        if(isShooting==true && reloadCoroutine == false)
        {
            if (Ammo > 0)
            {
                Ammo--;
                if (OnShoot != null)
                {
                    OnShoot(this, EventArgs.Empty);
                }
                for (int i = 0; i < weaponData.GetBulletCountToSpawn(); i++)
                {
                    ShootBullet();
                }
            }
            else
            {
                isShooting = false;
                if (OnShootNoAmmo != null)
                {
                    OnShootNoAmmo(this, EventArgs.Empty);
                }
                return;
            }
            FinishShooting();
        }
    }

    private void FinishShooting()
    {
        StartCoroutine(DelayNextShotCoroutine());
        if(weaponData.AutomaticFire == false)
        {
            isShooting = false;
        }
    }

    protected IEnumerator DelayNextShotCoroutine()
    {
        reloadCoroutine = true;
        yield return new WaitForSeconds(weaponData.WeaponDelay);
        reloadCoroutine = false;
    }

    private void ShootBullet()
    {
        SpawnBullet(muzzle.transform.position, CalculateAngle(muzzle));
    }

    private void SpawnBullet(Vector3 position, Quaternion rotation)
    {
        GameObject bulletPrefab = Instantiate(weaponData.BulletData.BulletPrefab, position, rotation);
        bulletPrefab.GetComponent<Bullet>().BulletData = weaponData.BulletData;
    }

    private Quaternion CalculateAngle(GameObject muzzle)
    {
        float spread = Random.Range(-weaponData.SpreadAngle, weaponData.SpreadAngle);
        Quaternion bulletSpreadRotation = Quaternion.Euler(new Vector3(0, 0, spread));
        return muzzle.transform.rotation * bulletSpreadRotation;
    }

    public void TryShooting()
    {
        isShooting = true;
    }
    public void StopShooting()
    {
        isShooting = false;
    }

    public void reload(int ammo)
    {
        Ammo += ammo;
    }
}
