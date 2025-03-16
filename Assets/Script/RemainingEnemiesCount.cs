using System;
using UnityEngine;
using TMPro;

public class RemainingEnemiesCount : MonoBehaviour
{ 
    //make it a singleton
    public static RemainingEnemiesCount Instance { get; private set; }


    [Header("References")] 
    [SerializeField] private Transform ennemiesHolder;
    [SerializeField] private TextMeshProUGUI remainingEnemiesText;
    
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
    }

    private void UpdateText()
    {
        remainingEnemiesText.text = "Enemies: " + remainingEnemies;
    }
    
    public void DecreaseRemainingEnemies()
    {
        remainingEnemies--;
        UpdateText();
    }
}
