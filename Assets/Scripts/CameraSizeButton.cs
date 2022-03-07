using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSizeButton : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 SandboxScreenCoordinates;

    private void Awake()
    {
        SandboxScreenCoordinates = FindObjectOfType<Camera>().WorldToScreenPoint(this.transform.position);
    }
}
