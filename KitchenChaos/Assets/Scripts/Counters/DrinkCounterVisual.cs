using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkCounterVisual : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private DrinkCounter drinkCounter;
    private const string DISPENSE = "Dispense";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        drinkCounter.OnDrinkCounterStateChanged += DrinkCounter_OnDrinkCounterStateChanged;
    }

    private void DrinkCounter_OnDrinkCounterStateChanged(object sender, DrinkCounter.OnDrinkCounterStateChangedEventArgs e)
    {
        if (e.state == DrinkCounter.State.Dispensing)
        {
            animator.SetBool(DISPENSE, true);
        }
        else
        {
            animator.SetBool(DISPENSE, false);
        }
    }

    
}
