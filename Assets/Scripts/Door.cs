using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    private GameManager GM;
    // Start is called before the first frame update
    void Start()
    {
        GM = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>().hasFarted)
        {
            //Debug.Log("Aye you did it you little freak ahahah UwU XD");
            GM.UpdateGameState(GameState.ItemPhase);
            Destroy(this.gameObject);
        }
        
    }
}
