using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour
{
    public string effectName;

    public void ForceExpireEffect()
    {
        ExpireEffect();
    }

    protected virtual void ExpireEffect()
    {
        Debug.Log("Forcing Removal of effect");
        Destroy(this.gameObject);
    }
}
