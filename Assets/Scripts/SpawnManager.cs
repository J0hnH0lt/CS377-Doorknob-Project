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

    public ItemManager itemManager;

    private Vector3 GetRandomPosition()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), Camera.main.farClipPlane / 2));
    }

    public GameObject SpawnDoor(Color c)
    {
        GameObject door = Instantiate(doorPrefab, GetRandomPosition(), Quaternion.identity);
        door.GetComponent<Renderer>().material.color = c;
        return door;
    }

    public void SpawnObstacles(int n, List<Color> colors)
    {
        ClearObstacles();

        colors.Add(Color.grey);

        for (int i = 0; i < n; i++)
        {
            Vector3 randomPos = GetRandomPosition();

            GameObject obstacle = Instantiate(obstaclePrefab, randomPos, Quaternion.identity);

            if (Physics2D.OverlapCircleAll(randomPos, 14f).Length > 1)
            {
                Destroy(obstacle);
            } else
            {
                obstacle.GetComponent<Renderer>().material.color = colors[Random.Range(0, colors.Count)];
                obstacleList.Add(obstacle);
            }
        }
    }

    public void ClearObstacles()
    {
        foreach (GameObject obstacle in obstacleList) Destroy(obstacle);
        obstacleList = new List<GameObject>();
    }

    public void ClearItems()
    {
        foreach (GameObject item in itemList) {
            if (item != null)
            {
                if (item.GetComponent<Item>().itemState == ItemState.Uncollected)
                {
                    Destroy(item);
                }
            }
            
        }
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
                randomItemPrefab = ItemManager.Instance.GetRandomItem();
                if (randomItemPrefab != null)
                {
                    itemList.Add(Instantiate(randomItemPrefab, randomPos, Quaternion.identity));
                }
            }
        }
    }
}
