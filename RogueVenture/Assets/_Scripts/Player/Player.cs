using System;
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
    [SerializeField]
    private bool isNearTargetChest = false;
    [field:SerializeField]
    private bool HasKey { get; set; }
    private PlayerWeapon playerWeapon;
    private TargetChest targetChestnearPlayer = null;

    [field: SerializeField]
    public UIHealth uiHealth { get; set; }

    [field: SerializeField]
    public UICoins uiCoins { get; set; }

    [field: SerializeField]
    public UIKey uiKey { get; set; }

    [field: SerializeField]
    public UnityEvent OnDie { get; set; }
    [field: SerializeField]
    public UnityEvent OnGetHit { get; set; }

    public event EventHandler OnChestOpened;

    private void Awake()
    {
        HasKey = false;
        playerWeapon = GetComponentInChildren<PlayerWeapon>();
    }

    private void Start()
    {
        HasKey = false;
        isNearTargetChest = false;
        Health = maxHealth;
        uiHealth.Initialize(Health);
        Coins = 0;
        uiCoins.UpdateCoinText(this.Coins);
        GameManager.Instance.SetGameScore(this.Coins);
        targetChestnearPlayer = null;
        GameInput.Instance.OnInteractPerformed += GameInput_OnInteractPerformed;
    }

    private void GameInput_OnInteractPerformed(object sender, System.EventArgs e)
    {
        if (isNearTargetChest && targetChestnearPlayer!=null)
        {
            if (HasKey)
            {
                targetChestnearPlayer.OpenChest();
                if (OnChestOpened != null)
                {
                    OnChestOpened(this,EventArgs.Empty);
                }
            }
            else
            {
                uiKey.UpdateKeyText(isNearTargetChest, HasKey);
            }
        }
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Resource") || collision.gameObject.layer == LayerMask.NameToLayer("TargetChest"))
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
                            uiKey.UpdateKeyText(isNearTargetChest, HasKey);
                        }
                        break;
                    case ResourceTypeEnum.Coin:
                        {
                            this.Coins += resource.ResourceData.GetAmount();
                            uiCoins.UpdateCoinText(this.Coins);
                            GameManager.Instance.SetGameScore(this.Coins);
                            resource.PickupResource();
                        }
                        break;
                    case ResourceTypeEnum.TargetChest:
                        {
                            isNearTargetChest = true;
                            targetChestnearPlayer = collision.gameObject.GetComponent<TargetChest>();
                            uiKey.UpdateKeyText(isNearTargetChest, HasKey);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Resource") || collision.gameObject.layer == LayerMask.NameToLayer("TargetChest"))
        {
            Resource resource = collision.gameObject.GetComponent<Resource>();
            if (resource != null)
            {
                switch (resource.ResourceData.ResourceType)
                {
                    case ResourceTypeEnum.None:
                        break;
                    case ResourceTypeEnum.Health:
                        break;
                    case ResourceTypeEnum.Ammo:
                        break;
                    case ResourceTypeEnum.Key:
                        break;
                    case ResourceTypeEnum.Coin:
                        break;
                    case ResourceTypeEnum.TargetChest:
                        {
                            isNearTargetChest = false;
                            targetChestnearPlayer = null;
                            uiKey.UpdateKeyText(isNearTargetChest, HasKey);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public bool PlayerHasKey()
    {
        return HasKey;
    }

    public void DisableInput()
    {
        GameInput.Instance.DisableGameInput();
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnInteractPerformed -= GameInput_OnInteractPerformed;
    }

}
