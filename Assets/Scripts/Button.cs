using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }    
    public void ExitGame()
    {
        Application.Quit();
    }
}