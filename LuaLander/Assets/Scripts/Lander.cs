using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour {
    
    private Rigidbody2D landerRigidBody2D;
    private void Awake() {
        landerRigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        if (Keyboard.current.upArrowKey.isPressed) {
            float force = 700f;
            landerRigidBody2D.AddForce(force * transform.up * Time.deltaTime);
        }
        if (Keyboard.current.leftArrowKey.isPressed) {
            float turnSpeed = 100f;
            landerRigidBody2D.AddTorque(turnSpeed * Time.deltaTime);
        }
        if (Keyboard.current.rightArrowKey.isPressed) {
            float turnSpeed = -100f;
            landerRigidBody2D.AddTorque(turnSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D) {
        float softLandingVelocityMagnitude = 4f;
        float minDotVector = 0.90f;
        if (collision2D.relativeVelocity.magnitude > softLandingVelocityMagnitude) {
            Debug.Log("Landed too hard!");
            return;
        }
        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        if (dotVector < minDotVector) {
            Debug.Log("Landed too steep!");
            return;
        }
        Debug.Log("Landed successfully!");
        
        
    }
}
