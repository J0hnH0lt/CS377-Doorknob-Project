using UnityEngine;
using System.Collections;

public class Safety : Item
{

    protected override void ItemPayload()
    {
        if (isSandBox == false)
        {
            GameManager.Instance.UpdateGameState(GameState.ItemPhase);
        }

        ItemHasExpired();
    }
}
