using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DashScript : MonoBehaviour
{

    private Image myDashIndicator;

    private bool dashEnabled;

    private float dashCooldownExpiration;

    private float dashExpiration;

    private Player myPlayer;

    [SerializeField]
    private float dashSpeed;

    [SerializeField]
    private float normalSpeed;

    [SerializeField]
    private float dashDuration;

    [SerializeField]
    private float dashCooldown;

    // Use this for initialization
    void Awake()
    {
        dashCooldownExpiration = Time.time;
        dashExpiration = Time.time;
        dashEnabled = false;
        myDashIndicator = gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        myPlayer = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dashEnabled && Time.time > dashExpiration)
        {
            GetComponent<Player>().currSpeed = normalSpeed;
            dashEnabled = false;
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (myPlayer.speedModifier == 1 && myPlayer.GetComponent<Rigidbody2D>().velocity.x != 0 && myPlayer.GetComponent<Rigidbody2D>().velocity.y != 0)
            {
                if (Time.time > dashCooldownExpiration)
                {
                    dashExpiration = Time.time + dashDuration;
                    dashCooldownExpiration = Time.time + dashCooldown;
                    dashEnabled = true;
                    //GetComponent<Player>().currSpeed = dashSpeed;
                    myPlayer.currSpeed = dashSpeed;
                    StartCoroutine(DashImageLerp());
                }
            }
        }
         
    }

    private IEnumerator DashImageLerp()
    {
        float startTime = Time.time;
        float timeElapsed = (Time.time - startTime) / dashCooldown;
        while (timeElapsed < 1f)
        {
            timeElapsed = (Time.time - startTime) / dashCooldown;
            myDashIndicator.fillAmount = Mathf.Lerp(1f, 0f, timeElapsed);
            yield return null;
        }
    }
}
