using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameTextManager : MonoBehaviour
{
    public static GameTextManager Instance;

    public GameObject InstructionPanel;
    public GameObject TitleText;
    public GameObject InfoPanel;

    private void Awake()
    {
        Instance = this;
    }

    public void StartUp()
    {
    }

    public void GameRunning()
    {
        //InfoPanel.transform.position.x = InfoPanel.transform.position.x + 600;
        InfoPanel.transform.position = new Vector3(InfoPanel.transform.position.x + 600,
                                                   InfoPanel.transform.position.y, 
                                                   InfoPanel.transform.position.z);

        TitleText.GetComponent<Text>().text = "";
    }

    public void GameOver()
    {
        TitleText.GetComponent<Text>().text = "GAME OVER \n You did you dirty little dog";
        
        //TitleText.transform.position = new Vector3(0, 0, 0);
        TitleText.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);

        //InfoPanel.transform.position = new Vector3(InfoPanel.transform.position.x - 600,
        //                                           InfoPanel.transform.position.y,
        //                                           InfoPanel.transform.position.z);
    }


}
