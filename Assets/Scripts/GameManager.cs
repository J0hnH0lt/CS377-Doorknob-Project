using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public SpawnManager mySpawnManager;

    public List<Player> players = new List<Player>();

    private List<GameObject> itemList = new List<GameObject>();

    public GameState State;

    public GameObject bigFistPrefab;

    public static event Action<GameState> OnGameStateChanged;

    private float minDoorSpawnTime = 3.0f;
    private float maxDoorSpawnTime = 7.0f;
    private float doorTimer = 0.0f;
    private float nextDoorTime;

    private float gameOverTimer = 0.0f;
    private float gameOverTime;

    private 

    void Awake() {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateGameState(GameState.Menu);
    }

    // Update is called once per frame
    void Update()
    {
        if (State == GameState.Menu)
        {
            if (players.Count >=2 && PlayersReady())
            {
                foreach (Player p in players)
                {
                    p.myReadyUpIcon.color = Color.clear;
                }

                UpdateGameState(GameState.ItemPhase);
                GameTextManager.Instance.GameRunning();
            }
        }
        if (State == GameState.ItemPhase){
            doorTimer += Time.deltaTime;
            if (doorTimer >= nextDoorTime) {
                UpdateGameState(GameState.CombatPhase);
            }
        }
        if (State == GameState.GameOver)
        {
            gameOverTimer += Time.deltaTime;
            if (gameOverTimer >= gameOverTime)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    public bool PlayersReady()
    {
        foreach (Player p in players)
        {
            if (!p.isReady)
            {
                return false;
            }
        }
        return true;
    }
 
    public void UpdateGameState(GameState newState) {
        State = newState;
        switch (newState) {
            case GameState.Menu:
                HandleMenu();
                break;
            case GameState.ItemPhase:
                HandleItemPhase();
                break;
            case GameState.CombatPhase:
                HandleFart();
                break;
            case GameState.GameOver:
                HandleGameOver();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

    public void HandleMenu() {
        //UpdateGameState(GameState.ItemPhase);
    }

    public void HandleItemPhase()
    {
        doorTimer = 0.0f;
        nextDoorTime = UnityEngine.Random.Range(minDoorSpawnTime, maxDoorSpawnTime);
    }

    public void HandleGameOver()
    {
        gameOverTimer = 0.0f;
        gameOverTime = 5.0f;
    }

    public void HandleFart() {
        mySpawnManager.SpawnObstacles();


        // SPAWN RANDOM ITEMS IN RANDOM lOCATIONS
        var itemCount = itemList.Count;
        for (int i = 0; i < itemCount; i++)
        {
            if (itemList[i] != null && itemList[i].GetComponent<BigFistItem>().IsNotActive())
            {
                Destroy(itemList[i]);
            }
        }
        itemList = new List<GameObject>();
        Debug.Log(itemList.Count);

        for (int i = 0; i < 100; i++)
        {
            Vector3 randomPos = Camera.main.ScreenToWorldPoint(new Vector3(UnityEngine.Random.Range(0, Screen.width), UnityEngine.Random.Range(0, Screen.height), Camera.main.farClipPlane / 2));
            Player randomPlayer = players[UnityEngine.Random.Range(0, players.Count)];
            if (itemList.Count < 1 && Physics2D.OverlapCircleAll(randomPos, 4f).Length == 0)
            {
                itemList.Add(Instantiate(bigFistPrefab, randomPos, Quaternion.identity));

                //itemList[itemList.Count - 1].GetComponent<Renderer>().material.color = randomPlayer.GetComponent<Renderer>().material.color;

            }
            else
            {
                Debug.Log(Physics2D.OverlapCircleAll(randomPos, 1f).Length);
            }
        }


        // GET A RANDOM PLAYER
        var healhList = new List<Tuple<int, Player>>();
  
        int random_sum = 0;
        foreach (Player p in players) {
            random_sum += p.health;
            healhList.Add(Tuple.Create(random_sum, p));
        }

        int health_helper = UnityEngine.Random.Range(0, random_sum);

        foreach (var tuple in healhList)
        {
            int chance = tuple.Item1;
            Player player = tuple.Item2;
            if (health_helper <= chance)
            {
                mySpawnManager.SpawnDoor(player.GetComponent<Renderer>().material.color);
                player.OnFart();
                break;
            }
        }
    }

    public void AddPlayer(Player p){
        players.Add(p);
        Debug.Log("Player Added");
    }
    
}


public enum GameState {
    Menu,
    ItemPhase,
    CombatPhase,
    GameOver
}