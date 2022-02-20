using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SpeedPotionItem : Item
{
    private float speedScale = 1.5f;

    private float speedPotionDuration = 3;

    private float speedPotionExpiration;

    protected override void ItemPayload()
    {
        base.ItemPayload();

        speedPotionExpiration = Time.time + speedPotionDuration;

        // Payload is to scale the fist
        playerUser.currSpeed = playerUser.currSpeed * speedScale;

    }

    protected override void ItemHasExpired()       // Checklist item 2
    {
        playerUser.currSpeed = playerUser.currSpeed / speedScale;
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
        if(itemState == ItemState.IsCollected && Time.time > speedPotionExpiration)
        {
            ItemHasExpired();
        }
    }
}
