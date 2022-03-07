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
        Vector3 BarricadePosn = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 498);
        foreach (Collider2D c in Physics2D.OverlapCircleAll(BarricadePosn, 0))
        {
            if(c.name == "BarricadePrefab(Clone)" && c.GetComponent<Renderer>().material.color == playerReference.playerColor)
            {
                c.GetComponent<BarricadeEffect>().scale += 1;
                return;
            }
        }
        BarricadeObject = Instantiate(BarricadePrefab, BarricadePosn, Quaternion.identity, playerReference.transform.parent);
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