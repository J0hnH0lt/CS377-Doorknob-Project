using UnityEngine;
using System.Collections;

public class Safety : Item
{

    protected override void Awake()
    {
        itemName = ItemName.Safety;
        base.Awake();
    }

    protected override void ItemPayload()
    {
        if (isSandBox == false && playerReference.isFarting == true)
        {
            GameManager.Instance.UpdateGameState(GameState.ItemPhase);
        }

        ItemHasExpired();
    }
}
