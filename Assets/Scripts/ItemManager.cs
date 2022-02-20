using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<GameObject> prefabList = new List<GameObject>();
    public GameObject bigFistPrefab;
    public GameObject speedPotionPrefab;
    public static ItemManager Instance;

    private void Awake()
    {
        Instance = this;
        prefabList.Add(bigFistPrefab);
        prefabList.Add(speedPotionPrefab);
    }

    public GameObject GetRandomItem()
    {
        int prefabIndex = UnityEngine.Random.Range(0, prefabList.Count);
        Debug.Log("Spawning Item wiht prefab index " + prefabIndex);

        return prefabList[prefabIndex];
    }
}
