using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{

    public bool GameOver= true;
    public bool CoopModeEnabled = true;
    [SerializeField]
    private GameObject _PlayerPrefab;
    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    private Animator _pauseAnimator;

    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("Spawn_manager").GetComponent<SpawnManager>();
        _pauseAnimator = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();
        _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    // Update is called once per frame
	void Update () {
        if (GameOver)
        {
            if (Input.GetKey(KeyCode.Return))
            {
                Instantiate(_PlayerPrefab, Vector3.zero, Quaternion.identity);

                GameOver = false;
                _uiManager.DisplayMainMenuImage(false);
                if (_spawnManager != null)
                {
                    _spawnManager.StartSpawning();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Main_Menu");
            }
        }
        else {
            if (Input.GetKeyDown(KeyCode.P))
            {
                PauseGame();
            }
        }
	    
	}

    public void ClearSceneFromExistingPlayers() {
        GameObject existingPlayers = GameObject.Find("Co-Op_Players(Clone)");
        if (existingPlayers != null)
        {
            Destroy(existingPlayers.gameObject);
        }
    }

    public void ClearSceneFromExistingEnemies()
    {
        GameObject existingPlayers = GameObject.Find("Enemy(Clone)");
        if (existingPlayers != null)
        {
            Destroy(existingPlayers.gameObject);
        }
    }

    private void PauseGame()
    {
        _uiManager.DisplayPauseMenu(true);
        _pauseAnimator.SetBool("IsPaused", true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        _uiManager.DisplayPauseMenu(false);
    }

}
