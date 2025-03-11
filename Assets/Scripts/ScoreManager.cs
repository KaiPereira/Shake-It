using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int score = 10302;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        scoreText.SetText(score.ToString());
    }

    void Update()
    {
        
    }
}
