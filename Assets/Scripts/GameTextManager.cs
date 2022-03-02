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
    public GameObject ItemPanel;
    public GameObject JoinStartText;

    private void Awake()
    {
        Instance = this;
        StartCoroutine(InstructionPanelLerpIn());
    }

    public void StartUp()
    {
        
    }

    public void GameRunning()
    {
        StartCoroutine(InstructionPanelLerpOut());
        StartCoroutine(PanelLerpOut(ItemPanel));

        Destroy(JoinStartText);


        TitleText.GetComponent<Text>().text = "";
    }

    public void GameOver()
    {
        TitleText.GetComponent<Text>().text = "GAME OVER";
    }

    private IEnumerator InstructionPanelLerpOut()
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

    private IEnumerator InstructionPanelLerpIn()
    {
        float startTime = Time.time;
        float timeElapsed = (Time.time - startTime);
        float finalPos = InfoPanel.transform.position.x;
        while (timeElapsed < 3f)
        {
            timeElapsed = (Time.time - startTime);
            InfoPanel.transform.position = new Vector3(finalPos + Mathf.Lerp(60f, 1f, timeElapsed),
                                                   InfoPanel.transform.position.y,
                                                   InfoPanel.transform.position.z);
            Color c = TitleText.GetComponent<Text>().color;
            c.a = Mathf.Lerp(0f, 1f, timeElapsed);
            TitleText.GetComponent<Text>().color = c;

            yield return null;
        }
    }

    // TODO add ability to input direction and distance
    private IEnumerator PanelLerpOut(GameObject textPanel)
    {
        float startTime = Time.time;
        float timeElapsed = (Time.time - startTime) / 500.0f;
        while (timeElapsed < 1f)
        {
            timeElapsed = (Time.time - startTime) / 500.0f;
            textPanel.transform.position = new Vector3(textPanel.transform.position.x,
                                                       textPanel.transform.position.y - Mathf.Lerp(0, 200, timeElapsed),
                                                       textPanel.transform.position.z);
            yield return null;
        }
    }

    private IEnumerator PanelLerpIn(GameObject textPanel)
    {
        float startTime = Time.time;
        float timeElapsed = (Time.time - startTime);
        float finalPos = textPanel.transform.position.x;
        while (timeElapsed < 3f)
        {
            timeElapsed = (Time.time - startTime);
            textPanel.transform.position = new Vector3(finalPos + Mathf.Lerp(60f, 1f, timeElapsed),
                                                   textPanel.transform.position.y,
                                                   textPanel.transform.position.z);
            Color c = TitleText.GetComponent<Text>().color;
            c.a = Mathf.Lerp(0f, 1f, timeElapsed);
            TitleText.GetComponent<Text>().color = c;
            yield return null;
        }
    }

}
