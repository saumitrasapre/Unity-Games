using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    //Event when the raycast thrown by the player hits a different counter 
    //than the one already highlighted
    public event EventHandler <OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        //The identity of the changed clear counter
        public BaseCounter selectedCounterPassedInEvent;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool isPlayerWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one player instance... This should not happen");
        }
        Instance = this;
    }
    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool getPlayerWalkingState()
    {
        return isPlayerWalking;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;

        if(Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance,countersLayerMask))
        {
            if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                //Object in front is a new Clear Counter
                if (selectedCounter != baseCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
                
            }
            else
            {
                //Object in front is not a clearCounter
                SetSelectedCounter(null);
            }
        }
        else
        {
            //No object in front
            SetSelectedCounter(null);
        }


    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        if (OnSelectedCounterChanged != null)
        {
            //Trigger the event for when the raycast thrown by the player hits a different counter 
            //than the one already highlighted
            OnSelectedCounterChanged(this, new OnSelectedCounterChangedEventArgs
            {
                selectedCounterPassedInEvent = selectedCounter
            });
        }
    }
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            //Player cannot move freely on the map along moveDir (maybe there is a wall blocking the player)
            //So check if player can move only along X (left and right)
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x!=0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

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
                canMove = moveDir.z !=0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

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

    public Transform GetKitchenObjectFollowTransform()
    {
        return this.kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return this.kitchenObject;
    }

    public void ClearKitchenObject()
    {
        this.kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return this.kitchenObject != null;
    }
}
