using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    //This counter can be used to place things on top of it.
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //Counter doesn't have anything on top of it.
            if (player.HasKitchenObject())
            {
                //Player is holding a kitchen object
                player.GetKitchenObject().SetKitchenObjectParent(this);
                //Teleport the kitchen object from the player's hand to this counter.
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
                //Don't do anything. Player cannot carry 2 items with him. EXCEPT for when he has a plate with him
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //Player is holding a plate with him
                    if (plateKitchenObject.TryAddIngredient(this.GetKitchenObject().GetKitchenObjectSO()))
                    {
                        //Transfer object from counter to plate
                        this.GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    //Player is not holding a plate with him, but has something else with him
                    if (this.GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        //Counter is holding a plate on top of it
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            //Transfer object from player to plate
                            player.GetKitchenObject().DestroySelf();
                        }
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

}
