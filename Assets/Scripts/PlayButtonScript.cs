using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayButtonPressed()
    {
        Debug.Log("Hit Play");
        SceneManager.LoadScene("MVP");
    }

}
