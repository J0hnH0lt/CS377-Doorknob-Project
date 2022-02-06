using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistScript : MonoBehaviour
{
    public float currentPosition;

    private float timeOfPunch;

    private float endOfPunchDuration;

    private float endOfPunchCoolDown;


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
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time < endOfPunchDuration && currentPosition != reach)
        {
            currentPosition = reach;
        }
        else if(Time.time > endOfPunchDuration && currentPosition == reach)
        {
            currentPosition = resting;
        }
        
    }

    public void PunchIt()
    {
        Debug.Log("Made it to PunchIt");

        if (Time.time > endOfPunchCoolDown)
        {
            Debug.Log("Made it inside PunchIt");

            endOfPunchCoolDown = Time.time + punchCoolDown;

            endOfPunchDuration = Time.time + punchDuration;
            //timeOfPunch = Time.time;
        }
    }       
}
