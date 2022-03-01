using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : Item
{
    private int healthIncrease = 1;


    protected override void ItemPayload()
    {
        base.ItemPayload();
        if (playerReference.currHealth != playerReference.maxHealth)
        {
            playerReference.currHealth += healthIncrease;
            playerReference.UpdateHealthBar();
        } 
        ItemHasExpired();
    }

    protected override void ItemHasExpired()       // Checklist item 2
    {
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
}

