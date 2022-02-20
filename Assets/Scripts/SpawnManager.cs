using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject doorPrefab;

    public GameObject obstaclePrefab;

    private GameObject randomItemPrefab;

    private List<GameObject> obstacleList = new List<GameObject>();

    private List<GameObject> itemList = new List<GameObject>();

    private ItemManager itemManager;

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
        Debug.Log("Item Count: "+ itemList.Count);
        itemList = new List<GameObject>();
    }

    public void SpawnItems(int numItems = 2)
    {
        ClearItems();

        while (itemList.Count < numItems)
        {
            Vector3 randomPos = GetRandomPosition();
            if (Physics2D.OverlapCircleAll(randomPos, 4f).Length == 0)
            {
                // @Clem, use this to spawn random items into the game
                // itemList.Add(RandomItem)
                randomItemPrefab = ItemManager.Instance.GetRandomItem();
                itemList.Add(Instantiate(randomItemPrefab, randomPos, Quaternion.identity));
            }
        }
    }
}
