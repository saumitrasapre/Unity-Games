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

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position+Vector3.up*playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            //Player cannot move freely on the map along moveDir (maybe there is a wall blocking the player)
            //So check if player can move only along X (left and right)
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                //Player can move only along the X axis.
                //Useful when a wall is blocking a forward path and you press W(or S) and A (or D) together.
                //Makes the player hug the wall and move along X axis
                moveDir = moveDirX;
            }
            else 
            {
                //Player cannot move freely on the map along X direction (maybe there is a wall blocking the sides of the player)
                //So check if player can move only along Z (front or back)
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    //Player can move only along the Z axis.
                    //Useful when a wall is blocking a side path and you press W (or S) and A (or D) together.
                    //Makes the player hug the wall and move along X axis
                    moveDir = moveDirZ;
                }
                else
                {
                    //Player cannot move at all in any direction
                }
            }

        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        float rotateSpeed = 10f;
        if (moveDir != Vector3.zero)
        {
            isPlayerWalking = true;
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        }
        else
        {
            isPlayerWalking = false;
        }

    }

    public bool getPlayerWalkingState()
    {
        return isPlayerWalking;
    }
}
