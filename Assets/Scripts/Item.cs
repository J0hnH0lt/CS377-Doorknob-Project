using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Game independent Power Up logic supporting 2D and 3D modes.
/// When collected, a Power Up has visuals switched off, but the Power Up gameobject exists until it is time for it to expire
/// Subclasses of this must:
/// 1. Implement ItemPayload()
/// 2. Optionally Implement ItemHasExpired() to remove what was given in the payload
/// 3. Call ItemHasExpired() when the power up has expired or tick ExpiresImmediately in inspector
/// </summary>
public class Item : MonoBehaviour
{
    public string itemName;
    public string itemExplanation;
    public string itemQuote;
    [Tooltip ("Tick true for power ups that are instant use, eg a health addition that has no delay before expiring")]
    public bool expiresImmediately;
    public GameObject specialEffect;
    public AudioClip soundEffect;

    /// <summary>
    /// It is handy to keep a reference to the player that collected us
    /// </summary>
    protected Player playerUser;

    protected SpriteRenderer spriteRenderer;

    protected enum ItemState
    {
        InAttractMode,
        IsCollected,
        IsExpiring
    }

    protected ItemState itemState;

    protected virtual void Awake ()
    {
        spriteRenderer = GetComponent<SpriteRenderer> ();
    }

    protected virtual void Start ()
    {
        itemState = ItemState.InAttractMode;
    }

    /// <summary>
    /// 2D support
    /// </summary>
    protected virtual void OnTriggerEnter2D (Collider2D other)
    {
        Debug.Log("TRIGGERED");
        ItemCollected (other.gameObject);
    }

    /// <summary>
    /// 3D support
    /// </summary>
    protected virtual void OnTriggerEnter (Collider other)
    {
        Debug.Log("ENTERED");
        ItemCollected (other.gameObject);
    }

    protected virtual void ItemCollected (GameObject gameObjectCollectingItem)
    {
        Debug.Log("COLLECTED" + gameObjectCollectingItem.tag);

        // We only care if we've been collected by the player
        if (gameObjectCollectingItem.tag != "Player")
        {
            return;
        }

        // We only care if we've not been collected before
        if (itemState == ItemState.IsCollected || itemState == ItemState.IsExpiring)
        {
            return;
        }
        itemState = ItemState.IsCollected;

        // We must have been collected by a player, store handle to player for later use      
        playerUser = gameObjectCollectingItem.GetComponent<Player> ();

        // We move the power up game object to be under the player that collect it, this isn't essential for functionality 
        // presented so far, but it is neater in the gameObject hierarchy
        gameObject.transform.parent = playerUser.gameObject.transform;
        gameObject.transform.position = playerUser.gameObject.transform.position;

        //// Collection effects
        //ItemEffects ();           

        // Payload      
        ItemPayload ();

        // // Send message to any listeners
        // foreach (GameObject go in EventSystemListeners.main.listeners)
        // {
        //     ExecuteEvents.Execute<IItemEvents> (go, null, (x, y) => x.OnItemCollected (this, playerBrain));
        // }

        // Now the power up visuals can go away
        spriteRenderer.enabled = false;
    }

    //protected virtual void ItemEffects ()
    //{
    //    if (specialEffect != null)
    //    {
    //        Instantiate (specialEffect, transform.position, transform.rotation, transform);
    //    }

    //    if (soundEffect != null)
    //    {
    //        MainGameController.main.PlaySound (soundEffect);
    //    }
    //}

    protected virtual void ItemPayload ()
    {
        Debug.Log ("Power Up collected, issuing payload for: " + gameObject.name);

        // If we're instant use we also expire self immediately
        if (expiresImmediately)
        {
            ItemHasExpired ();
        }
    }

    protected virtual void ItemHasExpired ()
    {
        if (itemState == ItemState.IsExpiring)
        {
            return;
        }
        itemState = ItemState.IsExpiring;

        //// Send message to any listeners
        //foreach (GameObject go in EventSystemListeners.main.listeners)
        //{
        //    ExecuteEvents.Execute<IItemEvents> (go, null, (x, y) => x.OnItemExpired (this, playerBrain));
        //}
        Debug.Log ("Power Up has expired, removing after a delay for: " + gameObject.name);
        DestroySelfAfterDelay ();
    }

    protected virtual void DestroySelfAfterDelay ()
    {
        // Arbitrary delay of some seconds to allow particle, audio is all done
        // TODO could tighten this and inspect the sfx? Hard to know how many, as subclasses could have spawned their own
        Destroy (gameObject, 10f);
    }
}

