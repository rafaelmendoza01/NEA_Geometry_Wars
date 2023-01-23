using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTracker : MonoBehaviour
{ 
    private int Score;
    private RandomSpawner ToGetStats;
    private TextMeshProUGUI DisplayScore;
    void Start()
    {
        Score = 0;
        ToGetStats = GameObject.FindObjectOfType<RandomSpawner>();
        DisplayScore = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        Score = ToGetStats.CurrentScore;
        DisplayScore.text = "Score: " + Score;
    }
}
