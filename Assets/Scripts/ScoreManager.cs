using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public float score = 0f;
    public TextMeshProUGUI scoreText;

    public void UpdateScore(float amount)
    {
        score += amount;
        score = (float)System.Math.Round(score, 2);

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
        scoreText.SetText(score.ToString());
    }
}
