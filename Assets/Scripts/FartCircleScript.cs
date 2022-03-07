using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FartCircleScript : MonoBehaviour
{
    private Vector3 finalScale = new Vector3(10, 10, 1);

    private float fartSpeed = .1f;

    private void Start()
    {
        Destroy(this.gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = transform.localScale;
        temp.x = Mathf.Lerp(transform.localScale.x, finalScale.x, Mathf.Sin(Time.time));
        temp.y = Mathf.Lerp(transform.localScale.y, finalScale.y, Mathf.Sin(Time.time));
        transform.localScale = temp;
    }

}
