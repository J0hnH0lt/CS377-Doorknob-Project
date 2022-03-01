using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineItem : Item
{
    public GameObject MinePrefab;

    private GameObject MineObject;

    private MineEffect mine;


    protected override void ItemPayload()
    {
        // instantiate mine object
        MineObject = Instantiate(MinePrefab, playerReference.transform.parent, true);
        // handle colors
        mine = MineObject.GetComponent<MineEffect>();
        mine.mineColor = playerReference.playerColor;
        MineObject.GetComponent<Renderer>().material.color = playerReference.playerColor;

        // assign its position
        MineObject.transform.position = this.gameObject.transform.position;

        // if the item is sandbox, communicate that to the effect
        mine.isSandbox = true;

        base.ItemPayload();
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