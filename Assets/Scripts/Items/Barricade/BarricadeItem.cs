using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeItem : Item
{
   
    public GameObject BarricadePrefab;

    private GameObject BarricadeObject;

    private BarricadeEffect Barricade;

    protected override void Awake()
    {
        itemName = ItemName.Block;
        base.Awake();
    }


    protected override void ItemPayload()
    {

        // instantiate mine object
        BarricadeObject = Instantiate(BarricadePrefab, this.gameObject.transform.position, Quaternion.identity);
        BarricadeObject.GetComponent<Collider2D>().enabled = true;
        // handle colors
        Barricade = BarricadeObject.GetComponent<BarricadeEffect>();
        Barricade.playerRef = playerReference;

        Barricade.GetComponent<Renderer>().material.color = playerReference.playerColor;
        BarricadeObject.GetComponent<Renderer>().material.color = Barricade.barricadeColor;

        // if the item is sandbox, communicate that to the effect
        Barricade.isSandbox = true;

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