using UnityEngine;
using TMPro;

public class ScoreTracker : MonoBehaviour
{ 
    //this class is used to display the current score of the user.

    public static int Score;
    //Score made static to get display in top 3 scores if needed as this script gets destroyed when scoreUI is loaded.

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
