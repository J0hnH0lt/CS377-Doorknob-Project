using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameTextManager : MonoBehaviour
{
    public static GameTextManager Instance;

    public Text GameText;

    // really booty implementation but idrgaf... its temp
    private bool player1Added = false;

    private void Awake()
    {
        Instance = this;
    }

    public void StartUp()
    {
        GameText.text = "Press X to Join";
    }

    public void GameRunning()
    {
        GameText.text = "";
    }

    public void GameOver()
    {
        GameText.text = "Game Over";
    }

    
}
