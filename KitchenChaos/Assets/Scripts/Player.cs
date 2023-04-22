using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;

    private bool isPlayerWalking;
    private void Update()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x,0,inputVector.y);
        float rotateSpeed = 10f;
        if (moveDir != Vector3.zero)
        {
            isPlayerWalking = true;
        }
        else
        {
            isPlayerWalking = false;
        }
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime*rotateSpeed);
        transform.position += moveDir * Time.deltaTime * moveSpeed;
    }

    public bool getPlayerWalkingState()
    {
        return isPlayerWalking;
    }
}
