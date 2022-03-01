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
        BarricaideObject = Instantiate(BarricadePrefab, this.gameObject.transform.position, Quaternion.identity);
        BarricaideObject.GetComponent<Collider2D>().enabled = true;
        // handle colors
        Barricade = BarricaideObject.GetComponent<BarricadeEffect>();
        Barricade.barricadeColor = playerReference.playerColor;
        Barricade.GetComponent<Renderer>().material.color = playerReference.playerColor;
        BarricaideObject.GetComponent<Renderer>().material.color = Barricade.barricadeColor;

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