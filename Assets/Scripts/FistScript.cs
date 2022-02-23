using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistScript : MonoBehaviour
{
    public float currentPosition;

    private float endOfPunchDuration;

    private float endOfPunchCoolDown;

    public float fistScaleMod = 1;

    private Collider2D myCollider;

    [SerializeField]
    private float reach;

    [SerializeField]
    private float resting;

    [SerializeField]
    private float punchDuration;

    [SerializeField]
    private float punchCoolDown;

    // Start is called before the first frame update
    void Start()
    {
        currentPosition = resting;

        myCollider = this.GetComponent<Collider2D>();

        myCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        this.transform.localScale = new Vector3(fistScaleMod, fistScaleMod, 1);

        if (Time.time < endOfPunchDuration && currentPosition != reach)
        {
            currentPosition = reach;
        }
        else if(Time.time > endOfPunchDuration && currentPosition == reach)
        {
            myCollider.enabled = false;
            currentPosition = resting;
        }
        else if (Time.time > endOfPunchDuration && myCollider.enabled)
        {
            myCollider.enabled = false;
        }


        
    }

    public void PunchIt()
    {

        if (Time.time > endOfPunchCoolDown)
        {

            endOfPunchCoolDown = Time.time + punchCoolDown;

            endOfPunchDuration = Time.time + punchDuration;
        }
    }       
}
