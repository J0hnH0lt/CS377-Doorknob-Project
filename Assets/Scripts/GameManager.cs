using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public GameObject doorPrefab;

    public static event Action<GameState> OnGameStateChanged;

    private float fartTime = 10;
    private float currTime;

    private float minDoorSpawnTime = 10.0f;
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
        UpdateGameState(GameState.ItemPhase);
    }

    public void HandleItemPhase()
    {
        timer = 0.0f;
        nextTime = UnityEngine.Random.Range(minDoorSpawnTime, maxDoorSpawnTime);
        Debug.Log(nextTime);
    }

    public void HandleFart() {
        Debug.Log("Ayo We Farting Lads");
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(UnityEngine.Random.Range(0, Screen.width), UnityEngine.Random.Range(0, Screen.height), Camera.main.farClipPlane / 2));
        Instantiate(doorPrefab, pos, Quaternion.identity);
    }

    public void HandleFight(){
        Debug.Log("Fight Fight Fight!");

    }
    
}


public enum GameState {
    Menu,
    ItemPhase,
    CombatPhase,
}