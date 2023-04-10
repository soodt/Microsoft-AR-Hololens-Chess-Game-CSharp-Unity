using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public void startMultiplayer()
    {
        Debug.Log("Multiplayer mode");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
    public void startSingleplayer()
    {

    }
    public void quit()
    {
        Debug.Log("Quitting");
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
