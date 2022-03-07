using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeButton : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 SandboxScreenCoordinates;
    public bool isFartRoyale;

    private void Awake()
    {
        
        SandboxScreenCoordinates = FindObjectOfType<Camera>().WorldToScreenPoint(this.transform.position);
    }

    public void EnableFartRoyale()
    {
        GameManager.Instance.isFartRoyale = !GameManager.Instance.isFartRoyale;
        CameraManager.Instance.setCameraSize(CameraState.Large);
    }
}
