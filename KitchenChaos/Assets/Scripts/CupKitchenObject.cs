using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupKitchenObject : KitchenObject
{
    [SerializeField] Material drinkColor;

    public bool IsEmptyCup()
    {
        if (drinkColor == null)
        {
            return true;
        }
        return false;
    }

}
