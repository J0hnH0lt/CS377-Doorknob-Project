using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SpeedPotionItem : Item
{
    private float speedScale = .35f;

    private float speedPotionDuration = 5;

    private float speedPotionExpiration;

    protected override void ItemPayload()
    {
        base.ItemPayload();

        speedPotionExpiration = Time.time + speedPotionDuration;

        // Payload is to scale the fist
        playerReference.speedModifier = playerReference.speedModifier + speedScale;

        itemState = ItemState.InEffect;

    }

    protected override void ItemHasExpired()       // Checklist item 2
    {
        playerReference.speedModifier = playerReference.speedModifier - speedScale;
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
        if(itemState == ItemState.InEffect && Time.time > speedPotionExpiration)
        {
            ItemHasExpired();
        }
    }
}
