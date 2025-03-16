using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    
    public static ScoreManager Instance { get; private set; }
    private int score;
    
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
        
        score = 0;
        UpdateText();
    }
    
    private void UpdateText()
    {
        scoreText.text = "Score: " + score;
    }
    
    public void IncreaseScore(int amount)
    {
        score += amount;
        UpdateText();
    }
}
