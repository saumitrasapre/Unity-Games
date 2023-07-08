using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BulletShellGenerator : ObjectPool
{
    [SerializeField] private float flyDuration = 0.3f;
    [SerializeField] private float flyStrength = 1;

    private void Start()
    {
        Weapon.Instance.OnShoot += Weapon_OnShoot;
    }

    private void OnDestroy()
    {
        Weapon.Instance.OnShoot -= Weapon_OnShoot;
    }

    private void Weapon_OnShoot(object sender, EventArgs e)
    {
        SpawnBulletShell();
    }

    public void SpawnBulletShell()
    {
        GameObject shell = SpawnObject();
        MoveShellInRandomDirection(shell);
    }

    private void MoveShellInRandomDirection(GameObject shell)
    {
        shell.transform.DOComplete();
        Vector2 randomDirection = Random.insideUnitCircle;
        randomDirection = randomDirection.y > 0 ? new Vector2(randomDirection.x, -randomDirection.y) : randomDirection;
        shell.transform.DOMove(((Vector2)this.transform.position + randomDirection) * flyStrength, flyDuration).OnComplete(()=>shell.GetComponentInChildren<AudioSource>().Play());
        shell.transform.DORotate(new Vector3(0, 0, Random.Range(0, 360f)), flyDuration);
    }
}
