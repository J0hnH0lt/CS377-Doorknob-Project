using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject doorPrefab;

    public GameObject obstaclePrefab;

    private List<GameObject> obstacleList = new List<GameObject>();

    private List<GameObject> itemList = new List<GameObject>();

    private Vector3 GetRandomPosition()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), Camera.main.farClipPlane / 2));
    }

    public void SpawnDoor(Color c)
    {
        Vector3 randomPos = GetRandomPosition();
        while(Physics2D.OverlapCircleAll(randomPos, 4f).Length > 0)
        {
            randomPos = GetRandomPosition();
        }
        Instantiate(doorPrefab, randomPos, Quaternion.identity).GetComponent<Renderer>().material.color = c;
    }

    public void SpawnObstacles(int n)
    {
        ClearObstacles();
        for (int i = 0; i < n; i++)
        {
            Vector3 randomPos = GetRandomPosition();
            
            if (Physics2D.OverlapCircleAll(randomPos, 14f).Length == 0)
            {
                obstacleList.Add(Instantiate(obstaclePrefab, randomPos, Quaternion.identity));
            }
        }
    }

    public void ClearObstacles()
    {
        foreach (GameObject obstacle in obstacleList) Destroy(obstacle);
        Debug.Log(obstacleList.Count);
    }

    public void ClearItems()
    {
        foreach (GameObject item in itemList) Destroy(item);
        Debug.Log(itemList.Count);
    }

    public void SpawnItems(int n)
    {
        ClearItems();

        for (int i = 0; i < n; i++)
        {
            Vector3 randomPos = GetRandomPosition();
            if (itemList.Count < 1 && Physics2D.OverlapCircleAll(randomPos, 4f).Length == 0)
            {
                // @Clem, use this to spawn random items into the game
                // itemList.Add(RandomItem)
            }
        }
    }
}
