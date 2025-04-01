using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private float score = 10f;
    public TextMeshProUGUI scoreText;

    public void Start()
    {
        UpdateUI();
    }

    public void UpdateScore(float amount)
    {
        score += amount;

        UpdateUI();
    }

    public bool BuyUpgrade(float cost)
    {
        if (score - cost < 0) return false;
        
        score -= cost;

        UpdateUI();

        return true;
    }

    public void UpdateUI()
    {
        RoundScore();
        scoreText.SetText(score.ToString());
    }

    private void RoundScore()
    {
        score = (float)System.Math.Round(score, 2);
    }
}
