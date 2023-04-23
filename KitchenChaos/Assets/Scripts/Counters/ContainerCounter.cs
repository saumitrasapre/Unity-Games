using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    //This counter can be used to spawn new things
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public event EventHandler OnPlayerGrabbedObject;

     public override void Interact(Player player)
    {
        //Directly spawn the associated kitchen object, and directly teleport it to the player's hand
        if (!player.HasKitchenObject())
        {
            //Spawn an object ONLY if the player doesn't already have something in his hand
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);

            if (OnPlayerGrabbedObject != null)
            {
                OnPlayerGrabbedObject(this, EventArgs.Empty);
            }
        }
    }

}
