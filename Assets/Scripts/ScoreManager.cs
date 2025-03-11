using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public float score = 0f;
    public TextMeshProUGUI scoreText;

    void Update()
    {
        scoreText.SetText(score.ToString());
    }

    public void UpdateScore(float amount)
    {
        score += amount;
        score = (float)System.Math.Round(score, 2);
    }
}
