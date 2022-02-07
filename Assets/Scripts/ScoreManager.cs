using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    Dictionary<int, int> dict = new Dictionary<int, int>();

    public static ScoreManager Instance;

    public Text HealthText;

    private void Awake()
    {
        Instance = this;
    }

    public void AddPlayer(int id, int health)
    {
        dict.Add(id, health);
        updateHealthText();
    }

    public void UpdateHealth(int id, int health)
    {
        dict[id] = health;
        updateHealthText();
    }

    public void updateHealthText()
    {
        if( dict.Count >= 2)
        {
            string temp = "";
            foreach (KeyValuePair<int, int> entry in dict)
            {
                temp += "Player " + entry.Key.ToString() + ": " + entry.Value.ToString() + " HP \t\t";
            }
            HealthText.text = temp.Trim();
        }
 
    }

    public void GameOver()
    {
        HealthText.text = "Game Over";
    }
}
