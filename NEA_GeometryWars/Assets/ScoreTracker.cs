using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTracker : DisplayTopUserAndScores
{ 
    private int Score;
    private RandomSpawner ToGetScore;
    private TextMeshProUGUI DisplayScore;
    void Start()
    {
        Score = 0;
        ToGetScore = GameObject.FindObjectOfType<RandomSpawner>();
        DisplayScore = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        Score = ToGetScore.CurrentScore;
        DisplayScore.text = "Score: " + Score;
    }
}
