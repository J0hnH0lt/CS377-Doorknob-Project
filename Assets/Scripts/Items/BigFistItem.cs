using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BigFistItem : Item
{
    private int fistScale = 2;

    private float bigFistDuration = 4;

    private float bigFistExperation;

    protected override void ItemPayload()
    {
        base.ItemPayload();

        bigFistExperation = Time.time + bigFistDuration;

        // Payload is to scale the fist
        //playerUser.myFist.transform.localScale = new Vector3(fistScale, fistScale, 1);
        playerReference.myFist.GetComponent<FistScript>().fistScaleMod += fistScale;

        itemState = ItemState.InEffect;
    }

    protected override void ItemHasExpired()       // Checklist item 2
    {
        //playerUser.myFist.transform.localScale = new Vector3(1, 1, 1);
        playerReference.myFist.GetComponent<FistScript>().fistScaleMod -= fistScale;

        base.ItemHasExpired();
    }

    public bool IsNotActive()
    {
        if (itemState == ItemState.Uncollected)
        {
            return true;
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if(itemState == ItemState.InEffect && Time.time > bigFistExperation)
        {
            ItemHasExpired();
        }
    }
}
