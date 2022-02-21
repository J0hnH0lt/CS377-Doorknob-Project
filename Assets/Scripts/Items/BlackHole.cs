using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BlackHole : Item
{
    private int blackHoleRadius;

    private float blackHoleDuration = 7;

    private float blackHoleExpiration;

    protected override void ItemPayload()
    {
        base.ItemPayload();

        blackHoleExpiration = Time.time + blackHoleDuration;

        BlackHoleEffect();

    }

    protected override void ItemHasExpired()       // Checklist item 2
    {
        playerUser.myFist.transform.localScale = new Vector3(1, 1, 1);
        base.ItemHasExpired();
    }

    public bool IsNotActive()
    {
        if (itemState == ItemState.InAttractMode)
        {
            return true;
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if(itemState == ItemState.IsCollected && Time.time > blackHoleExpiration)
        {
            ItemHasExpired();
        }
    }

    public void BlackHoleEffect()
    {
        Collider2D[] collidersInRadius = Physics2D.OverlapCircleAll(playerUser.transform.position, 3f);
        foreach (Collider2D collider in collidersInRadius)
        {
            if (collider.tag == "Player")
            {

                var directionOfPlayerFromPlayer = (transform.position - collider.gameObject.transform.position).normalized;



                // Adds the force towards the center
                collider.GetComponent<Rigidbody2D>().AddForce(directionOfPlayerFromPlayer * 3);
                // Payload is to scale the fist

            }
        }
    }
}
