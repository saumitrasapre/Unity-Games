using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : AgentWeapon
{
    private void Start()
    {
        GameInput.Instance.OnMousePositionChanged += GameInput_OnMousePositionChanged;
        GameInput.Instance.OnFirePressed += GameInput_OnFirePressed;
        GameInput.Instance.OnFireReleased += GameInput_OnFireReleased;
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
