using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadSinglePlayerGame()
    {
        Debug.Log("single");
        SceneManager.LoadScene("Single_Player");
    }
    public void LoadCoOpGame()
    {
        Debug.Log("co-op");
        SceneManager.LoadScene("Co-Op_Mode");
    }
}
