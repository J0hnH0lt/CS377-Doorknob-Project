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

    public GameObject myHealthBar;

    public GameObject HealthBarPrefab;

    public GameObject FistPrefab;

    private GameManager myGameManager;

    private GameObject fart;

    public GameObject FartPrefab;

    private float fartScale;

    private int id;

    public Color playerColor;

    public void Awake()
    {
        playerColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.9f, 1f);
        playerRigidBod = GetComponent<Rigidbody2D>();

        Vector2 vectorCast = transform.up;

        myFist = Instantiate(
            FistPrefab,
            playerRigidBod.position + vectorCast,
            Quaternion.identity);
        myFist.GetComponent<Renderer>().material.color = playerColor;

        myHealthBar = Instantiate(
           HealthBarPrefab,
           playerRigidBod.position,
           transform.rotation);

        myHealthBar.GetComponent<Transform>().transform.position = this.gameObject.transform.position + Vector3.down * 2f;

        
        GetComponent<Renderer>().material.color = playerColor;

        GetComponentInChildren<Image>().fillAmount = 1f;


        myGameManager = GameManager.Instance;
    }

    public void Start() {
        id = FindObjectsOfType<Player>().Length;
        Debug.Log("ID: " + id.ToString());

        GameManager.Instance.AddPlayer(this);
        ScoreManager.Instance.AddPlayer(this.id, this.playerColor, this.health);

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
                                                              (transform.up * myFist.GetComponent<FistScript>().currentPosition);


        myHealthBar.GetComponent<Transform>().transform.position = this.gameObject.transform.position + Vector3.down * 2f;


        if (myGameManager.State != GameState.CombatPhase && hasFarted == true)
        {
            hasFarted = false;
        }

    }


    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();


    public void OnFart() {
        fart = Instantiate(
            FartPrefab,
            playerRigidBod.position,
            Quaternion.identity);
        fart.transform.localScale = new Vector3(fartScale,fartScale,1);
        Destroy(fart,4f);
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
        if (collision.gameObject == myFist.gameObject)
        {
            return;
        }
        if (collision.gameObject.name == "FistPrefab(Clone)" && hasFarted)
        {
            Debug.Log("Punch");
            health -= damage;
            if (health <= 0)
            {
       
                ScoreManager.Instance.GameOver();
                GameManager.Instance.UpdateGameState(GameState.GameOver);
            }
            else
            {
                ScoreManager.Instance.updatePlayerHealth(this.id, this.health);
            }
        }
    }

    private IEnumerator DashImageLerp()
    {
        Image fillImage = GetComponentInChildren<Image>();
        float startTime = Time.time;
        float timeElapsed = (Time.time - startTime) / dashCooldown;
        while(timeElapsed < 1f)
        {
            timeElapsed = (Time.time - startTime) / dashCooldown;
            fillImage.fillAmount = Mathf.Lerp(0f, 1f, timeElapsed);
            yield return null;
        }
    }
}

