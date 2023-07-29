using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public Sprite sprite;
    public Vector2Int size = new Vector2Int(1, 1);
    public PlacementType placementType;
    public bool addOffset;
    public int health = 1;
    public bool nonDestructible;
    public bool hasAnimation = false;
    public RuntimeAnimatorController animatorController;
    public bool isIlluminated = false;
    public Color illuminationColor;
    public float illuminationIntensity = 1;
    public float illuminationInnerRadius = 0;
    public float illuminationOuterRadius = 1;
}
