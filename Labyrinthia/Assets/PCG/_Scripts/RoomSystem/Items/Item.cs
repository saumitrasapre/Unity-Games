using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class Item : MonoBehaviour, IHittable
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private BoxCollider2D itemCollider;
    [SerializeField]
    private BoxCollider2D itemChildCollider;
    
    [SerializeField]
    int health = 3;
    [SerializeField]
    bool nonDestructible;

    [SerializeField]
    private GameObject hitFeedback, destoyFeedback;

    public UnityEvent OnGetHit { get; set ; }

    [field: SerializeField]
    public UnityEvent OnDie { get; set; }

    public void Initialize(ItemData itemData)
    {
        //set sprite
        spriteRenderer.sprite = itemData.sprite;
        //set sprite offset
        spriteRenderer.transform.localPosition = new Vector2(0.5f * itemData.size.x, 0.5f * itemData.size.y);
        itemCollider.size = itemData.size;
        itemChildCollider.size = itemData.size/(Vector2) this.transform.GetChild(0).localScale;
        itemCollider.offset = spriteRenderer.transform.localPosition;
        if (itemData.hasAnimation)
        {
            this.transform.GetChild(0).AddComponent<Animator>();
            this.GetComponentInChildren<Animator>().runtimeAnimatorController = itemData.animatorController;
        }

        if (itemData.isIlluminated)
        {
            AddIllumination(itemData.illuminationColor,itemData.illuminationIntensity, itemData.illuminationInnerRadius, itemData.illuminationOuterRadius);
        }
        
        if (itemData.nonDestructible)
            nonDestructible = true;

        this.health = itemData.health;

    }

    public void AddIllumination(Color illuminationColor, float illuminationIntensity, float illuminationInnerRadius, float illuminationOuterRadius)
    {

        Light2D light = this.GetComponentInChildren<Light2D>();
        light.enabled = true;
        light.lightType = Light2D.LightType.Point;
        light.color = illuminationColor;
        light.intensity = illuminationIntensity;
        light.shadowsEnabled = true;
        light.shadowIntensity = 1;
        light.pointLightInnerRadius = illuminationInnerRadius;
        light.pointLightOuterRadius = illuminationOuterRadius;

    }

    public void GetHit(int damage, GameObject damageDealer)
    {
        if (nonDestructible)
            return;
        if (health > 1 && hitFeedback!=null)
            Instantiate(hitFeedback, spriteRenderer.transform.position, Quaternion.identity);
        else if (destoyFeedback!=null)
            Instantiate(destoyFeedback, spriteRenderer.transform.position, Quaternion.identity);
        spriteRenderer.transform.DOShakePosition(0.2f, 0.3f, 75, 1, false, true).OnComplete(ReduceHealth);
    }

    private void ReduceHealth()
    {
        health--;
        if (health <= 0)
        {
            if (OnDie != null)
            {
                OnDie.Invoke();
            }
            spriteRenderer.transform.DOComplete();
            Destroy(gameObject);
        }

    }
}

