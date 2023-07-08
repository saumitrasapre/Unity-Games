using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] protected float currentVelocity = 0f;
    protected Rigidbody2D rigidbody2d;
    [SerializeField] MovementDataSO movementData;
    protected Vector2 movementDirection;
    protected bool isKnockedBack = false;
    private bool isEnemyWalking;

    //public static EnemyMovement Instance { get; private set; }

    private void Awake()
    {
        //Instance = this;
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public bool getEnemyWalkingState()
    {
        return isEnemyWalking;
    }

    public void MoveAgent(Vector2 movementInput)
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
            isEnemyWalking = true;
        }
        else
        {
            isEnemyWalking = false;
        }
    }

    private void FixedUpdate()
    {
        if (isKnockedBack == false)
        {
            rigidbody2d.velocity = movementDirection * currentVelocity;
        }
        else
        {

        }
        
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

    public void KnockBack(Vector2 direction, float power, float duration)
    {
        if (isKnockedBack == false)
        {
            isKnockedBack = true;
            StartCoroutine(KnockBackCoroutine(direction, power, duration));
        }
    }

    public void ResetKnockBack()
    {
        StopAllCoroutines();
        StopCoroutine("KnockBackCoroutine");
        ResetKnockBackParameters();
    }

    IEnumerator KnockBackCoroutine(Vector2 direction, float power, float duration)
    {
        rigidbody2d.AddForce(direction.normalized * power, ForceMode2D.Impulse);
        yield return new WaitForSeconds(duration);
        ResetKnockBackParameters();
    }

    private void ResetKnockBackParameters()
    {
        currentVelocity = 0;
        rigidbody2d.velocity = Vector2.zero;
        isKnockedBack = false;
    }
}
