using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestingInputSystem : MonoBehaviour
{

    private Rigidbody sphereRigidBody;
    PlayerInputActions inputActions;
    private void Awake()
    {
        sphereRigidBody = GetComponent<Rigidbody>();
        inputActions =  new PlayerInputActions();

        //Enable player input action map before we can use the input action
        inputActions.Player.Enable();
        inputActions.Player.Jump.performed += Jump;
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = inputActions.Player.Movement.ReadValue<Vector2>();
        float speed = 1f;
        sphereRigidBody.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed, ForceMode.Force);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Jump!");
            sphereRigidBody.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
       
    }
}
