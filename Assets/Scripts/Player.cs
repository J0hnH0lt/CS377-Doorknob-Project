using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{   
    [SerializeField]
    public float currSpeed;

    public float speedModifier = 1;

    [SerializeField]
    public int health;

    [SerializeField]
    public int damage;

    [SerializeField]
    public float punchDistance;

    private Vector2 movementInput;

    public bool hasFarted;

    private Rigidbody2D playerRigidBod;

    public GameObject myFist;

    private GameObject myUI;

    private Image myHealthBar;

    private GameManager myGameManager;

    public GameObject FistPrefab;

    public GameObject FartPrefab;

    public Image myReadyUpIcon;

    private int id;

    public Color playerColor;

    // FART TRAIL STUFFS

    public GameObject trailRendererObjectPrefab;

    public GameObject trailRenderObject;

    public bool fartTrailActive;

    // is the player ready

    public bool isReady = false;

    // item
    public List<Item> Inventory;
    public GameObject myItemSlot1;
    public Sprite myItemSlot1Default;

    Vector3 trailVectorPosition;


    public void Awake()
    {
 
        playerRigidBod = GetComponent<Rigidbody2D>();
        myUI = gameObject.transform.GetChild(0).gameObject;
        myFist = Instantiate(
            FistPrefab,
            playerRigidBod.position,
            Quaternion.identity);

        myHealthBar = myUI.transform.GetChild(1).gameObject.GetComponent<Image>();
        myReadyUpIcon = myUI.transform.GetChild(2).gameObject.GetComponent<Image>();
        myItemSlot1 = myUI.transform.GetChild(3).gameObject;
        myItemSlot1Default = myItemSlot1.GetComponent<Image>().sprite;

        myReadyUpIcon.color = Color.red;

        AssignColor(Random.ColorHSV(0f, 1f, 1f, 1f, 0.9f, 1f));

        myGameManager = GameManager.Instance;
    }

    public void SwapColor(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (myGameManager.State == GameState.Menu)
            {
                AssignColor(Random.ColorHSV(0f, 1f, 1f, 1f, 0.9f, 1f));
            }
        }
    }

    private void AssignColor(Color c)
    {
        playerColor = c;
        GetComponent<Renderer>().material.color = playerColor;
        myFist.GetComponent<Renderer>().material.color = playerColor;
    }

    public void ToggleReadyUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (myGameManager.State == GameState.Menu)
            {
                isReady = !isReady;
                if(isReady)
                {
                    myReadyUpIcon.color = Color.green;
                } else
                {
                    myReadyUpIcon.color = Color.red;
                }
            
            }
        }
    }

    public void Start() {
        id = FindObjectsOfType<Player>().Length;
        Debug.Log("ID: " + id.ToString());

        GameManager.Instance.AddPlayer(this);
    }

    public void Update()
    {

        playerRigidBod.velocity = new Vector3(movementInput.x, movementInput.y, 0) * (currSpeed * speedModifier);

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

    public void Punch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            myFist.GetComponent<Collider2D>().enabled = true;
            myFist.GetComponent<FistScript>().PunchIt();
        }
    }

    // WE NEED TO SWITCH COMBAT FROM COLLISION TO ONTRIGGER
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Renderer>().material.color == GetComponent<Renderer>().material.color)
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        if (collision.gameObject == myFist.gameObject)
        {
            return;
        }
        if (collision.gameObject.name == "FistPrefab(Clone)" && hasFarted)
        {
            health -= damage;
            myHealthBar.fillAmount -= 0.1f;
            StartCoroutine(DamageFlash());
            if (health == 0)
            {
                GameTextManager.Instance.GameOver();
                GameManager.Instance.UpdateGameState(GameState.GameOver);
            }
        }
    }

    public void AddItemToInventory(Item item)
    {
        // if it is empty add the item to the players inventory
        Inventory.Add(item);
   
        // set the myItemSlot1 sprite
        myItemSlot1.GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
        myItemSlot1.GetComponent<Image>().color = item.GetComponent<SpriteRenderer>().color;
    }


    public void UseItem(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // if the inventory is not empty
            if (Inventory.Count == 0)
            {
                return;
            }

            // removes the item from the players inventory
            Item item = Inventory[0];
            Inventory.RemoveAt(0);


            // sets the myItemSlot1 sprite to the default neutral slate
            ResetItem1();


            // calls the item payload
            item.Activate();
        }
    }

    public void SetItem1(Sprite itemSprite, Color spriteColor)
    {
        myItemSlot1.GetComponent<Image>().sprite = itemSprite;
        myItemSlot1.GetComponent<Image>().color = spriteColor;
    }

    public void ResetItem1()
    {
        myItemSlot1.GetComponent<Image>().sprite = myItemSlot1Default;
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

    IEnumerator DamageFlash()
    {
        GetComponent<Renderer>().material.color = Color.red;
        myFist.GetComponent<Renderer>().material.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        GetComponent<Renderer>().material.color = playerColor;
        myFist.GetComponent<Renderer>().material.color = playerColor;
    }


}


