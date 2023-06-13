using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupsCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO cupKitchenObjectSO;
    private float spawnCupTimer;
    private float spawnCupTimerMax = 4f;
    private int cupsSpawnedAmount;
    private int cupsSpawnedAmountMax = 4;
    public event EventHandler OnCupSpawned;
    public event EventHandler OnCupRemoved;

    private void Update()
    {
        spawnCupTimer += Time.deltaTime;
        if (spawnCupTimer > spawnCupTimerMax)
        {
            spawnCupTimer = 0f;

            if (GameManager.Instance.IsGamePlaying() && cupsSpawnedAmount < cupsSpawnedAmountMax)
            {
                cupsSpawnedAmount++;
                if (OnCupSpawned != null)
                {
                    OnCupSpawned(this, EventArgs.Empty);
                }
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            //Player is not holding anything in his hands
            if (cupsSpawnedAmount > 0)
            {
                //There is at lease one cup available to be given to the player
                cupsSpawnedAmount--;
                //Spawn the kitchen object directly into the player's hands
                KitchenObject.SpawnKitchenObject(cupKitchenObjectSO, player);
                if (OnCupRemoved != null)
                {
                    OnCupRemoved(this, EventArgs.Empty);
                }
            }

        }
    }
}
