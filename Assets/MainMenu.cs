using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void startMultiplayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);    
    }
    public void quit()
    {
        Debug.Log("Quitting");
        UnityEditor.EditorApplication.isPlaying=false;
    }

}
