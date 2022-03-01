using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistScript : MonoBehaviour
{
    public float currentPosition;

    public float fistScaleMod = 1;

    [SerializeField]
    private float reach;

    [SerializeField]
    private float resting;

    [SerializeField]
    private float punchLength;

    // Start is called before the first frame update
    void Start()
    {
        currentPosition = resting;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localScale = new Vector3(fistScaleMod, fistScaleMod, 1);
        this.transform.position = this.transform.parent.position + (transform.up * (fistScaleMod + currentPosition));
    }

    public void TriggerPunch()
    {
        Collider2D target = Physics2D.OverlapCircle(this.transform.position, 0.5f*fistScaleMod);
        if (target != null && target.name == "PlayerPrefab(Clone)") target.gameObject.GetComponent<Player>().OnHit();
        StartCoroutine(AnimatePunch());
    }

    private IEnumerator AnimatePunch()
    {
        float startTime = Time.time;
        float timeElapsed = (Time.time - startTime) / punchLength;

        while (timeElapsed < 1f)
        {
            if (timeElapsed < 0.5f){
                currentPosition = Mathf.Lerp(resting, reach, timeElapsed);
            } else
            {
                currentPosition = Mathf.Lerp(reach, resting, timeElapsed);
            }
            timeElapsed = (Time.time - startTime) / punchLength;

            yield return null;
        }
    }
}
