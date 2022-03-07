using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FartCircleScript : MonoBehaviour
{
    private Vector3 finalScale = new Vector3(15, 15, 1);
    private Vector3 originalScale;
    public Player player;

    private void Awake()
    {
        originalScale = transform.localScale;
    }
    private void Start()
    {
        Destroy(this.gameObject, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
        Vector3 temp = transform.localScale;
        temp.x = Mathf.Lerp(originalScale.x, finalScale.x, Mathf.Sin(Time.time*5));
        temp.y = Mathf.Lerp(originalScale.y, finalScale.y, Mathf.Sin(Time.time*5));
        transform.localScale = temp;
    }
}
