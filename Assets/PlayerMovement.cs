using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{   
    [SerializeField]
    private float speed;

    [SerializeField]
    private float rotationSpeed;

    private Vector2 movementInput;

    public void Update()
    {

        Vector2 movementDirection = new Vector2(movementInput.x, movementInput.y).normalized;
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        transform.Translate(movementDirection * speed * inputMagnitude * Time.deltaTime, Space.World);

        if (movementDirection != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();
}
