using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    private int cuttingProgress;
    public event EventHandler OnCut;
    public event EventHandler <IHasProgress.OnProgressChangedEventArgs>OnProgressChanged;
    //Static event, belongs to the CuttingCounter class. This will be triggered when ANY cutting counter cuts the ingredient
    public static event EventHandler OnAnyCut; 
    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //Counter doesn't have anything on top of it.
            if (player.HasKitchenObject())
            {
                //If player is holding a kitchen object
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    //If player is holding something that can be cut.
                    //Teleport the kitchen object from the player's hand to this counter.
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(this.GetKitchenObject().GetKitchenObjectSO());
                    //Trigger the Progress changed event
                    if (OnProgressChanged != null)
                    {
                        OnProgressChanged(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                        }) ;
                    }
                    
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
                //Don't do anything. Player cannot carry 2 items with him EXCEPT for when he has a plate with him
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //Player is holding a plate with him
                    if (plateKitchenObject.TryAddIngredient(this.GetKitchenObject().GetKitchenObjectSO()))
                    {
                        this.GetKitchenObject().DestroySelf();
                    }

                }
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
            cuttingProgress++;
            if (OnCut != null)
            {
                OnCut(this, EventArgs.Empty);
            }
            if (OnAnyCut != null)
            {
                OnAnyCut(this, EventArgs.Empty);
            }
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(this.GetKitchenObject().GetKitchenObjectSO());
            if (OnProgressChanged != null)
            {
                OnProgressChanged(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                });
            }
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                this.GetKitchenObject().DestroySelf();
                //Spawn the sliced version of the kitchen object, and place it on top of the counter.
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
           
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSO != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }

        return null;
    }
}
