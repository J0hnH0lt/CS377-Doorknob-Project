using UnityEngine;
using System.Collections;

public class BarricadeEffect : Effect
{
    public Color barricadeColor;

    public bool isSandbox;

    private float barricadeDuration = 3.5f;

    public float barricadeExpiration;

    public Player playerRef;

    public float scale = 1f;

    private void Start()
    {
        effectName = "Barricade";

        barricadeColor = playerRef.playerColor;

        GetComponent<Renderer>().material.color = barricadeColor;

        barricadeExpiration = Time.time + barricadeDuration;

    }

    private void Update()
    {
        if (barricadeColor != playerRef.playerColor)
        {
            GetComponent<Renderer>().material.color = barricadeColor = playerRef.playerColor;
        }

        GetComponent<Transform>().transform.localScale = new Vector3(5,5,5) * scale;

        if (Time.time > barricadeExpiration)
        {
            if(scale > 1f)
            {
                scale -= 1f;
                barricadeExpiration = Time.time + 1f;
            }
            else
            {
                ExpireEffect();
            }
            
        }
    }
}
