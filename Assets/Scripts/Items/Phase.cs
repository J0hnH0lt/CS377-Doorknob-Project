using UnityEngine;
using System.Collections.Generic;

public class Phase : Item
{
    private float phaseDuration = 3.0f;

    private float phaseExperiation;

    private Color phaseColor;

    protected override void Awake()
    {
        itemName = ItemName.Phase;
        base.Awake();
    }

    protected override void ItemPayload()
    {
        base.ItemPayload();
        phaseExperiation = Time.time + phaseDuration;

        // Payload is to scale the fist
        playerReference.isPhase = true;

        phaseColor = playerReference.playerColor;
        // set transparency to 0
        phaseColor.a = 0;

        itemState = ItemState.InEffect;
    }

    protected override void ItemHasExpired()       // Checklist item 2
    {
        playerReference.isPhase= false;
        playerReference.GetComponent<Renderer>().material.color = playerReference.playerColor;
        playerReference.myFist.GetComponent<Renderer>().material.color = playerReference.playerColor;

        foreach (GameObject obstacle in playerReference.physicObjects)
        {
            if (obstacle != null)
            {
                Debug.Log("restoring Collsion");
                Physics2D.IgnoreCollision(obstacle.GetComponent<Collider2D>(), playerReference.GetComponent<Collider2D>(), false);
            }

        }
        playerReference.physicObjects = new List<GameObject>();

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

    // Update is called once per frame
    void Update()
    {
        if(itemState == ItemState.InEffect)
        {
            playerReference.GetComponent<Renderer>().material.color = LerpColor();
            playerReference.myFist.GetComponent<Renderer>().material.color = LerpColor();

        }
        if (itemState == ItemState.InEffect && Time.time > phaseExperiation)
        {
            ItemHasExpired();
        }
    }

    public Color LerpColor()
    {
        return Color.Lerp(phaseColor, playerReference.playerColor, Mathf.Sin(Time.time*15));
    }

}