using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public SpawnManager mySpawnManager;

    public PlayerInputManager inputManager;

    public GameObject obsticlePrefab;

    public List<Player> players = new List<Player>();

    public Player fartingPlayer;

    public List<Color> playerColors = new List<Color>();

    public List<GameObject> playerObsticles = new List<GameObject>();

    public GameState State;

    public Player winner;

    public static event Action<GameState> OnGameStateChanged;

    private float minDoorSpawnTime = 2.0f;
    private float maxDoorSpawnTime = 4.0f;
    private float doorTimer = 0.0f;
    private float nextDoorTime;
    private GameObject door;

    private float gameOverTimer = 0.0f;
    private float gameOverTime;

    private bool isStartAfterSandbox = true;

    public bool isFartRoyale;
    public bool fartRoyaleTickComplete = false;


    void Awake() {
        Instance = this;
        isFartRoyale = true;

    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateGameState(GameState.Menu);
        inputManager = PlayerInputManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (State == GameState.Menu)
        {
            if (players.Count >= 2 && PlayersReady())
            {
                foreach (Player p in players)
                {
                    p.myReadyUpIcon.color = Color.clear;
                    playerColors.Add(p.playerColor);
                }
                TurnJoinOff();
                UpdateGameState(GameState.ItemPhase);
                GameTextManager.Instance.GameRunning();
            }
        }
        if (State == GameState.ItemPhase) {
            doorTimer += Time.deltaTime;
            if (doorTimer >= nextDoorTime) {
                UpdateGameState(GameState.CombatPhase);
            }

        }
        if (State == GameState.CombatPhase)
        {
            CheckForGameOver();
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
                TurnJoinOn();
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

        if (isFartRoyale)
        {
            //Debug.Log("Fart Royale Tick");
            CameraManager.Instance.FartRoyaleTick();
        }

        if (isStartAfterSandbox)
        {
            handleSandBoxChange();
            isStartAfterSandbox = false;
        }

        if (door != null)
        {
            fartingPlayer.DisableTrailSlow();
            fartingPlayer = null;
            Destroy(door);
        }

        // spawn the door
        doorTimer = 0.0f;
        nextDoorTime = UnityEngine.Random.Range(minDoorSpawnTime, maxDoorSpawnTime);
    }

    public void HandleGameOver()
    {
        winner = players[0];
        winner.GetComponent<Boundaries>().enabled = false;
        CameraManager.Instance.setCameraSize(CameraState.Tiny);

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
        int health_helper = UnityEngine.Random.Range(1, random_sum+1);

        foreach (var tuple in healhList)
        {

            int chance = tuple.Item1;
            Player player = tuple.Item2;
            // select the player if the random number falls within their health range
            if (health_helper <= chance)
            {
                // spawn the door of the players color
                door = mySpawnManager.SpawnDoor(player.playerColor);
                player.OnFart();
                fartingPlayer = player;
                break;
            }
        }
    }

    public void AddPlayer(Player p)
    {
        players.Add(p);
        p.maxHealth = 8;
        p.currHealth = 8;
        //Debug.Log("Player Added");

    }

    public void RemovePlayer(int id)
    {
        for(int i = 0; i < players.Count; i++)
        {
            if (players[i].GetComponent<Player>().id == id)
            {
                players.RemoveAt(i);
                continue;
            }
        }
        if (players.Count != 1)
        {
            GameManager.Instance.UpdateGameState(GameState.ItemPhase);
        }
        //Debug.Log("Player Removed");

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

        CameraManager.Instance.setCameraSize(CameraState.Large);

        Destroy(FindObjectOfType<GameModeButton>().gameObject);
        Destroy(FindObjectOfType<CameraSizeButton>().gameObject);


    }

    public void TurnJoinOff()
    {
        //inputManager.DisableJoining();
        PlayerInputManager.instance.DisableJoining();
    }

    public void TurnJoinOn()
    {
        //inputManager.EnableJoining();
        PlayerInputManager.instance.EnableJoining();
    }

    public void CheckForGameOver()
    {
        if (players.Count == 1)
        {
            GameTextManager.Instance.GameOver();
            GameManager.Instance.UpdateGameState(GameState.GameOver);
        }
    }

}



public enum GameState {
    Menu,
    ItemPhase,
    CombatPhase,
    GameOver
}