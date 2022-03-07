using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{

    public Camera MainCamera;

    private float cameraSize = 40f;

    public CameraState currState;

    public static CameraManager Instance;

    private float lerpSpeed = .09f;

    private float minCameraSize = 16f;

    private void Awake()
    {
        MainCamera = FindObjectOfType<Camera>();
        Instance = this;
        currState = CameraState.Medium;
    }

    public void setCameraSize(CameraState camState)
    { 
        switch (camState)
        {
            case CameraState.Small:
                cameraSize = 25f;
                currState = CameraState.Small;
                break;
            case CameraState.Medium:
                cameraSize = 50f;
                currState = CameraState.Medium;
                break;
            case CameraState.Large:
                cameraSize = 75f;
                currState = CameraState.Large;
                break;
        }
        //MainCamera.orthographicSize = cameraSize;
    }

    public void ToggleMapSize()
    {
        if (currState == CameraState.Small)
        {
            setCameraSize(CameraState.Medium);
        }
        else if (currState == CameraState.Medium)
        {
            setCameraSize(CameraState.Large);
        }
        else if (currState == CameraState.Large)
        {
            setCameraSize(CameraState.Small);
        }
    }

    public void LateUpdate()
    {
        MainCamera.orthographicSize = Mathf.Lerp(MainCamera.orthographicSize, cameraSize, lerpSpeed);
        if(GameManager.Instance.State == GameState.Menu)
        {
            MoveItemsOnScreen();
        }
    }

    public void MoveItemsOnScreen()
    {
        Item[] items = FindObjectsOfType<Item>();
        foreach (Item i in items)
        {
            if (i!=null && i.isSandBox)
            {
                i.transform.position = MainCamera.ScreenToWorldPoint(i.SandboxScreenCoordinates);
            }
        }
        CameraSizeButton button = FindObjectOfType<CameraSizeButton>();
        button.transform.position = MainCamera.ScreenToWorldPoint(button.SandboxScreenCoordinates);

        Player[] players = FindObjectsOfType<Player>();
        foreach (Player p in players)
        {
            if (p != null)
            {
                p.transform.position = MainCamera.ScreenToWorldPoint(p.SandboxScreenCoordinates);
            }
        }

        GameModeButton gameModeButton = FindObjectOfType<GameModeButton>();
        gameModeButton.transform.position = MainCamera.ScreenToWorldPoint(gameModeButton.SandboxScreenCoordinates);
    }

    public void FartRoyaleTick()
    {
        if (MainCamera.orthographicSize > minCameraSize)
        {
            cameraSize = cameraSize - 5.0f;

        }
    }
}

public enum CameraState
{
    Small,
    Medium,
    Large,
}