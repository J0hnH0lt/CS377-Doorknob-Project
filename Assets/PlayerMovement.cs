using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{   
    [SerializeField]
    private float normalSpeed = 5;

    [SerializeField]
    private float dashSpeed = 50;

    [SerializeField]
    private float dashDuration = 0.2f;

    [SerializeField]
    private float dashCooldown = 2f;

    [SerializeField]
    private float rotationSpeed = 1000;

    private float currSpeed = 5;

    private Vector2 movementInput;
    private bool dashEnabled;
    private float dashExpiration;

    private float dashX;
    private float dashY;

    private float dashCooldownExpiration;

    public void Dash() {
        if (Time.time > dashCooldownExpiration) {
            dashEnabled = true;
            dashExpiration = Time.time + dashDuration;
            dashCooldownExpiration = Time.time + dashCooldown;
            currSpeed = dashSpeed;
            dashX = movementInput.x;
            dashY = movementInput.y;
        }
    }

    public void UpdateDash() {
        if (Time.time > dashExpiration) {
            dashEnabled = false;
            currSpeed = normalSpeed;
        }
    }

    public void Update()
    {
        Vector2 movementDirection;

        if (dashEnabled) {
            movementDirection = new Vector2(dashX, dashY).normalized;
            UpdateDash();
        } else {
            movementDirection = new Vector2(movementInput.x, movementInput.y).normalized;
        }

        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        transform.Translate(movementDirection * currSpeed * inputMagnitude * Time.deltaTime, Space.World);

        if (movementDirection != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();
}
