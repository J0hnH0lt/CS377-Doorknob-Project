using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineEffect : Effect
{
    public Color mineColor;

    private Color mineBlinkColor = Color.red;

    private Color currColor;

    private float mineSlowDuration = 2.0f;

    private float mineSlowExpiration;

    private float mineObjectDuration = 5;

    private float mineObjectExpriation;

    private Player playerEffected = null;

    public bool isEffectActive = false;

    private Renderer mineRenderer;

    private float mineBlinkTimer;

    private float mineBlinkTime = .5f;

    public bool isSandbox;


    public void Start()
    {
        effectName = "Mine";
        mineObjectExpriation = Time.time + mineObjectDuration;
        mineBlinkTimer = Time.time + mineBlinkTime;
        mineRenderer = GetComponent<Renderer>();
    }

    public void Update()
    {
        BlinkEffect();
        if (!isEffectActive && Time.time > mineObjectExpriation) // if the mine has not been hit for its object's duration
        {
            Debug.Log("Despawning unusedMine");
            Destroy(this.gameObject); // destroy itself
        }
        if (isEffectActive && Time.time > mineSlowExpiration)
        {
            playerEffected.speedModifier += .5f;
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO to make effect apply to an area of players implement OnPhysicsOverlapSphere
        // this makes the effect work for multiple players
        if (collision.gameObject.tag == "Player" && isEffectActive == false)
        {
            playerEffected = collision.gameObject.GetComponent<Player>();
            if (playerEffected.playerColor != mineColor)
            {
                playerEffected.speedModifier -= .5f;
                isEffectActive = true;
                mineSlowExpiration = Time.time + mineSlowDuration;
                mineRenderer.enabled = false;
            }

        }
    }

    protected override void ExpireEffect()
    {
        if (isEffectActive)
        {
            playerEffected.speedModifier += .5f;
        }
        base.ExpireEffect();
    }

    public void BlinkEffect()
    {
        if (Time.time > mineBlinkTimer)
        {
            currColor = this.GetComponent<Renderer>().material.color;
            this.GetComponent<Renderer>().material.color = (currColor == mineBlinkColor) ? mineColor : mineBlinkColor;
            mineBlinkTimer += mineBlinkTime;
        }
    }
}