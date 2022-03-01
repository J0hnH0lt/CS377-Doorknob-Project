using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<GameObject> prefabList = new List<GameObject>();
    public GameObject bigFistPrefab;
    public GameObject speedPotionPrefab;
    public GameObject mineItemPrefab;
    public GameObject healthPackItemPrefab;
    public GameObject barricadeItemPrefab;

    public static ItemManager Instance;

    private void Awake()
    {
        Instance = this;
        prefabList.Add(bigFistPrefab);
        prefabList.Add(speedPotionPrefab);
        prefabList.Add(mineItemPrefab);
        prefabList.Add(healthPackItemPrefab);
        prefabList.Add(barricadeItemPrefab);
    }

    public GameObject GetRandomItem()
    {
        int prefabIndex = UnityEngine.Random.Range(0, prefabList.Count);
        Debug.Log("Spawning Item wiht prefab index " + prefabIndex);

        return prefabList[prefabIndex];
    }
}
