using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public SpawnManager mySpawnManager;

    public List<Player> players = new List<Player>();

    public List<Color> playerColors = new List<Color>();

    public GameState State;

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
            if (players.Count >=1 && PlayersReady())
            {
                foreach (Player p in players)
                {
                    p.myReadyUpIcon.color = Color.clear;
                    playerColors.Add(p.playerColor);
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
                SceneManager.LoadScene("MVP");
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
                HandleLevelChange();
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

    public void HandleLevelChange() {


        // Get the number of items to spawn
        HandlePlayerFart();
        int numItems = UnityEngine.Random.Range(2, 4);
        mySpawnManager.SpawnItems(numItems);
        mySpawnManager.SpawnObstacles(10000, playerColors);

        // Handle the weighing and selection of who farts
        
    }

    public void HandlePlayerFart() {
        // construct a health range dictionary of all the players
        var healhList = new List<Tuple<int, Player>>();


        int random_sum = 0;
        foreach (Player p in players)
        {
            random_sum += p.health;
            healhList.Add(Tuple.Create(random_sum, p));
        }

        // get a random number between 0 and the total amount of player health
        int health_helper = UnityEngine.Random.Range(0, random_sum);

        foreach (var tuple in healhList)
        {
        
            int chance = tuple.Item1;
            Player player = tuple.Item2;
            // select the player if the random number falls within their health range
            if (health_helper <= chance)
            {
                // spawn the door of the players color
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