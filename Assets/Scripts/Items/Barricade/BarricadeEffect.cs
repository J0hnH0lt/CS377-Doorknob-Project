using UnityEngine;
using System.Collections;

public class BarricadeEffect : Effect
{
    public Color barricadeColor;

    public bool isSandbox;

    private float barricadeDuration = 5.0f;

    private float barricadeExpiration;

    public Player playerRef;

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

        if (Time.time > barricadeExpiration)
        {
            ExpireEffect();
        }
    }
}
