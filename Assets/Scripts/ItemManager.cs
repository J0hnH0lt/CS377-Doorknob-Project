using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Dictionary<ItemName, GameObject> prefabDictionary = new Dictionary<ItemName, GameObject>();
    public GameObject bigFistPrefab;
    public GameObject speedPotionPrefab;
    public GameObject mineItemPrefab;
    public GameObject healthPackItemPrefab;
    public GameObject barricadeItemPrefab;
    public GameObject phaseItemPrefab;


    public GameObject safetyItemPrefab;
    private bool safetyEnabled = true;

    public static ItemManager Instance;

    private void Awake()
    {

        Instance = this;
        prefabDictionary.Add(ItemName.BigFist, bigFistPrefab);
        prefabDictionary.Add(ItemName.Boost, speedPotionPrefab);
        prefabDictionary.Add(ItemName.Mine, mineItemPrefab);
        prefabDictionary.Add(ItemName.HealthBoost, healthPackItemPrefab);
        prefabDictionary.Add(ItemName.Block, barricadeItemPrefab);
        prefabDictionary.Add(ItemName.Phase, phaseItemPrefab);
    }

    public GameObject GetRandomItem()
    {
        if (safetyEnabled)
        {
            int safetyRandomNumber = UnityEngine.Random.Range(0, 100);
            if (safetyRandomNumber >= 95)
            {
                return safetyItemPrefab;
            }
        }
        if (prefabDictionary.Count == 0) // if all objects have been removed from the game
        {
            return null;
        }
        List<ItemName> keyList = new List<ItemName>(prefabDictionary.Keys);
        ItemName randomItemKey = keyList[UnityEngine.Random.Range(0, keyList.Count)];

        Debug.Log("Attempting to spawn Item: " + randomItemKey);
        return prefabDictionary[randomItemKey];
    }

    public void ToggleItemSpawning(ItemName itemName)
    {
        Debug.Log("Toggle for Item: " + itemName);
        switch (itemName)
        {
            case ItemName.Boost:
                {
                    if (prefabDictionary.ContainsKey(itemName))
                    {
                        prefabDictionary.Remove(itemName);
                    }
                    else
                    {
                        prefabDictionary.Add(ItemName.Boost, speedPotionPrefab);
                    }
                    break;
                }
            case ItemName.BigFist:
                {
                    if (prefabDictionary.ContainsKey(itemName))
                    {
                        prefabDictionary.Remove(itemName);
                    }
                    else
                    {
                        prefabDictionary.Add(ItemName.BigFist, bigFistPrefab);
                    }
                    break;
                }
            case ItemName.Mine:
                {
                    if (prefabDictionary.ContainsKey(itemName))
                    {
                        prefabDictionary.Remove(itemName);
                    }
                    else
                    {
                        prefabDictionary.Add(ItemName.Mine, mineItemPrefab);
                    }
                    break;
                }
            case ItemName.HealthBoost:
                {
                    if (prefabDictionary.ContainsKey(itemName))
                    {
                        prefabDictionary.Remove(itemName);
                    }
                    else
                    {
                        prefabDictionary.Add(ItemName.HealthBoost, healthPackItemPrefab);
                    }
                    break;
                }
            case ItemName.Block:
                {
                    if (prefabDictionary.ContainsKey(itemName))
                    {
                        prefabDictionary.Remove(itemName);
                    }
                    else
                    {
                        prefabDictionary.Add(ItemName.Block, barricadeItemPrefab);
                    }
                    break;
                }
            case ItemName.Phase:
                {
                    if (prefabDictionary.ContainsKey(itemName))
                    {
                        prefabDictionary.Remove(itemName);
                    }
                    else
                    {
                        prefabDictionary.Add(ItemName.Phase, phaseItemPrefab);
                    }
                    break;
                }
            case ItemName.Safety:
                {
                    safetyEnabled = !safetyEnabled;
                    break;
                }
        }
        Debug.Log("Dictionary length is now: " + prefabDictionary.Count.ToString());
    }
}

public enum ItemName
{
    Boost,
    BigFist,
    Mine,
    Block,
    HealthBoost,
    Phase,
    Safety
}