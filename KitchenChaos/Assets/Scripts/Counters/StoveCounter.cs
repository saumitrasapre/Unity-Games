using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    public event EventHandler<OnStoveStateChangedEventArgs> OnStoveStateChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public class OnStoveStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    private State state;

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
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    if (OnProgressChanged != null)
                    {
                        OnProgressChanged(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                        });
                    }
                    if (fryingTimer > this.fryingRecipeSO.fryingTimerMax)
                    {
                        this.GetKitchenObject().DestroySelf();
                        //Fried
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        this.burningRecipeSO = GetBurningRecipeSOWithInput(this.GetKitchenObject().GetKitchenObjectSO());
                        burningTimer = 0f;
                        this.state = State.Fried;
                        if (OnStoveStateChanged != null)
                        {
                            OnStoveStateChanged(this, new OnStoveStateChangedEventArgs
                            {
                                state = this.state
                            });
                        }

                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;

                    if (OnProgressChanged != null)
                    {
                        OnProgressChanged(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                        }) ;
                    }
                    if (burningTimer > this.burningRecipeSO.burningTimerMax)
                    {
                        this.GetKitchenObject().DestroySelf();
                        //Burned
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        this.state = State.Burned;
                        if (OnStoveStateChanged != null)
                        {
                            OnStoveStateChanged(this, new OnStoveStateChangedEventArgs
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
                    break;
                case State.Burned:
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
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    //If player is holding something that can be fried.
                    //Teleport the kitchen object from the player's hand to this counter.
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    this.fryingRecipeSO = GetFryingRecipeSOWithInput(this.GetKitchenObject().GetKitchenObjectSO());
                    this.fryingTimer = 0f;
                    this.state = State.Frying;
                    if (OnStoveStateChanged != null)
                    {
                        OnStoveStateChanged(this, new OnStoveStateChangedEventArgs
                        {
                            state = this.state
                        });
                    }

                    if (OnProgressChanged != null)
                    {
                        OnProgressChanged(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
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
                        if (OnStoveStateChanged != null)
                        {
                            OnStoveStateChanged(this, new OnStoveStateChangedEventArgs
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
                if (OnStoveStateChanged != null)
                {
                    OnStoveStateChanged(this, new OnStoveStateChangedEventArgs
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

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
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
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }

        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }

        return null;
    }

    public bool IsFried()
    {
        return state == State.Fried;
    }
}
