using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{   
    [SerializeField]
    public float currSpeed;

    [SerializeField]
    public int health;

    [SerializeField]
    public int damage;

    [SerializeField]
    public float punchDistance;

    private Vector2 movementInput;

    public bool hasFarted;

    private Rigidbody2D playerRigidBod;

    private GameObject myFist;

    private GameObject myUI;

    private Image myHealthBar;

    private GameManager myGameManager;

    public GameObject FistPrefab;

    public GameObject FartPrefab;

    private int id;

    public Color playerColor;

    // FART TRAIL STUFFS

    public GameObject trailRendererObjectPrefab;

    public GameObject trailRenderObject;

    public bool fartTrailActive;

    Vector3 trailVectorPosition;


    public void Awake()
    {
        playerColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.9f, 1f);
        playerRigidBod = GetComponent<Rigidbody2D>();

        myUI = gameObject.transform.GetChild(0).gameObject;
        myFist = Instantiate(
            FistPrefab,
            playerRigidBod.position,
            Quaternion.identity);

        myHealthBar = myUI.transform.GetChild(1).gameObject.GetComponent<Image>();

        
        GetComponent<Renderer>().material.color = playerColor;
        myFist.GetComponent<Renderer>().material.color = playerColor;

        myGameManager = GameManager.Instance;
    }

    public void Start() {
        id = FindObjectsOfType<Player>().Length;
        Debug.Log("ID: " + id.ToString());

        GameManager.Instance.AddPlayer(this);
    }

    public void Update()
    {

        playerRigidBod.velocity = new Vector3(movementInput.x, movementInput.y, 0) * currSpeed;

        if (movementInput.x + movementInput.y != 0)
        {
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(-playerRigidBod.velocity.x, playerRigidBod.velocity.y) * Mathf.Rad2Deg, Vector3.forward);
        } else
        {
            playerRigidBod.angularVelocity = 0;
        }
    

        myFist.GetComponent<Transform>().transform.position = this.gameObject.transform.position + 
                                                              (transform.up * 1.2f * myFist.GetComponent<FistScript>().currentPosition);
        myUI.GetComponent<Transform>().transform.eulerAngles = new Vector3(0,0,0);


        if (myGameManager.State != GameState.CombatPhase && hasFarted == true)
        {
            hasFarted = false;
        }

        if (fartTrailActive == true)
        {
            trailVectorPosition = gameObject.transform.position;
            trailRenderObject.transform.position = trailVectorPosition;
        }

    }

    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();

    public void OnFart() {
        fartTrailActive = true;
        trailRenderObject = Instantiate(trailRendererObjectPrefab, playerRigidBod.position, Quaternion.identity);
        trailVectorPosition = gameObject.transform.position;
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
            health -= damage;
            myHealthBar.fillAmount -= 0.1f;
            if (health == 0)
            {
                GameTextManager.Instance.GameOver();
                GameManager.Instance.UpdateGameState(GameState.GameOver);
            }
        }
    }

    public void DisableTrailSlow()
    {
        StartCoroutine(SlowTrailDisable());
    }

    IEnumerator SlowTrailDisable()
    {
        var trail = trailRenderObject.GetComponent<TrailRenderer>();
        float rate = trail.time / 30f;
        while (trail.time > 0)
        {
            trail.time -= rate;
            yield return 0;
        }
        fartTrailActive = false;
        Destroy(trailRenderObject);
    }
}


