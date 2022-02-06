using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private List<Player> players = new List<Player>();

    public GameState State;

    public GameObject doorPrefab;

    public static event Action<GameState> OnGameStateChanged;

    private float minDoorSpawnTime = 7.0f;
    private float maxDoorSpawnTime = 20.0f;
    private float timer = 0.0f;
    private float nextTime;

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
            if (players.Count >=2)
            {
                Debug.Log("Starting Game");
                UpdateGameState(GameState.ItemPhase);
            }
        }
        if (State == GameState.ItemPhase){
            timer += Time.deltaTime;
            if (timer >= nextTime) {
                UpdateGameState(GameState.CombatPhase);
            }
        }
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
        timer = 0.0f;
        nextTime = UnityEngine.Random.Range(minDoorSpawnTime, maxDoorSpawnTime);
    }

    public void HandleFart() {
        Debug.Log("Ayo We Farting Lads");
        // SPAWN A DOOR IN A RANDOM LOCATION
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(UnityEngine.Random.Range(0, Screen.width), UnityEngine.Random.Range(0, Screen.height), Camera.main.farClipPlane / 2));
        Instantiate(doorPrefab, pos, Quaternion.identity);

        // GET A RANDOM PLAYER
        int random_player = UnityEngine.Random.Range(0, players.Count);
        players[random_player].OnFart();
        Debug.Log("Player " + random_player + " is farting");
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
}