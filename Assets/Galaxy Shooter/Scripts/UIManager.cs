using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    
    public Image _livesImage;
    public Sprite[] _livesSprite;
    public GameObject mainMenuScreen;
    public GameObject pauseMenuPanel;
    public GameObject controls;
    private GameManager _gameManager;

    public Text ScoreText, BestScoreText;
    public int Score, BestScore;
    [SerializeField]
    private bool controlsDisplayed;

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (!_gameManager.CoopModeEnabled)
        {
            BestScore = PlayerPrefs.GetInt("HighScore", 0);
            BestScoreText.text = BestScore > 0 ? "Best:  " + BestScore : "";
        }
    }
    public void UpdateLives(int livesLeft)
    {
        _livesImage.sprite = _livesSprite[livesLeft];
    }

    public void UpdateScore ()
    {
        Score += 10;
        ScoreText.text = "Score: " + Score;
    }
    private void UpdateBestScore() {
        if (Score > BestScore)
        {
            BestScore = Score;
            BestScoreText.text = "Best:  " + BestScore;
            PlayerPrefs.SetInt("HighScore", BestScore);
        }
    }

    public void DisplayMainMenuImage(bool visible)
    {
        if (!_gameManager.CoopModeEnabled)
        {
            UpdateBestScore();
        }
        Score = 0;
        ScoreText.text = "";
        mainMenuScreen.SetActive(visible);
        controls.SetActive(visible);
        if (controlsDisplayed) {
            SetControlsVisibility();
        }
    }

    public void DisplayPauseMenu(bool visible)
    {
        pauseMenuPanel.SetActive(visible);
    }

    public void ResumeGame()
    {        
        _gameManager.ResumeGame();
    }

    public void LoadMainMenuScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main_Menu");
    }

    public void SetControlsVisibility() {
        int movementDistance = _gameManager.CoopModeEnabled ? 50: 35;
        if (!controlsDisplayed)
        {
            controls.transform.position += new Vector3(0, movementDistance, 0);
            controlsDisplayed = true;
        }
        else {
            controls.transform.position += new Vector3(0, movementDistance * -1, 0);
            controlsDisplayed = false;
        }
    }
}
