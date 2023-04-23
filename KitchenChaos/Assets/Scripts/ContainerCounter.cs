using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public event EventHandler OnPlayerGrabbedObject;

     public override void Interact(Player player)
    {
        //This counter directly spawns the associated kitchen object, and directly teleports it to the player
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);

        if (OnPlayerGrabbedObject != null)
        {
            OnPlayerGrabbedObject(this, EventArgs.Empty);
        }
    }

}
