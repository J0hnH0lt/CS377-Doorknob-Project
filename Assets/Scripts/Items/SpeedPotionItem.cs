using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SpeedPotionItem : Item
{
    private float speedScale = .35f;

    private float speedPotionDuration = 3;

    private float speedPotionExpiration;

    protected override void ItemPayload()
    {
        base.ItemPayload();

        speedPotionExpiration = Time.time + speedPotionDuration;

        // Payload is to scale the fist
        playerUser.speedModifier = playerUser.speedModifier + speedScale;

    }

    protected override void ItemHasExpired()       // Checklist item 2
    {
        playerUser.speedModifier = playerUser.speedModifier - speedScale;
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
        if(itemState == ItemState.InInventory && Time.time > speedPotionExpiration)
        {
            ItemHasExpired();
        }
    }
}
