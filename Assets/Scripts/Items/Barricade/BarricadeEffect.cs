using UnityEngine;
using System.Collections;

public class BarricadeEffect : Effect
{
    public Color barricadeColor;

    public bool isSandbox;

    private float barricadeDuration = 5.0f;

    private float barricadeExpiration;

    private void Start()
    {
        effectName = "Barricade";
        barricadeExpiration = Time.time + barricadeDuration;
    }

    private void Update()
    {
        if (Time.time > barricadeExpiration)
        {
            ExpireEffect();
        }
    }
}
