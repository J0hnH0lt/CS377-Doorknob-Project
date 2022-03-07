using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearDesc : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject parent;
 
    void Awake()
    {
        parent = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        if (parent.GetComponent<Item>().isSandBox)
        {
            if (Physics2D.OverlapCircleAll(parent.transform.position, 3f).Length > 1)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }

        } else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        
        
    }
}
