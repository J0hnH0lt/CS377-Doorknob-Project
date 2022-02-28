using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Parent class for Items
public class Item : MonoBehaviour
{
    // name of the item
    public string itemName;

    // sandbox trackers
    public bool isSandBox;
    private float sandboxItemInterval = 1.0f;

    // item description
    public string itemDescription;

    // the player who picks up the item
    protected Player playerReference;

    // the sprite for the item
    protected SpriteRenderer spriteRenderer;

    // the item's state (uncollected, inventroy, etc).
    public ItemState itemState;

    public void Activate()
    {
        ItemPayload();
    }

    protected virtual void Awake ()
    {
        // get the sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();

        // if it is a sandbox item, set the timer inverval at which it can be collected
        if (isSandBox)
        {
            sandboxItemInterval = Time.time + sandboxItemInterval;
        }
        
    }

    protected virtual void Start ()
    {
        itemState = ItemState.Uncollected;
    }

    // When a player collects the item
    protected virtual void OnTriggerEnter2D (Collider2D other)
    {
        //Debug.Log("Item picked up");
        ItemCollected (other.gameObject);
    }

    // Function to run on item collection
    protected virtual void ItemCollected (GameObject gameObjectCollectingItem)
    {
        // if it is a sandbox item, check to see if it can be collected
        if (isSandBox && Time.time < sandboxItemInterval)
        {
            return;
        }


        // If the game object collecting the item is not a palyer
        if (gameObjectCollectingItem.tag != "Player")
        {
            return;
        }

        Player p= gameObjectCollectingItem.GetComponent<Player>();

        // If the game object has already been picked up, we ignore it
        // (Note: after pickup the gameObject of the item persists, but with no renderer)
   
        if (itemState == ItemState.InInventory || itemState == ItemState.InEffect || (p.item1!=null && p.item2!=null))
        {
            return;
        }

        // if the item is a sandbox item, spawn a direct copy of that item
        if (isSandBox)
        {
            Debug.Log("Spawning new instance of item");
            Instantiate(this, this.transform.parent, true);
        }

        // Assign item palyer reference 
        playerReference = p;

        // Add item to player inventory
        playerReference.AddItemToInventory(this);
        itemState = ItemState.InInventory;

        // We move the power up game object to be under the player that collect it, this isn't essential for functionality 
        // presented so far, but it is neater in the gameObject hierarchy
        gameObject.transform.parent = playerReference.gameObject.transform;
        gameObject.transform.position = playerReference.gameObject.transform.position;

        // Turn of game object sprite renderer
        spriteRenderer.enabled = false;
    }

   
    // The item payload (the actual code that an item implements) is implemented in the child class
    protected virtual void ItemPayload ()
    {
        Debug.Log ("Power Up collected, issuing payload for: " + gameObject.name);
    }


    // Function to run when the item has been consumed (destroys the item)
    protected virtual void ItemHasExpired ()
    {
        Destroy(gameObject);
    }

}

// Enumerable of the possible item states
public enum ItemState
{
    Uncollected, //  the item is on the map
    InInventory, // the item is in the player inventory
    InEffect // the item has been consumed by the player
}

