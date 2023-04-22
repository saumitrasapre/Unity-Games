using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    //Since a scriptable object(SO) is not a MonoBehaviour, we cannot directly attach the SO to the prefab
    //Hence, this is a connector script that just attaches the SO to the prefab
    //This script only has one function - to return the SO attached to this prefab
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private ClearCounter clearCounter;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetClearCounter(ClearCounter clearCounter)
    {
        if (this.clearCounter != null)
        {
            //If this object was formerly a part of another clear counter,
            //then we must delete this object's reference from that clear counter.
            this.clearCounter.ClearKitchenObject();
        }
        //Update the object to know which new clear counter it now belongs to
        this.clearCounter = clearCounter;

        if (clearCounter.HasKitchenObject())
        {
            //Error out if the new clear counter already has another object on it.
            // This should never happen!
            Debug.LogError("Counter already has a kitchen object on it!");
        }

        //Update the new clear counter to tell that this object is now on top of it
        clearCounter.SetKitchenObject(this);

        //Actually visually teleport the object on top of the new clear counter
        transform.parent = clearCounter.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public ClearCounter GetClearCounter()
    {
        return this.clearCounter;
    }
}
