using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAudio : AudioPlayer
{
    [SerializeField] private AudioClip shootBulletClip = null;
    [SerializeField] private AudioClip outOfBulletsClip = null;

    private void Start()
    {
        Weapon.Instance.OnShoot += Weapon_OnShoot;
        Weapon.Instance.OnShootNoAmmo += Weapon_OnShootNoAmmo;
    }

    private void Weapon_OnShootNoAmmo(object sender, System.EventArgs e)
    {
        PlayOutOfBulletsSound();
    }

    private void Weapon_OnShoot(object sender, System.EventArgs e)
    {
        PlayShootSound();
    }

    public void PlayShootSound()
    {
        PlayClip(shootBulletClip);
    }

    public void PlayOutOfBulletsSound()
    {
        PlayClip(outOfBulletsClip);
    }
}
