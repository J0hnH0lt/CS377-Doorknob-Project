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
        StartCoroutine(InstructionPanelLerp());

        TitleText.GetComponent<Text>().text = "";
    }

    public void GameOver()
    {
        TitleText.GetComponent<Text>().text = "GAME OVER";

        //InfoPanel.transform.position = new Vector3(InfoPanel.transform.position.x - 600,
        //                                           InfoPanel.transform.position.y,
        //                                           InfoPanel.transform.position.z);
    }

    private IEnumerator InstructionPanelLerp()
    {
        float startTime = Time.time;
        float timeElapsed = (Time.time - startTime) / 500.0f;
        while (timeElapsed < 1f)
        {
            timeElapsed = (Time.time - startTime) / 500.0f;
            InfoPanel.transform.position = new Vector3(InfoPanel.transform.position.x + Mathf.Lerp(0, 200, timeElapsed),
                                                   InfoPanel.transform.position.y,
                                                   InfoPanel.transform.position.z);
            yield return null;
        }
    }


}
