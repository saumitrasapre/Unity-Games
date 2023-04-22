using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private ClearCounter clearCounter;
    [SerializeField] private GameObject visualGameObject;

    private void Start()
    {
        //Subscribe to the event when the raycast thrown by the player hits a different counter 
        //than the one already highlighted
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        //Get the identity of the changed counter, and update the visual of only that counter
        if (e.selectedCounterPassedInEvent == clearCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
        
    }

    private void Show()
    {
        visualGameObject.SetActive(true);
    }

    private void Hide()
    {
        visualGameObject.SetActive(false);
    }
}
