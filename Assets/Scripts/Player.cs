using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{   
    [SerializeField]
    public float currSpeed;

    public bool isGhost = false;

    public float speedModifier = 1;

    [SerializeField]
    public int maxHealth;
    [SerializeField]
    public int currHealth;

    [SerializeField]
    public int damage;

    [SerializeField]
    public float punchDistance;

    private Vector2 movementInput;

    public bool isFarting;

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
    public Item item1;
    public GameObject myItemSlot1;
    public Sprite myItemSlot1Default;

    public Item item2;
    public GameObject myItemSlot2;
    public Sprite myItemSlot2Default;

    public Vector3 SandboxScreenCoordinates;

    Vector3 trailVectorPosition;


    public void Awake()
    {
 
        playerRigidBod = GetComponent<Rigidbody2D>();
        myUI = gameObject.transform.GetChild(0).gameObject;
        myFist = gameObject.transform.GetChild(1).gameObject;

        myHealthBar = myUI.transform.GetChild(2).gameObject.GetComponent<Image>();
        myReadyUpIcon = myUI.transform.GetChild(3).gameObject.GetComponent<Image>();
        myItemSlot1 = myUI.transform.GetChild(4).gameObject;
        myItemSlot2 = myUI.transform.GetChild(5).gameObject;

        // defualt sprites are duplicated
        myItemSlot1Default = myItemSlot1.GetComponent<Image>().sprite;
        myItemSlot2Default = myItemSlot2.GetComponent<Image>().sprite;

        myReadyUpIcon.color = Color.red;

        AssignColor(Random.ColorHSV(0f, 1f, 1f, 1f, 0.9f, 1f));
        myHealthBar.color = Color.green;

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

        GameManager.Instance.AddPlayer(this);
    }

    public void Update()
    {
        SandboxScreenCoordinates = FindObjectOfType<Camera>().WorldToScreenPoint(this.transform.position);

        playerRigidBod.velocity = new Vector3(movementInput.x, movementInput.y, 0) * (currSpeed * speedModifier);

        if (movementInput.x + movementInput.y != 0)
        {
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(-playerRigidBod.velocity.x, playerRigidBod.velocity.y) * Mathf.Rad2Deg, Vector3.forward);
        } else
        {
            playerRigidBod.angularVelocity = 0;
        }
    
        myUI.GetComponent<Transform>().transform.eulerAngles = new Vector3(0,0,0);

        if (myGameManager.State != GameState.CombatPhase && isFarting == true)
        {
            isFarting = false;
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
        this.isFarting = true;
    }

    public void Punch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            myFist.GetComponent<FistScript>().TriggerPunch();
        }
    }

    public void OnHit()
    {
        if (isFarting)
        {
            currHealth -= damage;
            UpdateHealthBar();

            StartCoroutine(DamageFlash());
            if (currHealth == 0)
            {
                gameObject.SetActive(false);
                myFist.SetActive(false);
                Destroy(trailRenderObject);
                GameTextManager.Instance.GameOver();
                GameManager.Instance.UpdateGameState(GameState.GameOver);
            }
        }
    }

    public void UpdateHealthBar()
    {
        myHealthBar.fillAmount = (float)currHealth / maxHealth;
        if (myHealthBar.fillAmount < 0.3) myHealthBar.color = Color.red;
        else if (myHealthBar.fillAmount < 0.6) myHealthBar.color = Color.yellow;
        else myHealthBar.color = Color.green;
    }

    // WE NEED TO SWITCH COMBAT FROM COLLISION TO ONTRIGGER
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Renderer>().material.color == GetComponent<Renderer>().material.color || isGhost)
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    public void AddItemToInventory(Item item)
    {

        // if it is empty add the item to the players inventory
  
        if (item1 == null)
        {
            Image itemSlotImage = myItemSlot1.GetComponent<Image>();
     
            item1 = item;
            // set the myItemSlot1 sprite
            if (myItemSlot1.GetComponent<Image>().sprite == myItemSlot1Default)
            {
                itemSlotImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
                itemSlotImage.color = item.GetComponent<SpriteRenderer>().color;
            }
        }

        else
        {
            item2 = item;
            if (myItemSlot2.GetComponent<Image>().sprite == myItemSlot2Default)
            {
                myItemSlot2.GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
                myItemSlot2.GetComponent<Image>().color = item.GetComponent<SpriteRenderer>().color;
            }
        }
        
    }


    public void UseItem1(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // if the inventory is not empty
            if (item1 == null)
            {
                return;
            }

            // calls the item payload
            item1.Activate();

            // sets the myItemSlot1 sprite to the default neutral slate
            ResetItem1();


        }
    }

    public void UseItem2(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // if the inventory is not empty
            if (item2 == null)
            {
                return;
            }

            // calls the item payload
            item2.Activate();

            // sets the myItemSlot1 sprite to the default neutral slate
            ResetItem2();
        }
    }

    public void ResetItem1()
    {
        myItemSlot1.GetComponent<Image>().sprite = myItemSlot1Default;
        myItemSlot1.GetComponent<Image>().color = Color.white;
        item1 = null;
    }

    public void ResetItem2()
    {
        myItemSlot2.GetComponent<Image>().sprite = myItemSlot2Default;
        myItemSlot2.GetComponent<Image>().color = Color.white;
        item2 = null;
    }

    public void DisableTrailSlow()
    {
        StartCoroutine(SlowTrailDisable());
    }

    IEnumerator SlowTrailDisable()
    {
        var trail = trailRenderObject.GetComponent<TrailRenderer>();
        float rate = trail.time / 50f;
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


