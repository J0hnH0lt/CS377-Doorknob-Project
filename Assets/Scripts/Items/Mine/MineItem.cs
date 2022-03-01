using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineItem : Item
{
    public GameObject MinePrefab;

    private GameObject Mine;


    protected override void ItemPayload()
    {
        // instantiate mine object
        Mine = Instantiate(MinePrefab, playerReference.transform.parent, true);
        // handle colors
        Mine.GetComponent<Mine>().mineColor = playerReference.playerColor;
        Mine.GetComponent<Renderer>().material.color = playerReference.playerColor;

        // assign its position
        Mine.transform.position = this.gameObject.transform.position;

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