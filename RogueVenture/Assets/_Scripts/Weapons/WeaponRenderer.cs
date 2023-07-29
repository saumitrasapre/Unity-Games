using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class WeaponRenderer : MonoBehaviour
{
    [SerializeField] protected int playerSortingOrder = 0;
    protected SpriteRenderer weaponRenderer;

    private void Awake()
    {
        weaponRenderer = GetComponent<SpriteRenderer>();
    }

    public void FlipSprite(bool value)
    {
        int flipModifier = value ? -1 : 1;
        transform.localScale = new Vector3(transform.localScale.x, flipModifier * Mathf.Abs(transform.localScale.y), transform.localScale.z);
    }

    public void RenderBehindHead(bool value)
    {
        if (value)
        {
            weaponRenderer.sortingOrder = playerSortingOrder - 1;
        }
        else
        {
            weaponRenderer.sortingOrder = playerSortingOrder + 1;
        }
    }
}
