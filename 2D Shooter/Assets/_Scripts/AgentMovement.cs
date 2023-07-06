using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AgentMovement : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] protected float currentVelocity = 3f;
    protected Rigidbody2D rigidbody2d;
    [SerializeField] MovementDataSO movementData;
    protected Vector2 movementDirection;
    private bool isPlayerWalking;

    public static AgentMovement Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public bool getPlayerWalkingState()
    {
        return isPlayerWalking;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 movementInput = gameInput.GetMovementVectorNormalized();
        MoveAgent(movementInput);     
    }

    private void MoveAgent(Vector2 movementInput)
    {
        if (movementInput.magnitude > 0)
        {
            if (Vector2.Dot(movementInput, movementDirection) < 0)
            {
                currentVelocity = 0;
            }        
            movementDirection = movementInput;
        }
        currentVelocity = CalculateSpeed(movementInput);

        if (currentVelocity > 0)
        {
            isPlayerWalking = true;
        }
        else
        {
            isPlayerWalking = false;
        }
    }

    private void FixedUpdate()
    {
        rigidbody2d.velocity = movementDirection * currentVelocity;
    }

    private float CalculateSpeed(Vector2 movementDirection)
    {
        if (movementDirection.magnitude > 0)
        {
            currentVelocity += movementData.acceleration * Time.deltaTime;
        }
        else
        {
            currentVelocity -= movementData.deceleration * Time.deltaTime;
        }
        return Mathf.Clamp(currentVelocity, 0, movementData.maxSpeed);
    }

    public void StopImmediately()
    {
        currentVelocity = 0;
        rigidbody2d.velocity = Vector2.zero;
    }
}
