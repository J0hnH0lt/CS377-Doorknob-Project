using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{   
    [SerializeField]
    private float normalSpeed;

    [SerializeField]
    private float dashSpeed;

    [SerializeField]
    private float dashDuration;

    [SerializeField]
    private float dashCooldown;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float currSpeed;

    [SerializeField]
    public int health;

    [SerializeField]
    public int damage;

    private Vector2 movementInput;
    private bool dashEnabled;
    private float dashExpiration;

    private float dashX;
    private float dashY;

    private float dashCooldownExpiration;

    private Rigidbody2D playerRigidBod;

    private GameObject fist;

    public GameObject FistPrefab;

    public void Start() {
        GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        playerRigidBod = GetComponent<Rigidbody2D>();

        GameManager.Instance.AddPlayer(this);

        Vector2 vectorCast = transform.up * 2;
        fist = Instantiate(
            FistPrefab,
            playerRigidBod.position + vectorCast,
            Quaternion.identity);
    }


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

        fist.transform.position = this.gameObject.transform.position + (transform.up * 2);
    }

    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();

    public void Punch()
    {
        Debug.Log("Punch!");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "FistPrefab(Clone)")
        {
            health -= damage;
            Debug.Log("AH I HAVE BEEN HIT for " + damage + " I only have " + health + " left");
        }
        //collision.otherCollider.health -= damage;
    }
}

