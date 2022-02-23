﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Parent class for Items
public class Item : MonoBehaviour
{
    // name of the item
    public string itemName;

    // item description
    public string itemDescription;

    // the player who picks up the item
    protected Player playerUser;

    // the sprite for the item
    protected SpriteRenderer spriteRenderer;

    // the item's state (uncollected, inventroy, etc).
    protected ItemState itemState;

    protected virtual void Awake ()
    {
        // get the sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer> ();
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
        //Debug.Log("Collected item: " + gameObjectCollectingItem.tag);

        // If the game object collecting the item is not a palyer
        if (gameObjectCollectingItem.tag != "Player")
        {
            return;
        }

        // If the game object has already been picked up, we ignore it
        // (Note: after pickup the gameObject of the item persists, but with no renderer)
        // TODO MAKE SURE THE PLAYER HAS SPACE IN THEIR INVENTORY
        // TODO PLAYER INVENTORY SHOULD BE ABLE TO HANDLE DROPPING OF ITEMS
        if (itemState == ItemState.InInventory || itemState == ItemState.Expriring)// || gameOBjectCollectingItem.Inventory != full)
        {
            return;
        }

        // Add item to player inventory
        itemState = ItemState.InInventory;

        // Assign item palyer reference   
        playerUser = gameObjectCollectingItem.GetComponent<Player> ();

        // Put the item into the inventory
        // TODO WE SHOULD CREATE THIS SO IT IS AN INVENTORY
        // TODO IN PLAYER --> INVENTORY SHOULD HANDLE THE LOGIC OF ITEM SLOTS
        playerUser.SetItem1(spriteRenderer.sprite);

        // We move the power up game object to be under the player that collect it, this isn't essential for functionality 
        // presented so far, but it is neater in the gameObject hierarchy
        gameObject.transform.parent = playerUser.gameObject.transform;
        gameObject.transform.position = playerUser.gameObject.transform.position;

        // TODODO hehe --> move this to when player has actived their item      
        ItemPayload();

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
        if (itemState == ItemState.Expriring)
        {
            return;
        }

        itemState = ItemState.Expriring;

        Destroy(gameObject);

    }

}

// Enumerable of the possible item states
public enum ItemState
{
    Uncollected, //  the item is on the map
    InInventory, // the item is in the player inventory
    Expriring // the item has been consumed by the player
}

