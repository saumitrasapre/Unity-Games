using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateKitchenObject : KitchenObject
{
    private List<KitchenObjectSO> kitchenObjectSOList;
    [SerializeField] private List<KitchenObjectSO> validkitchenObjectSOList;

    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs :EventArgs {
        public KitchenObjectSO ingredientAdded;
    };

    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!validkitchenObjectSOList.Contains(kitchenObjectSO))
        {
            //Not a valid ingredient
            return false;
        }
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            //List already contains this ingredient
            return false;
        }
        else
        {
            kitchenObjectSOList.Add(kitchenObjectSO);
            if (OnIngredientAdded != null)
            {
                OnIngredientAdded(this, new OnIngredientAddedEventArgs
                {
                    ingredientAdded = kitchenObjectSO
                });
            }
            return true;
        }
        
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return this.kitchenObjectSOList;
    }
}
