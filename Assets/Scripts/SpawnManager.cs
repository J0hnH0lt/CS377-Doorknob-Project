using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject doorPrefab;

    public GameManager myGameManager;

    private List<GameObject> itemList = new List<GameObject>();

    public GameObject randomObstaclePrefab;

    private List<GameObject> obstacleList = new List<GameObject>();

    public void SpawnDoor(Color c)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), Camera.main.farClipPlane / 2));
        Instantiate(doorPrefab, pos, Quaternion.identity).GetComponent<Renderer>().material.color = c;
        
    }

    public void SpawnObstacles()
    {
        // SPAWN RANDOM OBSTACLES IN RANDOM lOCATIONS
        var numObstacles = obstacleList.Count;
        for (int i = 0; i < numObstacles; i++)
        {
            Destroy(obstacleList[i]);
        }
        obstacleList = new List<GameObject>();

        for (int i = 0; i < 100; i++)
        {
            Vector3 randomPos = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), Camera.main.farClipPlane / 2));
            Player randomPlayer = myGameManager.players[Random.Range(0, myGameManager.players.Count)];
            if (Physics2D.OverlapCircleAll(randomPos, 14f).Length == 0)
            {
                obstacleList.Add(Instantiate(randomObstaclePrefab, randomPos, Quaternion.identity));

                obstacleList[obstacleList.Count - 1].GetComponent<Renderer>().material.color = randomPlayer.GetComponent<Renderer>().material.color;

            }
        }
    }

    public void SpawnItems()
    {
        // Philip can do this
    }
}
