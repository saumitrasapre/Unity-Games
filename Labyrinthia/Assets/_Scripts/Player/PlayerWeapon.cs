using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : AgentWeapon
{
    [SerializeField] private UIAmmo uiAmmo = null;

    public bool AmmoFull { get=>weapon!=null && weapon.AmmoFull; }
    private void Start()
    {
        GameInput.Instance.OnMousePositionChanged += GameInput_OnMousePositionChanged;
        GameInput.Instance.OnFirePressed += GameInput_OnFirePressed;
        GameInput.Instance.OnFireReleased += GameInput_OnFireReleased;
        if (weapon != null)
        {
            weapon.OnAmmoChange.AddListener(uiAmmo.UpdateBulletText);
            uiAmmo.UpdateBulletText(weapon.Ammo);
        }
    }

    public void AddAmmo(int amount)
    {
        if (weapon != null)
        {
            weapon.Ammo += amount;
        }
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnMousePositionChanged -= GameInput_OnMousePositionChanged;
        GameInput.Instance.OnFirePressed -= GameInput_OnFirePressed;
        GameInput.Instance.OnFireReleased -= GameInput_OnFireReleased;
    }

    private void GameInput_OnMousePositionChanged(object sender, GameInput.OnMousePositionChangedEventArgs e)
    {
        AimWeapon(e.mousePosition);
    }

    private void GameInput_OnFireReleased(object sender, EventArgs e)
    {
        this.StopShooting();
    }

    private void GameInput_OnFirePressed(object sender, EventArgs e)
    {
        this.Shoot();
    }
}
