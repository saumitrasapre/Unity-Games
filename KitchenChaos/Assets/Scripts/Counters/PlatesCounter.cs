using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatesCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;

            if (GameManager.Instance.IsGamePlaying() && platesSpawnedAmount < platesSpawnedAmountMax)
            {
                platesSpawnedAmount++;
                if (OnPlateSpawned != null)
                {
                    OnPlateSpawned(this, EventArgs.Empty);
                }
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            //Player is not holding anything in his hands
            if (platesSpawnedAmount > 0)
            {
                //There is at lease one plate available to be given to the player
                platesSpawnedAmount--;
                //Spawn the kitchen object directly into the player's hands
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                if (OnPlateRemoved != null)
                {
                    OnPlateRemoved(this, EventArgs.Empty);
                }
            }

        }
    }
}
