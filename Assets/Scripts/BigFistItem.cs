using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BigFistItem : Item
{
    public int fistScale = 2;
    // Start is called before the first frame update

    protected override void ItemPayload()
    {
        base.ItemPayload();

        // Payload is to scale the fist
        playerUser.myFist.transform.localScale = new Vector3(fistScale, fistScale, 1);
    }
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
