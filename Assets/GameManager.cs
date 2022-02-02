using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;

    private float fartTime = 20;
    private float currTime;

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
        if (State == GameState.Menu){
            currTime = currTime + Time.deltaTime;
            if (currTime >= fartTime) {
                UpdateGameState(GameState.Fart);
            }
        }
    }
 
    public void UpdateGameState(GameState newState) {
        State = newState;
        switch (newState) {
            case GameState.Menu:
                HandleMenu();
                break;
            case GameState.Fart:
                HandleFart();
                break;
            case GameState.Fight:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

    public void HandleMenu() {
        currTime = 0;
    }

    public void HandleFart() {
        currTime = 0;
        Debug.Log("Ayo We Farting Lads");
        UpdateGameState(GameState.Fight);
    }

    public void HandleFight(){
        Debug.Log("Fight Fight Fight!");

    }
    
}


public enum GameState {
    Menu,
    Fart,
    Fight,
}