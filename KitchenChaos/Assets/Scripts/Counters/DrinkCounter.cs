using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StoveCounter;

public class DrinkCounter : BaseCounter, IHasProgress
{
    //Drink counter spawns specific drinks only
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private float drinkDispenseTimerMax;

    private float drinkDispenseTimer;

    public enum State
    {
        Idle,
        Dispensing,
        Dispensed
    }

    private State state;

    public event EventHandler<OnDrinkCounterStateChangedEventArgs> OnDrinkCounterStateChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public class OnDrinkCounterStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (this.HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Dispensing:
                    drinkDispenseTimer += Time.deltaTime;
                    if (OnProgressChanged != null)
                    {
                        OnProgressChanged(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = drinkDispenseTimer / drinkDispenseTimerMax
                        });
                    }
                    if (drinkDispenseTimer > this.drinkDispenseTimerMax)
                    {
                        this.GetKitchenObject().DestroySelf();
                        //Drink dispensed
                        KitchenObject.SpawnKitchenObject(kitchenObjectSO, this);
                        drinkDispenseTimer = 0f;
                        this.state = State.Dispensed;
                        if (OnDrinkCounterStateChanged != null)
                        {
                            OnDrinkCounterStateChanged(this, new OnDrinkCounterStateChangedEventArgs
                            {
                                state = this.state
                            });
                        }

                    }
                    break;
                case State.Dispensed:
                    break;
            }
        }
    }



    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //Counter doesn't have anything on top of it.
            if (player.HasKitchenObject())
            {
                //If player is holding a kitchen object
                if (player.GetKitchenObject().TryGetCup(out CupKitchenObject cupKitchenObject))
                {
                    //Drink counter only accepts cups
                    if (cupKitchenObject.IsEmptyCup())
                    {
                        //Drink counter only accepts empty cups
                        //If player is holding an empty cup.
                        //Teleport the kitchen object from the player's hand to this counter.
                        player.GetKitchenObject().SetKitchenObjectParent(this);
                        this.drinkDispenseTimer = 0f;
                        this.state = State.Dispensing;

                        if (OnDrinkCounterStateChanged != null)
                        {
                            OnDrinkCounterStateChanged(this, new OnDrinkCounterStateChangedEventArgs
                            {
                                state = this.state
                            });
                        }

                        if (OnProgressChanged != null)
                        {
                            OnProgressChanged(this, new IHasProgress.OnProgressChangedEventArgs
                            {
                                progressNormalized = drinkDispenseTimer / drinkDispenseTimerMax
                            });
                        }
                    }
                }
                else
                {
                    //Player has nothing in his hands
                    //There is nothing to be done.
                }
            }
        }
        else
        {
            //Counter has something already on top of it.
            if (player.HasKitchenObject())
            {
                //Player is holding a kitchen object
                //Don't do anything. Player cannot carry 2 items with him.EXCEPT for when he has a plate with him
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //Player is holding a plate with him
                    if (plateKitchenObject.TryAddIngredient(this.GetKitchenObject().GetKitchenObjectSO()))
                    {
                        this.GetKitchenObject().DestroySelf();
                        this.state = State.Idle;
                        if (OnDrinkCounterStateChanged != null)
                        {
                            OnDrinkCounterStateChanged(this, new OnDrinkCounterStateChangedEventArgs
                            {
                                state = this.state
                            });
                        }
                        if (OnProgressChanged != null)
                        {
                            OnProgressChanged(this, new IHasProgress.OnProgressChangedEventArgs
                            {
                                progressNormalized = 0f
                            });
                        }
                    }
                }
            }
            else
            {
                //Player has nothing in his hands
                this.GetKitchenObject().SetKitchenObjectParent(player);
                //Teleport the kitchen object from the kitchen counter to the player's hand.
                this.state = State.Idle;
                if (OnDrinkCounterStateChanged != null)
                {
                    OnDrinkCounterStateChanged(this, new OnDrinkCounterStateChangedEventArgs
                    {
                        state = this.state
                    });
                }
                if (OnProgressChanged != null)
                {
                    OnProgressChanged(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = 0f
                    });
                }
            }
        }
    }
}
