using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public SpawnManager mySpawnManager;

    public GameObject obsticlePrefab;

    public List<Player> players = new List<Player>();

    public Player fartingPlayer;

    public List<Color> playerColors = new List<Color>();

    public List<GameObject> playerObsticles = new List<GameObject>();

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;

    private float minDoorSpawnTime = 3.0f;
    private float maxDoorSpawnTime = 7.0f;
    private float doorTimer = 0.0f;
    private float nextDoorTime;
    private GameObject door;

    private float gameOverTimer = 0.0f;
    private float gameOverTime;

    private bool isStartAfterSandbox = true;

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
        if (isStartAfterSandbox)
        {
            handleSandBoxChange();
            isStartAfterSandbox = false;
        }

        if (door!=null)
        {
            fartingPlayer.DisableTrailSlow();
            fartingPlayer = null;
            Destroy(door);
        }

        // spawn the door
        doorTimer = 0.0f;
        nextDoorTime = UnityEngine.Random.Range(minDoorSpawnTime, maxDoorSpawnTime);

        //Destroy(GameTextManager.Instance.ItemPanel);
    }

    public void HandleGameOver()
    {
        gameOverTimer = 0.0f;
        gameOverTime = 5.0f;
    }

    public void HandleLevelChange() {


        // Get the number of items to spawn
        HandlePlayerFart();
        int numItems = UnityEngine.Random.Range(4, 10);
        mySpawnManager.SpawnItems(numItems);
        mySpawnManager.SpawnObstacles(200, playerColors);
        
    }

    public void HandlePlayerFart() {
        // construct a health range dictionary of all the players
        var healhList = new List<Tuple<int, Player>>();


        int random_sum = 0;
        foreach (Player p in players)
        {
            random_sum += p.currHealth;
            healhList.Add(Tuple.Create(random_sum, p));
        }

        // get a random number between 0 and the total amount of player health
        // TODO I beleive random_sum might need to be random_sum += 1, but not sure
        int health_helper = UnityEngine.Random.Range(0, random_sum);

        foreach (var tuple in healhList)
        {
        
            int chance = tuple.Item1;
            Player player = tuple.Item2;
            // select the player if the random number falls within their health range
            if (health_helper <= chance)
            {
                // spawn the door of the players color
                door = mySpawnManager.SpawnDoor(player.GetComponent<Renderer>().material.color);
                player.OnFart();
                fartingPlayer = player;
                break;
            }
        }
    }

    public void AddPlayer(Player p){
        players.Add(p);
        p.maxHealth = 5;
        p.currHealth = 5;
        Debug.Log("Player Added");

    }

    public void handleSandBoxChange()
    {
        // delete the sandbox items
        Item[] items = FindObjectsOfType<Item>();
        foreach (Item i in items)
        {
            if (i.itemState == ItemState.InEffect) // if the item is in effect, force its expiration
            {
                i.ForceExpiration();
            }
            else // if the item is in inventory or uncollected, destroy the item
            { 
                Destroy(i.gameObject);
            }
        }

        // force removal of all player effects
        Effect[] effects = FindObjectsOfType<Effect>();
        foreach (Effect e in effects)
        {
            e.ForceExpireEffect();
        }

        // delete items from player inventory
        foreach (Player p in players)
        {
            p.ResetItem1();
            p.ResetItem2();
        }
    }
    
    //public void AddPlayerObstacles()
    //{

    //    GameObject obstacle = Instantiate(obsticlePrefab, randomPos, Quaternion.identity);

    //    if (Physics2D.OverlapCircleAll(randomPos, 14f).Length > 1)
    //    {
    //        Destroy(obstacle);
    //    }
    //    else
    //    {
    //        obstacle.GetComponent<Renderer>().material.color = playe;
    //        obstacleList.Add(obstacle);
    //    }
    //}

}



public enum GameState {
    Menu,
    ItemPhase,
    CombatPhase,
    GameOver
}