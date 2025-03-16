using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class RemainingEnemiesCount : MonoBehaviour
{ 
    //make it a singleton
    public static RemainingEnemiesCount Instance { get; private set; }


    [Header("References")] 
    [SerializeField] private Transform ennemiesHolder;
    [SerializeField] private TextMeshProUGUI remainingEnemiesText;
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    
    
    [HideInInspector] public bool gameEnded = false;
    private int remainingEnemies;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        remainingEnemies = ennemiesHolder.childCount;
        UpdateText();
        endGamePanel.SetActive(false);
    }

    private void UpdateText()
    {
        remainingEnemiesText.text = "Enemies: " + remainingEnemies;
    }
    
    public void DecreaseRemainingEnemies()
    {
        remainingEnemies--;
        UpdateText();
        if (remainingEnemies == 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        StartCoroutine(SlowDownTime(3f));
    }

    IEnumerator SlowDownTime(float f)
    {
        gameEnded = true;
        float t = 0f;
        while (t < f)
        {
            t += Time.unscaledDeltaTime; //Time.deltaTime doesn't work here since we are slowing down time
            Time.timeScale = Mathf.Lerp(1f, 0f, t / f);
            yield return null;
        }
        Time.timeScale = 0f;
        endGamePanel.SetActive(true);
        scoreText.text = "Score: " + ScoreManager.Instance.GetScore();
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0);
        ScoreManager.Instance.UpdateHighScore();
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(1); //scene 1 is the game scene
    }
    
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); //scene 0 is the main menu scene
    }
}
