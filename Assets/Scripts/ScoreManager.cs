using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    Dictionary<int, Text> playerTextDictionary = new Dictionary<int, Text>();
    Dictionary<int, int> playerToHealthDictionary = new Dictionary<int, int>();

    public static ScoreManager Instance;

    public Text Player1Text;

    public Text Player2Text;

    public Text GameText;

    // really booty implementation but idrgaf... its temp
    private bool player1Added = false;

    private void Awake()
    {
        Instance = this;
    }

    public void AddPlayer(int id, Color playerColor, int health)
    {
        if (!player1Added)
        {
            Player1Text.color = playerColor;
            playerTextDictionary.Add(id, Player1Text);
            playerToHealthDictionary.Add(id, health);
            player1Added = true;
            updateHealthText(id);
            return;
        }
        else
        {
            Player2Text.color = playerColor;
            playerTextDictionary.Add(id, Player2Text);
            playerToHealthDictionary.Add(id, health);
            updateHealthText(id);
        }
    }


    public void updateHealthText(int id)
    {
        playerTextDictionary[id].text = playerToHealthDictionary[id].ToString();
    }

    public void updatePlayerHealth(int id, int health)
    {
        playerToHealthDictionary[id] = health;
        updateHealthText(id);
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
