using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    [SerializeField]
    public float punchDistance;

    private Vector2 movementInput;
    private bool dashEnabled;
    private float dashExpiration;

    private float dashX;
    private float dashY;

    public bool hasFarted;

    private float dashCooldownExpiration;

    private Rigidbody2D playerRigidBod;

    private GameObject myFist;

    private GameObject myUI;

    private Image myHealthBar;

    private Image myDashIndicator;

    private GameManager myGameManager;

    private GameObject fart;

    public GameObject FistPrefab;

    public GameObject FartPrefab;

    private float fartScale;

    private int id;

    public Color playerColor;

    public TrailRenderer fartTrail;

    public void Awake()
    {
        playerColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.9f, 1f);
        playerRigidBod = GetComponent<Rigidbody2D>();

        myUI = gameObject.transform.GetChild(0).gameObject;
        myFist = Instantiate(
            FistPrefab,
            playerRigidBod.position,
            Quaternion.identity);

        myDashIndicator = myUI.transform.GetChild(0).gameObject.GetComponent<Image>();
        myHealthBar = myUI.transform.GetChild(1).gameObject.GetComponent<Image>();

        
        GetComponent<Renderer>().material.color = playerColor;
        myFist.GetComponent<Renderer>().material.color = playerColor;

        myGameManager = GameManager.Instance;

        // Get the fart trail;
        fartTrail = GetComponent<TrailRenderer>();
    }

    public void Start() {
        id = FindObjectsOfType<Player>().Length;
        Debug.Log("ID: " + id.ToString());

        GameManager.Instance.AddPlayer(this);
    }


    public void Dash() {
        if (Time.time > dashCooldownExpiration) {
            dashEnabled = true;
            dashExpiration = Time.time + dashDuration;
            dashCooldownExpiration = Time.time + dashCooldown;
            currSpeed = dashSpeed;
            dashX = movementInput.x;
            dashY = movementInput.y;
            StartCoroutine(DashImageLerp());
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

        if (fart) {
            fartScale += 0.001f;
            fart.transform.position = this.gameObject.transform.position;
            fart.transform.localScale = new Vector3(fartScale,fartScale,1);
        } else {
            fartScale = 0.2f;
        }
        // Position fist infront of player
        // reduendant get component transform
        myFist.GetComponent<Transform>().transform.position = this.gameObject.transform.position + 
                                                              (transform.up * 1.2f * myFist.GetComponent<FistScript>().currentPosition);
        myUI.GetComponent<Transform>().transform.eulerAngles = new Vector3(0,0,0);


        if (myGameManager.State != GameState.CombatPhase && hasFarted == true)
        {
            hasFarted = false;
        }

    }


    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();


    public void OnFart() {
        fartTrail.enabled = true;
        this.hasFarted = true;
    }
    public void Punch()
    {
        myFist.GetComponent<Collider2D>().enabled = true;
        myFist.GetComponent<FistScript>().PunchIt();
    }

    // WE NEED TO SWITCH COMBAT FROM COLLISION TO ONTRIGGER
    private void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log(collision.gameObject);
        if (collision.gameObject == myFist.gameObject)
        {
            return;
        }
        if (collision.gameObject.name == "FistPrefab(Clone)" && hasFarted)
        {
            health -= damage;
            myHealthBar.fillAmount -= 0.1f;
            if (health == 0)
            {
                GameTextManager.Instance.GameOver();
                GameManager.Instance.UpdateGameState(GameState.GameOver);
            }
        }
    }

    private IEnumerator DashImageLerp()
    {
        float startTime = Time.time;
        float timeElapsed = (Time.time - startTime) / dashCooldown;
        while(timeElapsed < 1f)
        {
            timeElapsed = (Time.time - startTime) / dashCooldown;
            myDashIndicator.fillAmount = Mathf.Lerp(1f, 0f, timeElapsed);
            yield return null;
        }
    }
}

