using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    //Static event, belongs to the TrashCounter class. This will be triggered when ANY ingredient gets trashed in a TrashCounter
    public static event EventHandler OnAnyObjectTrashed;
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            if (OnAnyObjectTrashed != null)
            {
                OnAnyObjectTrashed(this, EventArgs.Empty);
            }
        }
    }
}
