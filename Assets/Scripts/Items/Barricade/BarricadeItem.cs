using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeItem : Item
{
    public GameObject BarricadePrefab;

    private GameObject BarricaideObject;

    private BarricadeEffect Barricade;

    protected override void ItemPayload()
    {

        // instantiate mine object
        BarricaideObject = Instantiate(BarricadePrefab, playerReference.transform.parent, true);
        // handle colors
        Barricade = BarricaideObject.GetComponent<BarricadeEffect>();
        Barricade.barricadeColor = playerReference.playerColor;
        Barricade.GetComponent<Renderer>().material.color = playerReference.playerColor;

        // assign its position
        BarricaideObject.transform.position = this.gameObject.transform.position;

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