using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, IAgent, IHittable
{
    [SerializeField]
    private int maxHealth;
    private int health;
    public int Health
    {
        get => health; set
        {
            health = Mathf.Clamp(value, 0, maxHealth);
            uiHealth.UpdateUI(health);
        }
    }

    [field: SerializeField]
    public int Coins { get; set; }
    private bool isDead = false;
    private bool HasKey { get; set; }
    private PlayerWeapon playerWeapon;

    [field: SerializeField]
    public UIHealth uiHealth { get; set; }

    [field: SerializeField]
    public UICoins uiCoins { get; set; }

    [field: SerializeField]
    public UnityEvent OnDie { get; set; }
    [field: SerializeField]
    public UnityEvent OnGetHit { get; set; }
    [field: SerializeField]
    public UnityEvent OnGetKey{ get; set; }

    private void Awake()
    {
        playerWeapon = GetComponentInChildren<PlayerWeapon>();
    }

    private void Start()
    {
        HasKey = false;
        Health = maxHealth;
        uiHealth.Initialize(Health);
        Coins = 0;
        uiCoins.UpdateCoinText(this.Coins);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Resource"))
        {
            Resource resource = collision.gameObject.GetComponent<Resource>();
            if (resource != null)
            {
                switch (resource.ResourceData.ResourceType)
                {
                    case ResourceTypeEnum.Health:
                        if (Health >= maxHealth)
                        {
                            return;
                        }
                        else
                        {
                            Health += resource.ResourceData.GetAmount();
                            resource.PickupResource();
                        }
                        break;
                    case ResourceTypeEnum.Ammo:
                        if (playerWeapon.AmmoFull)
                        {
                            return;
                        }
                        else
                        {
                            playerWeapon.AddAmmo(resource.ResourceData.GetAmount());
                            resource.PickupResource();
                        }
                        break;
                    case ResourceTypeEnum.Key:
                        if (HasKey)
                        {
                            return;
                        }
                        else
                        {
                            HasKey = true;
                            resource.PickupResource();
                        }
                        break;
                    case ResourceTypeEnum.Coin:
                        {
                            this.Coins += resource.ResourceData.GetAmount();
                            uiCoins.UpdateCoinText(this.Coins);
                            resource.PickupResource();
                            if (OnGetKey != null)
                            {
                                OnGetKey.Invoke();
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void DisableInput()
    {
        GameInput.Instance.DisableGameInput();
    }

}
