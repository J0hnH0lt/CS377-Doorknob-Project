using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineItem : Item
{
    public GameObject MinePrefab;

    private GameObject MineObject;

    private MineEffect mine;

    protected override void Awake()
    {
        itemName = ItemName.Mine;
        base.Awake();
    }


    protected override void ItemPayload()
    {
        // instantiate mine object
        Vector3 MinePosn = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 497.0f);

        MineObject = Instantiate(MinePrefab, MinePosn, Quaternion.identity, playerReference.transform.parent);
        // handle colors
        mine = MineObject.GetComponent<MineEffect>();
        mine.mineColor = playerReference.playerColor;
        MineObject.GetComponent<Renderer>().material.color = playerReference.playerColor;

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