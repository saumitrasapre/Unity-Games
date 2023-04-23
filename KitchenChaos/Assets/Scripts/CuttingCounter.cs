using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //Counter doesn't have anything on top of it.
            if (player.HasKitchenObject())
            {
                //Player is holding a kitchen object
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    //If player is holding something that can be cut.
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    //Teleport the kitchen object from the player's hand to this counter.
                }

            }
            else
            {
                //Player has nothing in his hands
                //There is nothing to be done.
            }
        }
        else
        {
            //Counter has something already on top of it.
            if (player.HasKitchenObject())
            {
                //Player is holding a kitchen object
                //Don't do anything. Player cannot carry 2 items with him.
            }
            else
            {
                //Player has nothing in his hands
                this.GetKitchenObject().SetKitchenObjectParent(player);
                //Teleport the kitchen object from the kitchen counter to the player's hand.
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (this.HasKitchenObject() && HasRecipeWithInput(this.GetKitchenObject().GetKitchenObjectSO()))
        {
            //If this counter has a KitchenObject placed on top of it AND it can be cut
            //Replace the kitchen object with a sliced version of itself
            KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            this.GetKitchenObject().DestroySelf();
            //Spawn the sliced version of the kitchen object, and place it on top of the counter.
            KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
           
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return true;
            }
        }

        return false;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach(CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if(cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO.output;
            }
        }

        return null;
    }
}
